using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainLayer;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    public GameObject latestChunk;
    public float maxOptimizationDistance;
    private float opDistance;
    private float optimizationCooldown;
    public float optimizationCooldownDuration;


    private void Start()
    {
        playerLastPosition = player.transform.position;
    }

    private void Update()
    {
        ChunkChecker();
        ChunkOptimization();
    }

    private Dictionary<string, string[]> directionsToSpawn = new Dictionary<string, string[]>()
{
    { "Right", new string[] { "Right", "UpRight", "DownRight" } },
    { "Left",  new string[] { "Left", "UpLeft", "DownLeft" } },
    { "Up",    new string[] { "Up", "UpRight", "UpLeft" } },
    { "Down",  new string[] { "Down", "DownRight", "DownLeft" } },
    { "UpRight",  new string[] { "UpRight", "Up", "Right" } },
    { "UpLeft",   new string[] { "UpLeft", "Up", "Left" } },
    { "DownRight",new string[] { "DownRight", "Down", "Right" } },
    { "DownLeft", new string[] { "DownLeft", "Down", "Left" } },
};

    void ChunkChecker()
    {
        if (!currentChunk) return;

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;
        string directionName = GetDirectionName(moveDir);

        if (directionsToSpawn.TryGetValue(directionName, out var neighbors))
        {
            // neighbors je napø. {"Right", "UpRight", "DownRight"}
            foreach (var neighbor in neighbors)
            {
                CheckAndSpawnChunk(neighbor);
            }
        }
    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainLayer))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

        string GetDirectionName(Vector3 direction)
        {
            direction = direction.normalized;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Moving Horizontally
                if (direction.y > 0.5f)
                {
                    // also moving up
                    return direction.x > 0 ? "UpRight" : "UpLeft";
                }
                else if (direction.y < -0.5f)
                {
                    // also moving down
                    return direction.x > 0 ? "DownRight" : "DownLeft";
                }
                else
                {
                    // only moving horizontally
                    return direction.x > 0 ? "Right" : "Left";
                }
            }
            else
            {
                // Moving Vertically
                if (direction.x > 0.5f)
                {
                    // also moving right
                    return direction.y > 0 ? "UpRight" : "DownRight";
                }
                else if (direction.x < -0.5f)
                {
                    // also moving left
                    return direction.y > 0 ? "UpLeft" : "DownLeft";
                }
                else
                {
                    // only moving vertically
                    return direction.y > 0 ? "Up" : "Down";
                }
            }
        }


        void SpawnChunk(Vector3 spawnPosition)
        {
            int randomIndex = Random.Range(0, terrainChunks.Count);
            latestChunk = Instantiate(terrainChunks[randomIndex], spawnPosition, Quaternion.identity);
            spawnedChunks.Add(latestChunk);
        }

        void ChunkOptimization()
        {
            optimizationCooldown -= Time.deltaTime;
            if (optimizationCooldown > 0f) return;

            optimizationCooldown = optimizationCooldownDuration;

            foreach (GameObject chunk in spawnedChunks)
            {
                opDistance = Vector2.Distance(player.transform.position, chunk.transform.position);
                chunk.SetActive(opDistance <= maxOptimizationDistance);
            }
        }
}

