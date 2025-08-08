using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollState : IState
{
    PlayerController p;
    PlayerStateMachine sm;
    float timer;
    public DodgeRollState(PlayerController player, PlayerStateMachine stateMachine)
    {
        p = player;
        sm = stateMachine;
    }

    public void Enter()
    {
        p.playerAnimation.SetRolling(true);
        timer = p.dodgeRollCooldown;
    }

    public void Execute()
    {

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(new MovementState(p, sm));
        }

    }

    public void Exit()
    {
        p.playerAnimation.SetRolling(false);
    }
}
