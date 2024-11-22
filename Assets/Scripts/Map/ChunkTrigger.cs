using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{

    MapController mapController;

    public GameObject targetMap;

    private void Start()
    {
        mapController = FindObjectOfType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            mapController.currentChunk = targetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if(mapController.currentChunk == targetMap)
            {
                mapController.currentChunk = null;
            }
        }
    }

}
