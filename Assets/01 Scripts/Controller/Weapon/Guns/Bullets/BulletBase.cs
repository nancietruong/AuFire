using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected float speed, damage, lifetime;
    protected Vector2 direction = Vector2.zero;

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

    public void Init(float speed, float damage, float lifetime, Vector2 direction)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.direction = direction;
    }

    public abstract void Boom(GameObject target);

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
}
