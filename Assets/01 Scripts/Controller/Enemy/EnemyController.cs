using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimation))]
public class EnemyController : MonoBehaviour, ITakeDamage
{
    [Header("Basic Settings")]
    Rigidbody2D enemy;
    Vector2 movement;
    [SerializeField] float speed;
    [SerializeField] Transform player;
    EnemyAnimation enemyAnimation;
    public StateMachine<EnemyController> enemyStateMachine;
    [SerializeField] MaterialTintColor materialTintColor;

    [Header("Health Settings")]
    public float health;
    public float maxHealth = 100f;

    [Header("AI Settings")]
    [SerializeField] LayerMask layerMask, playerLayerMask;
    [SerializeField] float detectionRange;


    [Header("Wander Settings")]
    [SerializeField] float wanderRadius = 3f;
    [SerializeField] float wanderInterval = 2f;
    Vector2 wanderTarget;
    float wanderTimer;

    private void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        materialTintColor = GetComponent<MaterialTintColor>();
    }

    private void Start()
    {
        health = maxHealth;
        enemyStateMachine = new StateMachine<EnemyController>(this);
        enemyStateMachine.ChangeState(new PatrolState());
    }

    private void Update()
    {
        enemyStateMachine.UpdateState();
        FindPlayer();   
        enemyAnimation.UpdateBlendTree(enemy.velocity.sqrMagnitude);
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
    }

    private void FixedUpdate()
    {
        enemy.velocity = movement * speed;
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
        Vector3 scale = transform.localScale;
        if (player.position.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x) * -1f;
        else
            scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
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
        Vector3 scale = transform.localScale;
        if (wanderTarget.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x) * -1f;
        else
            scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        materialTintColor.SetTintColor(new Color(1, 0, 0, 1));
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
