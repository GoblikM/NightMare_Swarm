using UnityEngine;
using System.Collections;

/// <summary>
/// Integration test for the LevelWrapper system
/// This script tests the complete flow of the system
/// </summary>
public class LevelWrapperIntegrationTest : MonoBehaviour
{
    [Header("Test Results")]
    public bool testPassed = false;
    public string testResults = "";
    
    [Header("Test Configuration")]
    public GameObject playerPrefab;
    public GameObject testWeaponPrefab;
    public GameObject testPassiveItemPrefab;
    
    private int testStep = 0;
    private bool testRunning = false;
    
    private void Start()
    {
        // Run test automatically if in test mode
        if (Debug.isDebugBuild)
        {
            StartCoroutine(RunIntegrationTest());
        }
    }
    
    /// <summary>
    /// Run the complete integration test
    /// </summary>
    public IEnumerator RunIntegrationTest()
    {
        if (testRunning)
        {
            Debug.LogWarning("Test already running!");
            yield break;
        }
        
        testRunning = true;
        testPassed = false;
        testResults = "";
        
        Debug.Log("Starting LevelWrapper Integration Test...");
        
        try
        {
            // Step 1: Setup test environment
            yield return StartCoroutine(TestStep1_Setup());
            
            // Step 2: Test placeholder initialization
            yield return StartCoroutine(TestStep2_PlaceholderInit());
            
            // Step 3: Test player replacement
            yield return StartCoroutine(TestStep3_PlayerReplacement());
            
            // Step 4: Test controller tracking
            yield return StartCoroutine(TestStep4_ControllerTracking());
            
            // Step 5: Test runtime controller spawning
            yield return StartCoroutine(TestStep5_RuntimeSpawning());
            
            // Step 6: Test level exit cleanup
            yield return StartCoroutine(TestStep6_LevelExit());
            
            // Test completed successfully
            testPassed = true;
            testResults = "All tests passed!";
            Debug.Log("Integration test completed successfully!");
        }
        catch (System.Exception e)
        {
            testPassed = false;
            testResults = $"Test failed at step {testStep}: {e.Message}";
            Debug.LogError($"Integration test failed: {e.Message}");
        }
        finally
        {
            testRunning = false;
        }
    }
    
    private IEnumerator TestStep1_Setup()
    {
        testStep = 1;
        Debug.Log("Step 1: Setting up test environment...");
        
        // Clean up any existing instances
        if (LevelWrapper.instance != null)
        {
            DestroyImmediate(LevelWrapper.instance.gameObject);
        }
        
        PlayerPlaceholder[] placeholders = FindObjectsOfType<PlayerPlaceholder>();
        foreach (var placeholder in placeholders)
        {
            DestroyImmediate(placeholder.gameObject);
        }
        
        yield return null;
        
        // Create test objects
        GameObject levelWrapperObj = new GameObject("TestLevelWrapper");
        LevelWrapper levelWrapper = levelWrapperObj.AddComponent<LevelWrapper>();
        
        GameObject placeholderObj = new GameObject("TestPlayerPlaceholder");
        PlayerPlaceholder placeholder = placeholderObj.AddComponent<PlayerPlaceholder>();
        
        // Configure LevelWrapper
        if (playerPrefab != null)
        {
            var playerPrefabField = typeof(LevelWrapper).GetField("playerPrefab");
            if (playerPrefabField != null)
            {
                playerPrefabField.SetValue(levelWrapper, playerPrefab);
            }
        }
        
        Debug.Log("Step 1: Complete");
        yield return null;
    }
    
    private IEnumerator TestStep2_PlaceholderInit()
    {
        testStep = 2;
        Debug.Log("Step 2: Testing placeholder initialization...");
        
        PlayerPlaceholder placeholder = FindObjectOfType<PlayerPlaceholder>();
        if (placeholder == null)
        {
            throw new System.Exception("PlayerPlaceholder not found");
        }
        
        // Add test controllers to placeholder
        if (testWeaponPrefab != null)
        {
            GameObject weapon = Instantiate(testWeaponPrefab, placeholder.transform);
            weapon.name = "TestWeapon";
        }
        
        if (testPassiveItemPrefab != null)
        {
            GameObject passiveItem = Instantiate(testPassiveItemPrefab, placeholder.transform);
            passiveItem.name = "TestPassiveItem";
        }
        
        yield return null;
        
        // Validate placeholder contents
        if (placeholder.GetControllerCount() == 0)
        {
            throw new System.Exception("No controllers added to placeholder");
        }
        
        Debug.Log($"Step 2: Complete - {placeholder.GetControllerCount()} controllers in placeholder");
        yield return null;
    }
    
    private IEnumerator TestStep3_PlayerReplacement()
    {
        testStep = 3;
        Debug.Log("Step 3: Testing player replacement...");
        
        LevelWrapper levelWrapper = LevelWrapper.instance;
        if (levelWrapper == null)
        {
            throw new System.Exception("LevelWrapper instance not found");
        }
        
        // Get initial controller count
        PlayerPlaceholder placeholder = FindObjectOfType<PlayerPlaceholder>();
        int initialControllerCount = placeholder != null ? placeholder.GetControllerCount() : 0;
        
        // Initialize level (this should replace the placeholder)
        levelWrapper.InitializeLevel();
        
        yield return null;
        
        // Verify player was created
        GameObject player = levelWrapper.GetCurrentPlayer();
        if (player == null)
        {
            throw new System.Exception("Player was not created after replacement");
        }
        
        // Verify controllers were transferred
        var transferredControllers = levelWrapper.GetTransferredControllers();
        if (transferredControllers.Count != initialControllerCount)
        {
            throw new System.Exception($"Controller count mismatch: expected {initialControllerCount}, got {transferredControllers.Count}");
        }
        
        Debug.Log($"Step 3: Complete - Player created with {transferredControllers.Count} controllers");
        yield return null;
    }
    
    private IEnumerator TestStep4_ControllerTracking()
    {
        testStep = 4;
        Debug.Log("Step 4: Testing controller tracking...");
        
        LevelWrapper levelWrapper = LevelWrapper.instance;
        var initialControllers = levelWrapper.GetTransferredControllers();
        int initialCount = initialControllers.Count;
        
        // Test adding a controller
        GameObject testController = new GameObject("TestController");
        testController.AddComponent<WeaponController>();
        
        levelWrapper.AddTransferredController(testController);
        
        yield return null;
        
        var updatedControllers = levelWrapper.GetTransferredControllers();
        if (updatedControllers.Count != initialCount + 1)
        {
            throw new System.Exception("Controller was not added to tracking");
        }
        
        // Test removing a controller
        levelWrapper.RemoveTransferredController(testController);
        
        var finalControllers = levelWrapper.GetTransferredControllers();
        if (finalControllers.Count != initialCount)
        {
            throw new System.Exception("Controller was not removed from tracking");
        }
        
        // Clean up test controller
        DestroyImmediate(testController);
        
        Debug.Log("Step 4: Complete - Controller tracking working correctly");
        yield return null;
    }
    
    private IEnumerator TestStep5_RuntimeSpawning()
    {
        testStep = 5;
        Debug.Log("Step 5: Testing runtime controller spawning...");
        
        // Find PlayerStats component
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats not found, skipping runtime spawning test");
            yield return null;
            return;
        }
        
        var initialControllers = LevelWrapper.instance.GetTransferredControllers();
        int initialCount = initialControllers.Count;
        
        // Test spawning a weapon
        if (testWeaponPrefab != null)
        {
            playerStats.SpawnWeapon(testWeaponPrefab);
            yield return null;
            
            var updatedControllers = LevelWrapper.instance.GetTransferredControllers();
            if (updatedControllers.Count <= initialCount)
            {
                throw new System.Exception("Runtime spawned weapon was not tracked");
            }
        }
        
        Debug.Log("Step 5: Complete - Runtime spawning working correctly");
        yield return null;
    }
    
    private IEnumerator TestStep6_LevelExit()
    {
        testStep = 6;
        Debug.Log("Step 6: Testing level exit cleanup...");
        
        LevelWrapper levelWrapper = LevelWrapper.instance;
        var controllersBeforeExit = levelWrapper.GetTransferredControllers();
        int controllerCountBeforeExit = controllersBeforeExit.Count;
        
        if (controllerCountBeforeExit == 0)
        {
            throw new System.Exception("No controllers to clean up");
        }
        
        // Exit level
        levelWrapper.ExitLevel();
        
        yield return null;
        
        // Verify controllers were cleaned up
        var controllersAfterExit = levelWrapper.GetTransferredControllers();
        if (controllersAfterExit.Count != 0)
        {
            throw new System.Exception($"Controllers not cleaned up: {controllersAfterExit.Count} remaining");
        }
        
        // Verify actual GameObjects were destroyed
        foreach (var controller in controllersBeforeExit)
        {
            if (controller != null)
            {
                throw new System.Exception("Controller GameObject was not destroyed");
            }
        }
        
        Debug.Log("Step 6: Complete - Level exit cleanup working correctly");
        yield return null;
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "LevelWrapper Integration Test");
        
        if (testRunning)
        {
            GUI.Label(new Rect(10, 30, 300, 20), $"Running test step {testStep}...");
        }
        else
        {
            GUI.Label(new Rect(10, 30, 300, 20), $"Test Status: {(testPassed ? "PASSED" : "NOT RUN")}");
            
            if (!string.IsNullOrEmpty(testResults))
            {
                GUI.Label(new Rect(10, 50, 300, 40), testResults);
            }
            
            if (GUI.Button(new Rect(10, 90, 100, 30), "Run Test"))
            {
                StartCoroutine(RunIntegrationTest());
            }
        }
    }
}