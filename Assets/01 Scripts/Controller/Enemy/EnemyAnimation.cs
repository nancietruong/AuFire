using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    static readonly int xVelocityHash = Animator.StringToHash("xVelocity");
    static readonly int isDeadHash = Animator.StringToHash("isDead");

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void UpdateBlendTree(float xVelocity)
    {
        animator.SetFloat(xVelocityHash, xVelocity);
    }
}
