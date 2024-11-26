using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object has the ICollectible interface
        if (collision.gameObject.TryGetComponent(out ICollectible collectable))
        {
            // Call the Collect method
            collectable.Collect();
        }
    }
    

}
