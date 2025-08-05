using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentState = newState;
        if (CurrentState != null)
        {
            CurrentState.Enter();
        }
    }

    public void UpdateState()
    {
        CurrentState?.Execute();
    }
}
