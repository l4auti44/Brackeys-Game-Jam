using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormManager : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRenderer; // Assign the sprite renderer of the image here
    public GameObject gameProgressObject;  // Object holding the progress variable
    public AsteroidSpawner asteroidSpawnerScript;  // Reference to the asteroid spawner script

    [Header("Image Alpha Settings")]
    public float maxAlpha = 1f;  // Maximum alpha value for the image
    public float alphaSpeedIncrease; //Speed at which the alpha channel changes

    [Header("Asteroid Speed Settings")]
    public float minAsteroidSpeedLimit = 10f;  // Maximum limit for min asteroid speed
    public float maxAsteroidSpeedLimit = 20f;  // Maximum limit for max asteroid speed
    public float asteroidSpeedIncrease = 1f; //Speed at which the max and min asteroid speed are accelerating

    [Header("Asteroid Spawn Interval Settings")]
    public float minSpawnIntervalLimit = 0.5f;  // Minimum spawn interval limit
    public float maxSpawnIntervalLimit = 2f;    // Maximum spawn interval limit
    public float asteroidSpawnIntervalIncrease; //Speed at which the max and min asteroid spawning interval are accelerating

    private float gameProgress = 0f; // Variable to hold the game progress value

    void Update()
    {
        UpdateGameProgress();
        UpdateImageAlpha();
        UpdateAsteroidSpeed();
        UpdateAsteroidSpawnInterval();
    }

    void UpdateGameProgress()
    {
        // Assuming the progress value is a public variable in another script called "GameProgress"
        gameProgress = gameProgressObject.GetComponent<GameManager>().gameProgress * 0.01f;
    }

    void UpdateImageAlpha()
    {
        // Adjust alpha of the sprite renderer
        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(0, maxAlpha, gameProgress * alphaSpeedIncrease);
        spriteRenderer.color = color;
    }

    void UpdateAsteroidSpeed()
    {
        if (asteroidSpawnerScript != null)
        {
            // Increase asteroid speed range based on progress
            asteroidSpawnerScript.minAsteroidSpeed = Mathf.Lerp(asteroidSpawnerScript.minAsteroidSpeed, minAsteroidSpeedLimit, gameProgress * asteroidSpeedIncrease * 0.01f);
            asteroidSpawnerScript.maxAsteroidSpeed = Mathf.Lerp(asteroidSpawnerScript.maxAsteroidSpeed, maxAsteroidSpeedLimit, gameProgress * asteroidSpeedIncrease * 0.01f);
        }
    }

    void UpdateAsteroidSpawnInterval()
    {
        if (asteroidSpawnerScript != null)
        {
            // Decrease spawn interval range based on progress (faster spawns as progress increases)
            asteroidSpawnerScript.minSpawnInterval = Mathf.Lerp(asteroidSpawnerScript.minSpawnInterval, minSpawnIntervalLimit, gameProgress * asteroidSpawnIntervalIncrease * 0.01f);
            asteroidSpawnerScript.maxSpawnInterval = Mathf.Lerp(asteroidSpawnerScript.maxSpawnInterval, maxSpawnIntervalLimit, gameProgress * asteroidSpawnIntervalIncrease * 0.01f);
        }
    }
}
