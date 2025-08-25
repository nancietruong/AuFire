using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    private int currentWaveIndex;
    private RoomData currentRoomData;
    private Transform[] currentSpawnPoints;
    private int enemiesAlive;
    private RoomTrigger currentRoomTrigger;

    public void StartRoomWaves(RoomData roomData, Transform[] spawnPoints, RoomTrigger roomTrigger)
    {
        currentRoomData = roomData;
        currentSpawnPoints = spawnPoints;
        currentRoomTrigger = roomTrigger;
        currentWaveIndex = 0;
        StartCoroutine(SpawnWavesCoroutine());
    }

    private IEnumerator SpawnWavesCoroutine()
    {
        while (currentWaveIndex < currentRoomData.waves.Length)
        {
            WaveData wave = currentRoomData.waves[currentWaveIndex];
            yield return StartCoroutine(SpawnWave(wave, currentSpawnPoints));
            while (enemiesAlive > 0)
                yield return null;
            yield return new WaitForSeconds(wave.delayAfterWave);
            currentWaveIndex++;
        }
        currentRoomTrigger.OnRoomCleared();
    }

    IEnumerator SpawnWave(WaveData waveData, Transform[] spawnPoints)
    {
        foreach (var enemyGroup in waveData.enemies)
        {
            for (int i = 0; i < enemyGroup.count; i++)
            {
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyGroup.enemyPrefab, point.position, Quaternion.identity);
                enemiesAlive++;
                enemy.AddComponent<EnemyDeathListener>().Init(this);
                yield return new WaitForSeconds(enemyGroup.spawnRate);
            }
        }
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
    }
}

public class EnemyDeathListener : MonoBehaviour, ITakeDamage
{
    private WaveManager waveManager;
    private bool isDead = false;

    public void Init(WaveManager manager)
    {
        waveManager = manager;
    }

    public void TakeDamage(float dmg)
    {
        if (!isDead)
        {
            EnemyController ec = GetComponent<EnemyController>();
            if (ec != null)
            {
                ec.TakeDamage(dmg);
                if (ec.health <= 0)
                {
                    isDead = true;
                    waveManager.OnEnemyDied();
                }
            }
        }
    }
}
