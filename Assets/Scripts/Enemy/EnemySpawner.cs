using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name; // Name of the wave
        public List<EnemyGroup> enemyGroups; // List of enemy groups in the wave
        public int waveQuota; // Total number of enemies in the wave
        public float spawnInterval; // Time between each spawn
        public int spawnCount; // Number of enemies spawned so far
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount; // The number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;

    }

    public List<Wave> waves; // List of waves
    public int currentWaveCount; // The current wave number

    [Header("Spawner Attributes")]
    private float spawnTimer; // Timer to keep track of the spawn interval
    public int enemiesAlive; // Number of enemies currently alive
    public int maxEnemiesAllowed; // Maximum number of enemies allowed to be alive at once
    public bool maxEnemiesReached = false; // Check if the maximum number of enemies has been reached
    public float waveInterval; // Time between each wave

    [Header("Spawn Positions")]
    public List<Transform> spawnPositions; // List of spawn positions

    Transform player;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();

    }

    void Update()
    {
        // Check if the current wave has ended and start the next wave
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(StartNextWave());
        }

        spawnTimer += Time.deltaTime;

        // Check if the time has reached the spawn interval
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator StartNextWave()
    {
        
        yield return new WaitForSeconds(waveInterval); // Wait for the wave interval

        // Check if there are more waves to start after the current wave
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }


    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);

    }

    private void SpawnEnemies()
    {
        // Check if the wave has reached its quota
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            // Loop through each enemy group in the wave
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // Check if the enemy group has reached its quota
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Check if the maximum number of enemies has been reached
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    // Spawn the enemy prefab at a random spawn position around the player
                    Instantiate(enemyGroup.enemyPrefab, player.position + spawnPositions[Random.Range(0, spawnPositions.Count)].position, Quaternion.identity);

                    // create a random spawn position around the player
                    //Vector2 spawnPostion = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    //Instantiate(enemyGroup.enemyPrefab, spawnPostion, Quaternion.identity);

                    enemyGroup.spawnCount++; // Increment the spawn count of the enemy group
                    waves[currentWaveCount].spawnCount++; // Increment the spawn count of the wave
                    enemiesAlive++; // Increment the number of enemies alive
                }
            }
        }
 
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }

}
