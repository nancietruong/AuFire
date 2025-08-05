using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    static readonly int SpeedHash = Animator.StringToHash("Speed");

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
}
