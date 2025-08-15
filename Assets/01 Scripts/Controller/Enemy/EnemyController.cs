using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Basic Settings")]
    Rigidbody2D enemy;
    Vector2 movement;
    [SerializeField] float speed;
    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed;

    private void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        enemy.velocity = movement * speed;
    }

    void ChasePlayer()
    {
        if (player == null)
        {
            movement = Vector2.zero;
            return;
        }

        Vector2 direction = player.position - transform.position;

    }
}
