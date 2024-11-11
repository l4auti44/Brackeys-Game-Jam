using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private float fadeSpeed;   // Speed at which the light fades
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private bool isFading = false; // Flag to control the fading process
    private GameManager gameManager;

    void Start()
    {
        // Automatically find the Light2D component in the child object
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find the Game Manager by name
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Get the light fade speed from the game manager
        fadeSpeed = gameManager.asteroidFadeLightSpeed;

        isFading = true;
    }

    void Update()
    {
        // If fading is activated, reduce the light intensity over time
        if (isFading)
        {
            Color color = spriteRenderer.color; // Get the current color
            color.a -= fadeSpeed * 0.01f; // Set the alpha to 1
            spriteRenderer.color = color; // Apply the updated color back to the SpriteRenderer
        }
    }

    // Method to reset the light intensity to the maximum
    public void IncreaseLightToMax()
    {
        Color color = spriteRenderer.color; // Get the current color
        color.a = 1f; // Set the alpha to 1
        spriteRenderer.color = color; // Apply the updated color back to the SpriteRenderer

    }
}
