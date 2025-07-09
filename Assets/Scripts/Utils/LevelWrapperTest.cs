using UnityEngine;
using System.Collections;

/// <summary>
/// Test script for the LevelWrapper and PlayerPlaceholder system
/// This script can be attached to a GameObject in the scene to test the functionality
/// </summary>
public class LevelWrapperTest : MonoBehaviour
{
    [Header("Test Configuration")]
    public GameObject testWeaponPrefab;
    public GameObject testPassiveItemPrefab;
    public KeyCode testSpawnKey = KeyCode.T;
    public KeyCode testExitKey = KeyCode.E;
    public KeyCode testInfoKey = KeyCode.I;
    
    private void Update()
    {
        if (Input.GetKeyDown(testSpawnKey))
        {
            TestSpawnControllers();
        }
        
        if (Input.GetKeyDown(testExitKey))
        {
            TestExitLevel();
        }
        
        if (Input.GetKeyDown(testInfoKey))
        {
            TestShowInfo();
        }
    }
    
    private void TestSpawnControllers()
    {
        Debug.Log("Testing controller spawning...");
        
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("No PlayerStats found in scene");
            return;
        }
        
        // Test spawning a weapon
        if (testWeaponPrefab != null)
        {
            playerStats.SpawnWeapon(testWeaponPrefab);
            Debug.Log("Spawned test weapon");
        }
        
        // Test spawning a passive item
        if (testPassiveItemPrefab != null)
        {
            playerStats.SpawnPassiveItem(testPassiveItemPrefab);
            Debug.Log("Spawned test passive item");
        }
        
        TestShowInfo();
    }
    
    private void TestExitLevel()
    {
        Debug.Log("Testing level exit...");
        
        if (LevelWrapper.instance != null)
        {
            LevelWrapper.instance.ExitLevel();
            Debug.Log("Level exit completed");
        }
        else
        {
            Debug.LogWarning("No LevelWrapper instance found");
        }
        
        TestShowInfo();
    }
    
    private void TestShowInfo()
    {
        Debug.Log("=== LevelWrapper Test Info ===");
        
        if (LevelWrapper.instance != null)
        {
            var controllers = LevelWrapper.instance.GetTransferredControllers();
            Debug.Log($"Tracked controllers: {controllers.Count}");
            
            foreach (var controller in controllers)
            {
                if (controller != null)
                {
                    Debug.Log($"  - {controller.name} ({controller.GetType().Name})");
                }
            }
            
            GameObject player = LevelWrapper.instance.GetCurrentPlayer();
            if (player != null)
            {
                Debug.Log($"Current player: {player.name} with {player.transform.childCount} children");
            }
        }
        else
        {
            Debug.LogWarning("No LevelWrapper instance found");
        }
        
        // Check for PlayerPlaceholder
        PlayerPlaceholder placeholder = FindObjectOfType<PlayerPlaceholder>();
        if (placeholder != null)
        {
            Debug.Log($"PlayerPlaceholder found with {placeholder.GetControllerCount()} controllers");
        }
        else
        {
            Debug.Log("No PlayerPlaceholder found in scene");
        }
        
        Debug.Log("=== End Info ===");
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "LevelWrapper Test Controls:");
        GUI.Label(new Rect(10, 30, 300, 20), $"Press {testSpawnKey} to spawn test controllers");
        GUI.Label(new Rect(10, 50, 300, 20), $"Press {testExitKey} to exit level");
        GUI.Label(new Rect(10, 70, 300, 20), $"Press {testInfoKey} to show info");
        
        if (LevelWrapper.instance != null)
        {
            var controllers = LevelWrapper.instance.GetTransferredControllers();
            GUI.Label(new Rect(10, 100, 300, 20), $"Tracked controllers: {controllers.Count}");
        }
    }
}