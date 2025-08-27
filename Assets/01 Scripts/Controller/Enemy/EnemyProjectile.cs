using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed, damage, lifetime;
    public GameObject projectilePrefab;
    Vector2 direction = Vector2.zero;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    public void ShootProjectile(Vector2 firePosition, Vector2 direction, float speed, float damage, float lifetime)
    {
        transform.position = firePosition;
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
        ITakeDamage damagable = collision.gameObject.GetComponent<ITakeDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
        }
    }
}
