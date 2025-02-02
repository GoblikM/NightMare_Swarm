using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleFlicker : MonoBehaviour
{
    private Light2D candleLight;

    [Header("Flicker Settings")]
    [SerializeField]
    private float baseIntensity = 2.5f;
    [SerializeField]
    private float flickerStrength = 2.5f;
    [SerializeField]
    private float flickerSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        candleLight = GetComponent<Light2D>();
        candleLight.intensity = baseIntensity;
        
    }

    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        candleLight.intensity = baseIntensity + noise * flickerStrength;
    }
}
