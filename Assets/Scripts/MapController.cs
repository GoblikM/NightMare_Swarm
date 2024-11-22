using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    private Vector3 noTerrainPosition;
    public LayerMask terrainLayer;
    public GameObject currentChunk;
    private PlayerMovement playerMovement;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    public GameObject latestChunk;
    public float maxOptimizationDistance;
    private float opDistance;
    private float optimizationCooldown;
    public float optimizationCooldownDuration;

    // Directional markers
    private Dictionary<string, string> directions = new Dictionary<string, string>()
    {
        { "Right", "Right" },
        { "Left", "Left" },
        { "Up", "Up" },
        { "Down", "Down" },
        { "UpRight", "UpRight" },
        { "DownRight", "DownRight" },
        { "UpLeft", "UpLeft" },
        { "DownLeft", "DownLeft" }
    };

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        ChunkChecker();
        ChunkOptimization();
    }

    void ChunkChecker()
    {
        if (currentChunk == null)
        {
            return;
        }

        // Get player direction
        Vector2 playerMoveDir = playerMovement.GetPlayerMoveDir();

        foreach (var dir in directions)
        {
            // If player is moving in that direction, check for terrain
            if (IsMovingInDirection(playerMoveDir, dir.Key))
            {
                CheckForTerrain(dir.Key);
                break; // We only need to check one direction at a time
            }
        }
    }

    bool IsMovingInDirection(Vector2 moveDir, string direction)
    {
        switch (direction)
        {
            case "Right":
                return moveDir.x > 0 && moveDir.y == 0;
            case "Left":
                return moveDir.x < 0 && moveDir.y == 0;
            case "Up":
                return moveDir.y > 0 && moveDir.x == 0;
            case "Down":
                return moveDir.y < 0 && moveDir.x == 0;
            case "UpRight":
                return moveDir.x > 0 && moveDir.y > 0;
            case "DownRight":
                return moveDir.x > 0 && moveDir.y < 0;
            case "UpLeft":
                return moveDir.x < 0 && moveDir.y > 0;
            case "DownLeft":
                return moveDir.x < 0 && moveDir.y < 0;
            default:
                return false;
        }
    }

    void CheckForTerrain(string direction)
    {
        // Find the position of the corresponding direction and check for terrain
        Transform directionTransform = currentChunk.transform.Find(direction);
        if (directionTransform != null)
        {
            if (!Physics2D.OverlapCircle(directionTransform.position, checkerRadius, terrainLayer))
            {
                noTerrainPosition = directionTransform.position;
                SpawnChunk();
            }
        }
        else
        {
            Debug.LogWarning("Direction not found: " + direction);
        }
    }

    void SpawnChunk()
    {
        int randomIndex = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[randomIndex], noTerrainPosition, Quaternion.identity);
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
