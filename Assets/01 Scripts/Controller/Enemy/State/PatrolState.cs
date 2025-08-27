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
        if (GameManager.State != GameState.Playing) return;

        enemy.Patrol();

        if (enemy.FindPlayer())
        {
            float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);
            if (distance <= enemy.attackRange)
            {
                enemy.enemyStateMachine.ChangeState(new AttackState());
            }
            else
            {
                enemy.enemyStateMachine.ChangeState(new ChaseState());
            }
            return;
        }
    }

    public void Exit(EnemyController enemy)
    {
        
    }
}
