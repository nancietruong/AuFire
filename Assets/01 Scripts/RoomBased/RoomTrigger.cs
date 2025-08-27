using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public enum BattleState
    {
        Idle,
        InProgress,
        Cleared
    }

    public RoomData roomData;
    public Transform[] spawnPoints;
    public DoorInteraction door;
    public GameObject keyPrefab;
    public Transform spawnKey;

    bool isPlayerInRoom = false;
    BattleState battleState;

    private void Awake()
    {
        battleState = BattleState.Idle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerInRoom)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            isPlayerInRoom = true;
            if (battleState == BattleState.Idle)
            {
                StartBattle();
            }
        }
    }

    void StartBattle()
    {
        Debug.Log("Start Battle in " + roomData.name);
        battleState = BattleState.InProgress;
        WaveManager.Instance.StartRoomWaves(roomData, spawnPoints, this);
    }

    public void OnRoomCleared()
    {
        battleState = BattleState.Cleared;
        Debug.Log("Room Cleared: " + roomData.name);
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, spawnKey.position, Quaternion.identity);
            AudioManager.PlaySound(TypeOfSoundEffect.Key);
        }
    }
}
