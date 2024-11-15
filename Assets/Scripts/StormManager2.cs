using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormManager2 : MonoBehaviour
{
    [Header("Storm Settings")]
    public float minStormInterval = 5f;  // Minimum time between storm activations
    public float maxStormInterval = 10f; // Maximum time between storm activations
    private float stormTimer;

    [Header("Storm Level Settings")]
    public float[] speedAdjustmentLV1 = new float[2]; // [minSpeed, maxSpeed] for level 1
    public float[] speedAdjustmentLV2 = new float[2]; // [minSpeed, maxSpeed] for level 2
    public float[] speedAdjustmentLV3 = new float[2]; // [minSpeed, maxSpeed] for level 3

    public float[] spawnIntervalAdjustmentLV1 = new float[2]; // [minSpawnInterval, maxSpawnInterval] for level 1
    public float[] spawnIntervalAdjustmentLV2 = new float[2]; // [minSpawnInterval, maxSpawnInterval] for level 2
    public float[] spawnIntervalAdjustmentLV3 = new float[2]; // [minSpawnInterval, maxSpawnInterval] for level 3

    public float[] alphaAdjustmentLV1 = new float[2]; // [minAlpha, maxAlpha] for level 1
    public float[] alphaAdjustmentLV2 = new float[2]; // [minAlpha, maxAlpha] for level 2
    public float[] alphaAdjustmentLV3 = new float[2]; // [minAlpha, maxAlpha] for level 3

    private GameManager gameManager; // Reference to GameManager
    private AsteroidSpawner asteroidSpawner; // Reference to AsteroidSpawner
    private SpriteRenderer spriteRenderer; // Reference to a sprite renderer for alpha changes

    private void Start()
    {
        // Get references to the GameManager and AsteroidSpawner
        gameManager = FindObjectOfType<GameManager>();
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();

        // Set initial storm timer
        SetStormTimer();
    }

    private void Update()
    {
        stormTimer -= Time.deltaTime;

        if (stormTimer <= 0f)
        {
            ActivateStorm();
            SetStormTimer();
        }
    }

    private void SetStormTimer()
    {
        stormTimer = Random.Range(minStormInterval, maxStormInterval);
    }

    private void ActivateStorm()
    {
        //Console indication
        Debug.Log("Storm is coming");
        
        // Randomly select storm level (1, 2, or 3)
        int stormLevel = Random.Range(1, 4);

        // Apply changes based on the selected storm level
        switch (stormLevel)
        {
            case 1:
                ModifyGameValues(speedAdjustmentLV1, spawnIntervalAdjustmentLV1, alphaAdjustmentLV1);
                break;
            case 2:
                ModifyGameValues(speedAdjustmentLV2, spawnIntervalAdjustmentLV2, alphaAdjustmentLV2);
                break;
            case 3:
                ModifyGameValues(speedAdjustmentLV3, spawnIntervalAdjustmentLV3, alphaAdjustmentLV3);
                break;
        }

        // Optionally, add storm visual effects or sounds here
        Debug.Log($"Storm Level {stormLevel} Activated");
    }

    private void ModifyGameValues(float[] speedAdjustment, float[] spawnIntervalAdjustment, float[] alphaAdjustment)
    {
        // Modify asteroid speed in the AsteroidSpawner
        asteroidSpawner.minAsteroidSpeed += speedAdjustment[0];
        asteroidSpawner.maxAsteroidSpeed += speedAdjustment[1];

        // Modify asteroid spawn interval in the AsteroidSpawner
        asteroidSpawner.minSpawnInterval -= spawnIntervalAdjustment[0];
        asteroidSpawner.maxSpawnInterval -= spawnIntervalAdjustment[1];

        // Modify alpha value of sprite renderer
        if (spriteRenderer != null)
        {
            float newAlpha = Mathf.Clamp(spriteRenderer.color.a + Random.Range(alphaAdjustment[0], alphaAdjustment[1]), 0f, 1f);
            Color newColor = spriteRenderer.color;
            newColor.a = newAlpha;
            spriteRenderer.color = newColor;
        }
    }

}
