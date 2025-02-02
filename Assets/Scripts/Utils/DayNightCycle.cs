using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;

    [SerializeField] private float dayDuration = 300f;

    [Header("Light Intensity")]
    [SerializeField] private float nightIntensity = 0.2f;
    [SerializeField] private float dayIntensity = 1f;
  
    [Header("Light Color")]
    // this is the night color - 3165FF
    [SerializeField] private Color nightColor = new Color(0.1921569f, 0.3960785f, 1f, 1f);

    // this is the day color - FFFFFF
    [SerializeField] private Color dayColor = new Color(1f, 1f, 1f, 1f);

    private float startTime;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        globalLight.intensity = nightIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;

        float t = Mathf.Clamp01(elapsedTime / dayDuration);

        float currentIntensity = Mathf.Lerp(nightIntensity, dayIntensity, t);
        globalLight.intensity = currentIntensity;

        Color currentColor = Color.Lerp(nightColor, dayColor, t);
        globalLight.color = currentColor;

    }
}
