using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    public GameObject wavePrefab;         // Prefab for the sonar wave
    public float maxSize = 5f;            // Maximum size of the sonar waves
    public float waveInterval = 2f;       // Time between sonar waves
    public float scaleSpeed = 1f;         // Speed multiplier for scaling

    private void Start()
    {
        // Start the sonar wave generation
        StartCoroutine(TriggerSonarWaves());
    }

    private IEnumerator TriggerSonarWaves()
    {
        while (true)
        {
            // Start a new expansion wave
            StartCoroutine(ExpandWave());

            // Wait for the specified interval before triggering the next wave
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private IEnumerator ExpandWave()
    {
        // Instantiate the wave prefab at the sonar's position
        GameObject waveInstance = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        Transform waveTransform = waveInstance.transform;

        float currentSize = 0f;

        // Increase size until it reaches the maximum size
        while (currentSize < maxSize)
        {
            currentSize += Time.deltaTime * scaleSpeed; // Adjust size by scaleSpeed
            waveTransform.localScale = new Vector3(currentSize, currentSize, 1f); // Scale in X and Y
            yield return null; // Wait for the next frame
        }

        // Optionally destroy the wave instance after reaching max size
        Destroy(waveInstance); // Destroy the wave after it has finished expanding
    }
}
