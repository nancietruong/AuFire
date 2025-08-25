using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawn
    {
        public GameObject enemyPrefab;
        public int count = 5;
        public float spawnRate = 1f;
    }

    public EnemySpawn[] enemies;
    public float delayAfterWave = 2f;
}