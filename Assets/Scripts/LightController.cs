using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private float fadeSpeed = 0.5f;   // Speed at which the light fades
    public float maxLightIntensity = 1f; // The maximum intensity of the light

    private Light2D childLight;    // Reference to the Light2D component
    private bool isFading = false; // Flag to control the fading process

    void Start()
    {
        // Automatically find the Light2D component in the child object
        childLight = GetComponentInChildren<Light2D>();
        if (childLight == null)
        {
            Debug.LogError("No Light2D component found in children.");
        }

        isFading = true;
    }

    void Update()
    {
        // If fading is activated, reduce the light intensity over time
        if (isFading && childLight != null)
        {
            childLight.intensity = Mathf.Max(0, childLight.intensity - fadeSpeed * Time.deltaTime);
        }
    }

    // Method to activate the light fading
    public void StartFading()
    {
        
    }

    // Method to reset the light intensity to the maximum
    public void IncreaseLightToMax()
    {
        if (childLight != null)
        {
            childLight.intensity = maxLightIntensity;
        }
    }
}
