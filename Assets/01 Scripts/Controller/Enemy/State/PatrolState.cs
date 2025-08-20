using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<EnemyController>
{
    public void Enter(EnemyController enemy)
    {
        
    }

    public void Execute(EnemyController enemy)
    {

        enemy.Patrol();
        if (enemy.FindPlayer())
        {
            enemy.enemyStateMachine.ChangeState(new ChaseState());
            return;
        }
    }

    public void Exit(EnemyController enemy)
    {
        
    }
}
