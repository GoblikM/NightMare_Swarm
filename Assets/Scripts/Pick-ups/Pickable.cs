using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * * This class is the parent class of all pickable objects in the game.
 * * */
public class Pickable : MonoBehaviour, ICollectible
{
    public bool hasBeenCollected = false;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }

    /**
* When the player collides with the pickable object, the object will be destroyed.
* */
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
