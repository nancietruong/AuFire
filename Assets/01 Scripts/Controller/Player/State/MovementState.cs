using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : IState
{
    PlayerController player;
    PlayerStateMachine sm;

    public MovementState(PlayerController player, PlayerStateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        player.PlayerMoving();
        
    }

    public void Exit()
    {
    }
}
