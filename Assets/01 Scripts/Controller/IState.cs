using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : MonoBehaviour
{
    void Enter(T owner);

    void Execute(T owner);
    void Exit(T owner);
}
