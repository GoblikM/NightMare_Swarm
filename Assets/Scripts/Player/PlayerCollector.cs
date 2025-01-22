using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{

    PlayerStats player;
    CircleCollider2D circleCollider;
    public float pullSpeed;



    private void Start()
    {
        // Get the player stats component from the player object
        player = FindObjectOfType<PlayerStats>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // Set the radius of the circle collider to the current pick up range of the player
        circleCollider.radius = player.CurrentPickUpRange;
    }

    /**
     * This method is called when the player enters the trigger collider of the object
     **/   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object has the ICollectible interface
        if (collision.gameObject.TryGetComponent(out ICollectible collectable))
        {
            // Get the rigidbody of the collectible object
            Rigidbody2D collectibleRb = collision.gameObject.GetComponent<Rigidbody2D>();
            // Calculate the force direction towards the player (vector from the collectible to the player)
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            // Apply the force to the collectible object to pull it towards the player
            collectibleRb.AddForce(forceDirection * pullSpeed);
            // Call the Collect method
            collectable.Collect();
        }
    }
    

}
