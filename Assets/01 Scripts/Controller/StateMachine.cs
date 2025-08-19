using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    public IState<T> currentState;
    public T TOwner;

    public StateMachine(T owner)
    {
        this.TOwner = owner;
    }

    public void ChangeState(IState<T> newState)
    {
        if (currentState != null)
        {
            currentState.Exit(TOwner);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter(TOwner);
        }
    }

    public void UpdateState()
    {
        currentState?.Execute(TOwner);
    }
}
