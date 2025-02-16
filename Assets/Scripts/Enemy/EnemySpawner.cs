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
        public int allSpawnedEnemiesCount; // Number of enemies spawned so far
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnedCount; // The number of enemies of this type already spawned in this wave
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
    bool isWaveActive = false;
    public int totalEnemiesKilled = 0;

    [Header("Wave Timer")]
    public float waveTimeLimit = 60f; // Time limit for each wave
    private float waveTimer = 0f; // Timer to keep track of the wave time

    [Header("Spawn Positions")]
    public List<Transform> spawnPositions; // List of spawn positions

    Transform player;

    

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        waveTimer = 0f;

    }

    void Update()
    {
        waveTimer += Time.deltaTime;
        // Check if the current wave has ended and start the next wave
        if ((currentWaveCount < waves.Count && enemiesAlive == 0 && waves[currentWaveCount].allSpawnedEnemiesCount >= waves[currentWaveCount].waveQuota && !isWaveActive)
            || (waveTimer >= waveTimeLimit))
        {
            Debug.Log("Starting next wave...");
            maxEnemiesAllowed += 5; // Increase the maximum number of enemies allowed for the next wave
            waveTimer = 0f; // Reset the wave timer
            waveTimeLimit += 10f; // Increase the time limit for the next wave
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
        isWaveActive = true;

        yield return new WaitForSeconds(waveInterval); // Wait for the wave interval

        // Check if there are more waves to start after the current wave
        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
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
        if (waves[currentWaveCount].allSpawnedEnemiesCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            // Loop through each enemy group in the wave
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // Check if the enemy group has reached its quota
                if (enemyGroup.spawnedCount < enemyGroup.enemyCount)
                {

                    // Spawn the enemy prefab at a random spawn position around the player
                    Instantiate(enemyGroup.enemyPrefab, player.position + spawnPositions[Random.Range(0, spawnPositions.Count)].position, Quaternion.identity);

                    // create a random spawn position around the player
                    //Vector2 spawnPostion = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    //Instantiate(enemyGroup.enemyPrefab, spawnPostion, Quaternion.identity);

                    enemyGroup.spawnedCount++; // Increment the spawn count of the enemy group
                    waves[currentWaveCount].allSpawnedEnemiesCount++; // Increment the spawn count of the wave
                    enemiesAlive++; // Increment the number of enemies alive

                    // Check if the maximum number of enemies has been reached
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
 
    }

    public void OnEnemyKilled()
    {
        // Decrement the number of enemies alive
        enemiesAlive--;
        totalEnemiesKilled++;
        GameManager.instance.enemiesKilled.text = "Enemies Killed: " + totalEnemiesKilled;

        // Reset the maxEnemiesReached flag when the number of enemies alive is less than the maximum allowed
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

}
