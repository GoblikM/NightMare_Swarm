using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // Speed of bobbing
    public float magnitude; // Range of bobbing
    public Vector3 direction; // Direction of bobbing
    Vector3 initialPosition;

    private void Start()
    {
        // save the initial position of the object
        initialPosition = transform.position;
    }

    private void Update()
    {
        // calculate the new position of the object
        Vector3 newPosition = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
        transform.position = newPosition;
    }
}
