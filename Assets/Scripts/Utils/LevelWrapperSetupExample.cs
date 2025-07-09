using UnityEngine;

/// <summary>
/// Example configuration script showing how to set up the LevelWrapper and PlayerPlaceholder system
/// This is a helper script that can be used in the Unity Editor to set up the system correctly
/// </summary>
public class LevelWrapperSetupExample : MonoBehaviour
{
    [Header("Setup Configuration")]
    public GameObject playerPrefab;
    public GameObject[] initialWeapons;
    public GameObject[] initialPassiveItems;
    
    [Header("Scene References")]
    public LevelWrapper levelWrapper;
    public PlayerPlaceholder playerPlaceholder;
    
    [Header("Debug")]
    public bool setupOnStart = false;
    public bool enableDebugMode = true;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupLevelSystem();
        }
    }
    
    /// <summary>
    /// Set up the level system with the configured components
    /// </summary>
    [ContextMenu("Setup Level System")]
    public void SetupLevelSystem()
    {
        Debug.Log("Setting up LevelWrapper and PlayerPlaceholder system...");
        
        // Create LevelWrapper if it doesn't exist
        if (levelWrapper == null)
        {
            GameObject levelWrapperObj = new GameObject("LevelWrapper");
            levelWrapper = levelWrapperObj.AddComponent<LevelWrapper>();
        }
        
        // Create PlayerPlaceholder if it doesn't exist
        if (playerPlaceholder == null)
        {
            GameObject placeholderObj = new GameObject("PlayerPlaceholder");
            playerPlaceholder = placeholderObj.AddComponent<PlayerPlaceholder>();
        }
        
        // Configure LevelWrapper
        if (playerPrefab != null)
        {
            var playerPrefabField = typeof(LevelWrapper).GetField("playerPrefab");
            if (playerPrefabField != null)
            {
                playerPrefabField.SetValue(levelWrapper, playerPrefab);
                Debug.Log($"Assigned player prefab: {playerPrefab.name}");
            }
        }
        
        // Enable debug mode if requested
        if (enableDebugMode)
        {
            var debugField = typeof(PlayerPlaceholder).GetField("debugMode", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (debugField != null)
            {
                debugField.SetValue(playerPlaceholder, true);
                Debug.Log("Debug mode enabled for PlayerPlaceholder");
            }
        }
        
        // Add initial weapons to placeholder
        if (initialWeapons != null)
        {
            foreach (var weapon in initialWeapons)
            {
                if (weapon != null)
                {
                    GameObject weaponInstance = Instantiate(weapon, playerPlaceholder.transform);
                    Debug.Log($"Added weapon to placeholder: {weapon.name}");
                }
            }
        }
        
        // Add initial passive items to placeholder
        if (initialPassiveItems != null)
        {
            foreach (var passiveItem in initialPassiveItems)
            {
                if (passiveItem != null)
                {
                    GameObject passiveItemInstance = Instantiate(passiveItem, playerPlaceholder.transform);
                    Debug.Log($"Added passive item to placeholder: {passiveItem.name}");
                }
            }
        }
        
        Debug.Log("LevelWrapper system setup complete!");
        LogSystemStatus();
    }
    
    /// <summary>
    /// Log the current status of the system
    /// </summary>
    [ContextMenu("Log System Status")]
    public void LogSystemStatus()
    {
        Debug.Log("=== LevelWrapper System Status ===");
        
        if (levelWrapper != null)
        {
            Debug.Log($"LevelWrapper: Found on {levelWrapper.gameObject.name}");
            
            if (LevelWrapper.instance != null)
            {
                var controllers = LevelWrapper.instance.GetTransferredControllers();
                Debug.Log($"Tracked controllers: {controllers.Count}");
                
                foreach (var controller in controllers)
                {
                    if (controller != null)
                    {
                        Debug.Log($"  - {controller.name}");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("LevelWrapper not found!");
        }
        
        if (playerPlaceholder != null)
        {
            Debug.Log($"PlayerPlaceholder: Found on {playerPlaceholder.gameObject.name}");
            Debug.Log($"Controllers in placeholder: {playerPlaceholder.GetControllerCount()}");
            
            var controllers = playerPlaceholder.GetControllers();
            foreach (var controller in controllers)
            {
                if (controller != null)
                {
                    string controllerType = "Unknown";
                    if (controller.GetComponent<WeaponController>() != null)
                        controllerType = "WeaponController";
                    else if (controller.GetComponent<PassiveItem>() != null)
                        controllerType = "PassiveItem";
                    
                    Debug.Log($"  - {controller.name} ({controllerType})");
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerPlaceholder not found!");
        }
        
        Debug.Log("=== End Status ===");
    }
    
    /// <summary>
    /// Clean up the system (useful for testing)
    /// </summary>
    [ContextMenu("Clean Up System")]
    public void CleanUpSystem()
    {
        Debug.Log("Cleaning up LevelWrapper system...");
        
        if (LevelWrapper.instance != null)
        {
            LevelWrapper.instance.ExitLevel();
        }
        
        if (playerPlaceholder != null)
        {
            playerPlaceholder.ClearControllers();
        }
        
        Debug.Log("System cleaned up!");
    }
    
    /// <summary>
    /// Validate the system setup
    /// </summary>
    [ContextMenu("Validate System")]
    public void ValidateSystem()
    {
        Debug.Log("Validating LevelWrapper system...");
        
        bool isValid = true;
        
        // Check LevelWrapper
        if (levelWrapper == null)
        {
            Debug.LogError("LevelWrapper is missing!");
            isValid = false;
        }
        
        // Check PlayerPlaceholder
        if (playerPlaceholder == null)
        {
            Debug.LogError("PlayerPlaceholder is missing!");
            isValid = false;
        }
        
        // Check Player prefab
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned!");
            isValid = false;
        }
        
        // Validate controllers in placeholder
        if (playerPlaceholder != null)
        {
            if (!playerPlaceholder.ValidateControllers())
            {
                Debug.LogError("Invalid controllers found in PlayerPlaceholder!");
                isValid = false;
            }
        }
        
        if (isValid)
        {
            Debug.Log("System validation passed!");
        }
        else
        {
            Debug.LogError("System validation failed!");
        }
    }
    
    private void OnGUI()
    {
        if (levelWrapper == null || playerPlaceholder == null)
        {
            GUI.Label(new Rect(10, 10, 300, 20), "LevelWrapper Setup Example");
            GUI.Label(new Rect(10, 30, 300, 20), "Configure fields and run SetupLevelSystem()");
        }
        else
        {
            GUI.Label(new Rect(10, 10, 300, 20), "LevelWrapper System: Active");
            
            if (playerPlaceholder != null)
            {
                GUI.Label(new Rect(10, 30, 300, 20), $"Placeholder controllers: {playerPlaceholder.GetControllerCount()}");
            }
            
            if (LevelWrapper.instance != null)
            {
                var controllers = LevelWrapper.instance.GetTransferredControllers();
                GUI.Label(new Rect(10, 50, 300, 20), $"Tracked controllers: {controllers.Count}");
            }
        }
    }
}