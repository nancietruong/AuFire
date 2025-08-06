using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController player;
    void Init()
    {
        player.Init();
    }

    void Start()
    {
        Init();
    }
}
