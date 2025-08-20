using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState<EnemyController>
{
    public void Enter(EnemyController owner)
    {
    }

    public void Execute(EnemyController owner)
    {
        owner.ChasePlayer();
        
        if(owner.FindPlayer() == false)
        {
            owner.enemyStateMachine.ChangeState(new PatrolState());
            return;
        }
    }

    public void Exit(EnemyController owner)
    {
    }
}
