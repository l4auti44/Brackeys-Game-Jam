using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Settings defaultsettings;
    [SerializeField] Settings[] acrosslevels;
    public Transform pos1;
    public Transform pos2;
    public GameObject[] asteroidPrefabs;
    public GameObject arrow1Prefab;
    public GameObject arrow2Prefab;
    public GameObject arrow3Prefab;
    public bool spawnArrow = true;
    public float moveSpeed = 2f;
    public float minSpawnInterval = 1f;  // Minimum spawn interval
    public float maxSpawnInterval = 5f;  // Maximum spawn interval
    public float minAsteroidSpeed = 0.1f;
    public float maxAsteroidSpeed = 5f;
    public float minRotationSpeed = 10f;
    public float maxRotationSpeed = 100f;
    public float arrowYOffset = 1f;
    public Transform player; // Assign the player transform in the inspector or dynamically
    public int asteroidsBeforeFollow = 5; // Number of asteroids to spawn before following the player once

    private Vector3 pos1Value;
    private Vector3 pos2Value;
    private Vector3 targetPos;
    private float spawnTimer;
    private int asteroidSpawnCount = 0;
    private bool isFollowingPlayer = false;

    void Start()
    {
        EnforceDefaultSettings();
        pos1Value = pos1.position;
        pos2Value = pos2.position;
        targetPos = pos2Value;

        // Randomize the initial spawn timer
        spawnTimer = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (!SceneController.isGameStopped)
        {
            if (isFollowingPlayer)
            {
                FollowPlayerOnXAxis();
            }
            else
            {
                MoveBetweenPositions();
            }

            HandleAsteroidSpawning();
        }
    }

    void MoveBetweenPositions()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = targetPos == pos1Value ? pos2Value : pos1Value;
        }
    }

    void FollowPlayerOnXAxis()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = player.position.x;
        transform.position = newPosition;
    }

    void HandleAsteroidSpawning()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnAsteroid();
            spawnTimer = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval); // Reset spawn timer

            // Check if we should follow the player next
            asteroidSpawnCount++;
            if (asteroidSpawnCount % asteroidsBeforeFollow == 0)
            {
                isFollowingPlayer = true;
            }
            else
            {
                isFollowingPlayer = false;
            }
        }
    }

    void SpawnAsteroid()
    {
        GameObject randomAsteroid = asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Length)];
        GameObject asteroid = Instantiate(randomAsteroid, transform.position, Quaternion.identity);

        float randomSpeed = UnityEngine.Random.Range(minAsteroidSpeed, maxAsteroidSpeed);
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * randomSpeed;
        }

        float randomRotationSpeed = UnityEngine.Random.Range(minRotationSpeed, maxRotationSpeed);
        rb.angularVelocity = randomRotationSpeed;

        if (spawnArrow)
        {
            float speedRange = maxAsteroidSpeed - minAsteroidSpeed;
            GameObject selectedArrowPrefab;

            if (randomSpeed > minAsteroidSpeed + (2f / 3f) * speedRange)
            {
                selectedArrowPrefab = arrow3Prefab;
            }
            else if (randomSpeed > minAsteroidSpeed + (1f / 3f) * speedRange)
            {
                selectedArrowPrefab = arrow2Prefab;
            }
            else
            {
                selectedArrowPrefab = arrow1Prefab;
            }

            Vector3 arrowPosition = asteroid.transform.position + new Vector3(0, arrowYOffset, 0);
            GameObject arrow = Instantiate(selectedArrowPrefab, arrowPosition, Quaternion.identity);

            float distance = Mathf.Abs(arrowYOffset);
            float lifetime = distance / randomSpeed;
            lifetime = Mathf.Clamp(lifetime, 0.5f, 10f);

            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            if (arrowController != null)
            {
                arrowController.SetLifetime(lifetime);
            }
        }
    }

    void EnforceDefaultSettings()
    {
        if (acrosslevels.Length > 0)
        {
            acrosslevels[0] = defaultsettings;
        }
    }

    public void TweakAsteroidSettings(int lvl)
    {
        if (lvl <= acrosslevels.Length)
        {
            lvl = lvl - 1;
            moveSpeed = acrosslevels[lvl].moveSpeed;
            minSpawnInterval = acrosslevels[lvl].minSpawnInterval;  // Minimum spawn interval
            maxSpawnInterval = acrosslevels[lvl].maxSpawnInterval;  // Maximum spawn interval

            arrowYOffset = acrosslevels[lvl].arrowYOffset;
            asteroidsBeforeFollow = acrosslevels[lvl].asteroidsBeforeFollow; // Number of asteroids to spawn before following the player once
        }
}

    [Serializable]
    public struct Settings 
    {

        public float moveSpeed;
        public float minSpawnInterval;  // Minimum spawn interval
        public float maxSpawnInterval;  // Maximum spawn interval
        public float arrowYOffset;
        public int asteroidsBeforeFollow; // Number of asteroids to spawn before following the player once

    }
}
