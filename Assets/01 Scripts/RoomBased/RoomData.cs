using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoomData", menuName = "Game/Room Data")]
public class RoomData : ScriptableObject
{
    public WaveData[] waves;
}
