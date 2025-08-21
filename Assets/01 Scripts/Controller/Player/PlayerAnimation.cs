using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    static readonly int SpeedHash = Animator.StringToHash("Speed");
    static readonly int isRollingHash = Animator.StringToHash("isRolling");
    static readonly int isDeadHash = Animator.StringToHash("isDead");

    Animator animator;
    PlayerController player;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<PlayerController>();
    }


    public void UpdateBlendTree()
    {
        animator.SetFloat(SpeedHash, player.playerRB.velocity.sqrMagnitude);
    }

    public void SetRolling(bool isRolling)
    {
        animator.SetBool(isRollingHash, isRolling);
    }

    public void SetDead(bool isDead)
    {
        animator.SetBool(isDeadHash, isDead);
    }
}
