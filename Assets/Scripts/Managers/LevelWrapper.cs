using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LevelWrapper : MonoBehaviour
{
    [Header("Level Management")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    
    [Header("Controller Tracking")]
    private List<GameObject> transferredControllers = new List<GameObject>();
    private PlayerPlaceholder currentPlaceholder;
    private GameObject currentPlayer;
    
    public static LevelWrapper instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeLevel();
    }
    
    /// <summary>
    /// Initialize the level by setting up the player placeholder
    /// </summary>
    public void InitializeLevel()
    {
        // Find or create the player placeholder
        currentPlaceholder = FindObjectOfType<PlayerPlaceholder>();
        if (currentPlaceholder == null)
        {
            Debug.LogWarning("No PlayerPlaceholder found in the scene");
            return;
        }
        
        // Store references to all controllers in the placeholder
        StoreControllerReferences();
        
        // Replace placeholder with actual player
        ReplacePlayerPlaceholder();
    }
    
    /// <summary>
    /// Store references to all controllers (weapons, passive items) in the placeholder
    /// </summary>
    private void StoreControllerReferences()
    {
        transferredControllers.Clear();
        
        if (currentPlaceholder != null)
        {
            // Get all child controllers from the placeholder
            for (int i = 0; i < currentPlaceholder.transform.childCount; i++)
            {
                GameObject child = currentPlaceholder.transform.GetChild(i).gameObject;
                
                // Check if the child is a controller (WeaponController or PassiveItem)
                if (child.GetComponent<WeaponController>() != null || 
                    child.GetComponent<PassiveItem>() != null)
                {
                    transferredControllers.Add(child);
                }
            }
            
            Debug.Log($"Stored {transferredControllers.Count} controllers from placeholder");
        }
    }
    
    /// <summary>
    /// Replace the PlayerPlaceholder with the actual Player
    /// </summary>
    private void ReplacePlayerPlaceholder()
    {
        if (currentPlaceholder == null || playerPrefab == null)
        {
            Debug.LogError("Cannot replace player placeholder - missing placeholder or player prefab");
            return;
        }
        
        // Get the position and rotation of the placeholder
        Vector3 placeholderPosition = currentPlaceholder.transform.position;
        Quaternion placeholderRotation = currentPlaceholder.transform.rotation;
        
        // Instantiate the actual player
        currentPlayer = Instantiate(playerPrefab, placeholderPosition, placeholderRotation);
        
        // Transfer all controllers from placeholder to player
        TransferControllers();
        
        // Update GameManager reference if needed
        if (GameManager.instance != null)
        {
            GameManager.instance.playerObject = currentPlayer;
        }
        
        // Destroy the placeholder
        Destroy(currentPlaceholder.gameObject);
        
        Debug.Log("Player placeholder replaced successfully");
    }
    
    /// <summary>
    /// Transfer all controllers from placeholder to the actual player
    /// </summary>
    private void TransferControllers()
    {
        if (currentPlayer == null) return;
        
        foreach (GameObject controller in transferredControllers)
        {
            if (controller != null)
            {
                controller.transform.SetParent(currentPlayer.transform);
                controller.transform.localPosition = Vector3.zero;
                
                // Update controller references if needed
                UpdateControllerReferences(controller);
            }
        }
        
        Debug.Log($"Transferred {transferredControllers.Count} controllers to player");
    }
    
    /// <summary>
    /// Update controller references to the new player
    /// </summary>
    private void UpdateControllerReferences(GameObject controller)
    {
        // Update WeaponController references
        WeaponController weaponController = controller.GetComponent<WeaponController>();
        if (weaponController != null)
        {
            // Use reflection to update the protected playerMovement field
            FieldInfo playerMovementField = typeof(WeaponController).GetField("playerMovement", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (playerMovementField != null)
            {
                playerMovementField.SetValue(weaponController, currentPlayer.GetComponent<Player>());
            }
        }
        
        // Update PassiveItem references
        PassiveItem passiveItem = controller.GetComponent<PassiveItem>();
        if (passiveItem != null)
        {
            // Use reflection to update the protected player field
            FieldInfo playerField = typeof(PassiveItem).GetField("player", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (playerField != null)
            {
                playerField.SetValue(passiveItem, currentPlayer.GetComponent<PlayerStats>());
            }
        }
    }
    
    /// <summary>
    /// Clean up controllers when exiting the level
    /// </summary>
    public void ExitLevel()
    {
        RemoveTransferredControllers();
        transferredControllers.Clear();
        
        Debug.Log("Level exited and controllers cleaned up");
    }
    
    /// <summary>
    /// Remove all controllers that were transferred from the placeholder
    /// </summary>
    private void RemoveTransferredControllers()
    {
        if (currentPlayer == null) return;
        
        foreach (GameObject controller in transferredControllers)
        {
            if (controller != null)
            {
                Debug.Log($"Removing controller: {controller.name}");
                Destroy(controller);
            }
        }
        
        Debug.Log($"Removed {transferredControllers.Count} transferred controllers");
    }
    
    /// <summary>
    /// Get the current player object
    /// </summary>
    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }
    
    /// <summary>
    /// Get the list of transferred controllers
    /// </summary>
    public List<GameObject> GetTransferredControllers()
    {
        return new List<GameObject>(transferredControllers);
    }
    
    /// <summary>
    /// Add a controller to the tracking list (for runtime-spawned controllers)
    /// </summary>
    public void AddTransferredController(GameObject controller)
    {
        if (controller != null && !transferredControllers.Contains(controller))
        {
            transferredControllers.Add(controller);
            Debug.Log($"Added controller to tracking: {controller.name}");
        }
    }
    
    /// <summary>
    /// Remove a controller from the tracking list
    /// </summary>
    public void RemoveTransferredController(GameObject controller)
    {
        if (controller != null && transferredControllers.Contains(controller))
        {
            transferredControllers.Remove(controller);
            Debug.Log($"Removed controller from tracking: {controller.name}");
        }
    }
    
    private void OnDestroy()
    {
        // Clean up when the level wrapper is destroyed
        ExitLevel();
    }
}