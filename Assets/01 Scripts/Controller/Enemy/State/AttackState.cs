using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<EnemyController>
{
    public void Enter(EnemyController enemy)
    {
    }

    public void Execute(EnemyController enemy)
    {
        if (GameManager.State != GameState.Playing) return;

        if (enemy.player == null || Vector2.Distance(enemy.transform.position, enemy.player.position) > enemy.detectionRange)
        {
            enemy.enemyStateMachine.ChangeState(new PatrolState());
            return;
        }

        if (Vector2.Distance(enemy.transform.position, enemy.player.position) > enemy.attackRange)
        {
            enemy.enemyStateMachine.ChangeState(new ChaseState());
            return;
        }
        enemy.Attack();
    }

    public void Exit(EnemyController enemy)
    {
    }
}
