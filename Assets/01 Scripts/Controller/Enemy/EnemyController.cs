using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimation))]
public class EnemyController : MonoBehaviour, ITakeDamage
{
    [Header("Basic Settings")]
    Rigidbody2D enemy;
    Vector2 movement;
    [SerializeField] float speed;
    [SerializeField] public Transform player;
    EnemyAnimation enemyAnimation;
    public StateMachine<EnemyController> enemyStateMachine;
    [SerializeField] MaterialTintColor materialTintColor;
    [SerializeField] Transform avatar;
    Animator animator;

    [Header("Health Settings")]
    public float health;
    public float maxHealth = 100f;
    EnemyHealth enemyHealth;

    [Header("AI Settings")]
    [SerializeField] LayerMask layerMask, playerLayerMask;
    [SerializeField] public float detectionRange;
    public float attackRange;

    [Header("Wander Settings")]
    [SerializeField] float wanderRadius = 3f;
    [SerializeField] float wanderInterval = 2f;
    Vector2 wanderTarget;
    float wanderTimer;

    [Header("Loot Settings")]
    public List<LootItem> lootTable = new List<LootItem>();

    [Header("Attack Settings")]
    public EnemyProjectile projectilePrefab;
    public float projectileSpeed = 8f;
    public float projectileDamage = 10f;
    public float projectileLifetime = 2f;
    public float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    [Header("Knockback Settings")]
    [SerializeField] float knockbackForce = 8f;
    [SerializeField] float knockbackDuration = 0.15f;
    private float knockbackTimer = 0f;
    private Vector2 knockbackDirection;

    private WaveManager waveManager;
    private bool isDead = false;

    private void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        materialTintColor = GetComponent<MaterialTintColor>();
        enemyHealth = GetComponentInChildren<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        health = maxHealth;
        enemyStateMachine = new StateMachine<EnemyController>(this);
        enemyStateMachine.ChangeState(new PatrolState());
    }

    private void Update()
    {
        if (GameManager.State != GameState.Playing) return;
        enemyStateMachine.UpdateState();
        FindPlayer();
        enemyAnimation.UpdateBlendTree(enemy.velocity.sqrMagnitude);

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        if (isDetectObstacle())
        {
            player = null;
            return;
        }
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > detectionRange)
            {
                player = null;
            }
        }

        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (GameManager.State != GameState.Playing)
        {
            enemy.velocity = Vector2.zero;
            if (enemyAnimation != null)
            {
                if (animator != null) animator.speed = 0f;
            }
            return;
        }

        if (knockbackTimer > 0f)
        {
            enemy.velocity = knockbackDirection * knockbackForce;
            return;
        }

        enemy.velocity = movement * speed;
        if (enemyAnimation != null)
        {
            if (animator != null) animator.speed = 1f;
        }
    }

    public bool FindPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayerMask);
        if (collider != null)
        {
            player = collider.transform;
            return true;
        }
        return false;
    }

    public void ChasePlayer()
    {

        FlipToPlayer();
        if (player == null)
        {
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;
    }

    void FlipToPlayer()
    {
        if (player == null) return;
        Vector3 scale = avatar.transform.localScale;
        if (player.position.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x) * -1f;
        else
            scale.x = Mathf.Abs(scale.x);
        avatar.transform.localScale = scale;
    }

    bool isDetectObstacle()
    {
        if (player == null) return true;

        Vector2 direction = (player.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, layerMask);
        if (hit.collider == null) return false;
        if (hit.collider.CompareTag("Player")) return false;

        Debug.DrawLine(transform.position, hit.point, Color.red);
        Debug.DrawRay(hit.point, hit.normal, Color.yellow);

        return true;
    }

    public void Patrol()
    {
        wanderTimer -= Time.deltaTime;

        FlipToMovement();

        if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.2f)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized * Random.Range(1f, wanderRadius);
            wanderTarget = (Vector2)transform.position + randomDirection;
            wanderTimer = wanderInterval;
        }

        Vector2 direction = (wanderTarget - (Vector2)transform.position).normalized;
        movement = direction;
    }

    void FlipToMovement()
    {
        if (Mathf.Approximately(wanderTarget.x, transform.position.x)) return;
        Vector3 scale = avatar.transform.localScale;
        if (wanderTarget.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x) * -1f;
        else
            scale.x = Mathf.Abs(scale.x);
        avatar.transform.localScale = scale;
    }

    public void Init(WaveManager manager)
    {
        waveManager = manager;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        materialTintColor.SetTintColor(new Color(1, 0, 0, 1));
        enemyHealth.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            isDead = true;
            if (waveManager != null)
            {
                waveManager.OnEnemyDied();
            }
            Die();
        }
    }

    public void Die()
    {
        foreach (LootItem item in lootTable)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                if (item.itemPrefab != null)
                {
                    Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                }
            }
        }
        gameObject.SetActive(false);
    }

    public void Attack()
    {
        FlipToPlayer();
        if (attackTimer > 0f) return;
        if (player == null) return;

        Vector2 firePos = transform.position;
        Vector2 dir = ((Vector2)player.position - firePos).normalized;

        EnemyProjectile projectile = ObjectPooling.Instance.GetCOMP<EnemyProjectile>(projectilePrefab);
        projectile.ShootProjectile(firePos, dir, projectileSpeed, projectileDamage, projectileLifetime);

        attackTimer = attackCooldown;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ITakeDamage damagable = collision.gameObject.GetComponent<ITakeDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(20f);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            ApplyKnockback(dir);
        }
    }

    public void ApplyKnockback(Vector2 direction)
    {
        knockbackDirection = direction;
        knockbackTimer = knockbackDuration;
    }

    public void ResetEnemy(Vector3 position, WaveManager manager)
    {
        transform.position = position;
        health = maxHealth;
        isDead = false;
        waveManager = manager;
        player = null;
        knockbackTimer = 0f;
        movement = Vector2.zero;
        if (enemyStateMachine == null)
            enemyStateMachine = new StateMachine<EnemyController>(this);
        enemyStateMachine.ChangeState(new PatrolState());
        gameObject.SetActive(true);

        if (enemyHealth == null)
            enemyHealth = GetComponentInChildren<EnemyHealth>();
        if (enemyHealth != null)
            enemyHealth.UpdateHealthBar(health, maxHealth);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
