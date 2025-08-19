using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollState : IState<PlayerController>
{
    float timer;
    public void Enter(PlayerController player)
    {
        player.playerAnimation.SetRolling(true);
        timer = player.dodgeRollCooldown;
    }

    public void Execute(PlayerController player)
    {

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            player.playerStateMachine.ChangeState(new MovementState());
        }

    }

    public void Exit(PlayerController player)
    {
        player.playerAnimation.SetRolling(false);
    }
}
