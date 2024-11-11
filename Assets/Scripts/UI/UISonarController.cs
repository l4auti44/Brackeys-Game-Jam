using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISonarController : MonoBehaviour
{
    public float maxSize = 5f;           // Maximum size of the sonar waves
    public float waveInterval = 2f;      // Time between sonar waves
    public float scaleSpeed = 1f;        // Speed multiplier for scaling

    private bool isExpanding = false;    // To track if the sonar is currently expanding

    private void Start()
    {
        // Start the first sonar wave immediately
        StartCoroutine(TriggerSonarWaves());
    }

    private IEnumerator TriggerSonarWaves()
    {
        while (true)
        {
            // If not currently expanding, start a new expansion
            if (!isExpanding)
            {
                StartCoroutine(ExpandWave());
            }
            // Wait for the defined interval before checking again
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private IEnumerator ExpandWave()
    {
        isExpanding = true; // Mark as currently expanding
        float currentSize = 0f;

        // Increase size until it reaches the maximum size
        while (currentSize < maxSize)
        {
            currentSize += Time.deltaTime * scaleSpeed; // Adjust size by scaleSpeed
            transform.localScale = new Vector3(currentSize, currentSize, 1f); // Scale in X and Y
            yield return null; // Wait for the next frame
        }

        // Reset the scale after the wave is done
        transform.localScale = Vector3.zero; // Reset scale to 0 for next wave
        isExpanding = false; // Mark as no longer expanding
    }
}
