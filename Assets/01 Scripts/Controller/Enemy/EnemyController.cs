using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimation))]
public class EnemyController : MonoBehaviour
{
    [Header("Basic Settings")]
    Rigidbody2D enemy;
    Vector2 movement;
    [SerializeField] float speed;
    [SerializeField] Transform player;
    EnemyAnimation enemyAnimation;
    public StateMachine<EnemyController> enemyStateMachine;

    [Header("AI Settings")]
    [SerializeField] LayerMask layerMask, playerLayerMask;
    [SerializeField] float detectionRange;

    private void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    private void Start()
    {
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
            movement = Vector2.zero;
            return;
        }
        FlipToPlayer();
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
        if (player == null)
        {
            movement = Vector2.zero;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
