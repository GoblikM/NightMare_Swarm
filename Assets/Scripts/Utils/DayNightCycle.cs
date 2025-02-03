using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;

    [SerializeField] private float nightDuration = 600f;

    [Header("Light Intensity")]
    [SerializeField] private float nightIntensity = 0.1f;
    [SerializeField] private float dayIntensity = 1f;
  
    [Header("Light Color")]
    // this is the night color - 3165FF
    [SerializeField] private Color nightColor;

    // this is the day color - FFFFFF
    [SerializeField] private Color dayColor;

    private float startTime;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        globalLight.intensity = nightIntensity;
        globalLight.color = nightColor;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;

        float t = Mathf.Clamp01(elapsedTime / nightDuration);

        float currentIntensity = Mathf.Lerp(nightIntensity, dayIntensity, t);
        globalLight.intensity = currentIntensity;

        Color currentColor = Color.Lerp(nightColor, dayColor, t);
        globalLight.color = currentColor;

    }
}
