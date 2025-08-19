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
    }

    public void Exit(EnemyController owner)
    {
    }
}
