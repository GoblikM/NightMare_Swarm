using UnityEngine;

/// <summary>
/// PlayerPlaceholder acts as a temporary holder for controllers (weapons, passive items) 
/// before they are transferred to the actual Player during level initialization
/// </summary>
public class PlayerPlaceholder : MonoBehaviour
{
    [Header("Placeholder Configuration")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private Color gizmoColor = Color.yellow;
    
    /// <summary>
    /// Initialize the placeholder with default controllers if needed
    /// </summary>
    private void Start()
    {
        if (debugMode)
        {
            Debug.Log($"PlayerPlaceholder initialized with {transform.childCount} controllers");
        }
    }
    
    /// <summary>
    /// Add a controller to the placeholder
    /// </summary>
    /// <param name="controller">The controller GameObject to add</param>
    public void AddController(GameObject controller)
    {
        if (controller == null)
        {
            Debug.LogWarning("Cannot add null controller to PlayerPlaceholder");
            return;
        }
        
        controller.transform.SetParent(transform);
        controller.transform.localPosition = Vector3.zero;
        
        if (debugMode)
        {
            Debug.Log($"Added controller to placeholder: {controller.name}");
        }
    }
    
    /// <summary>
    /// Remove a controller from the placeholder
    /// </summary>
    /// <param name="controller">The controller GameObject to remove</param>
    public void RemoveController(GameObject controller)
    {
        if (controller == null)
        {
            Debug.LogWarning("Cannot remove null controller from PlayerPlaceholder");
            return;
        }
        
        if (controller.transform.parent == transform)
        {
            controller.transform.SetParent(null);
            
            if (debugMode)
            {
                Debug.Log($"Removed controller from placeholder: {controller.name}");
            }
        }
    }
    
    /// <summary>
    /// Get all controllers currently in the placeholder
    /// </summary>
    /// <returns>Array of controller GameObjects</returns>
    public GameObject[] GetControllers()
    {
        GameObject[] controllers = new GameObject[transform.childCount];
        
        for (int i = 0; i < transform.childCount; i++)
        {
            controllers[i] = transform.GetChild(i).gameObject;
        }
        
        return controllers;
    }
    
    /// <summary>
    /// Get the number of controllers in the placeholder
    /// </summary>
    /// <returns>Number of controllers</returns>
    public int GetControllerCount()
    {
        return transform.childCount;
    }
    
    /// <summary>
    /// Clear all controllers from the placeholder
    /// </summary>
    public void ClearControllers()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            
            if (debugMode)
            {
                Debug.Log($"Clearing controller from placeholder: {child.name}");
            }
            
            Destroy(child);
        }
    }
    
    /// <summary>
    /// Check if the placeholder has any controllers
    /// </summary>
    /// <returns>True if placeholder has controllers</returns>
    public bool HasControllers()
    {
        return transform.childCount > 0;
    }
    
    /// <summary>
    /// Validate that all children are valid controllers
    /// </summary>
    /// <returns>True if all children are valid controllers</returns>
    public bool ValidateControllers()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            
            // Check if child has a valid controller component
            if (child.GetComponent<WeaponController>() == null && 
                child.GetComponent<PassiveItem>() == null)
            {
                Debug.LogWarning($"Child {child.name} is not a valid controller");
                return false;
            }
        }
        
        return true;
    }
    
#if UNITY_EDITOR
    /// <summary>
    /// Draw gizmos to visualize the placeholder in the scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
            // Draw connections to child controllers
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Gizmos.DrawLine(transform.position, child.position);
            }
        }
    }
    
    /// <summary>
    /// Draw GUI information in the scene view
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (debugMode)
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.7f, 
                $"PlayerPlaceholder\nControllers: {transform.childCount}");
        }
    }
#endif
}