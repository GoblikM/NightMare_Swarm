using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // Speed of bobbing
    public float magnitude; // Range of bobbing
    public Vector3 direction; // Direction of bobbing
    Vector3 initialPosition;
    Pickable pickable;

    private void Start()
    {
        pickable = GetComponent<Pickable>();
        // save the initial position of the object
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(pickable && !pickable.hasBeenCollected)
        {
            // calculate the new position of the object
            Vector3 newPosition = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
            transform.position = newPosition;
        }
    }
}
