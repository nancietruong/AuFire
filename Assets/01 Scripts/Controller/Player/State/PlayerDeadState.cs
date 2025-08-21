using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : IState<PlayerController>
{
    float deadTimer = 1f;
    public void Enter(PlayerController owner)
    {
        owner.playerAnimation.SetDead(true);
    }

    public void Execute(PlayerController owner)
    {
        // Handle player dead state logic
        deadTimer -= Time.deltaTime;

        if (deadTimer <= 0f)
        {
            Time.timeScale = 0f; // Stop the game
        }
    }

    public void Exit(PlayerController owner)
    {
        owner.playerAnimation.SetDead(false);
    }
}
