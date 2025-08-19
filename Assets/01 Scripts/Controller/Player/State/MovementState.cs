using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : IState<PlayerController>
{
    public void Enter(PlayerController player)
    {
    }

    public void Execute(PlayerController player)
    {
        player.PlayerMoving();

        if (player.isDodging)
        {
            player.playerStateMachine.ChangeState(new DodgeRollState());
        }

    }

    public void Exit(PlayerController player)
    {
    }
}
