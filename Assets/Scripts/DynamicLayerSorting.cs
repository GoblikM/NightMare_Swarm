using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLayerSorting : MonoBehaviour
{
    // reference to the player
    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Find player according to the tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player != null)
        {
            sortLayers();
        }


    }

    private void sortLayers()
    {
        if (player.position.y > transform.position.y)// if player is above the object 
        {
            spriteRenderer.sortingOrder = 30;
        }
        else
        {
            spriteRenderer.sortingOrder = 1;
        }

    }
}
