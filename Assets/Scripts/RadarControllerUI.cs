using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarControllerUI : MonoBehaviour
{
    public Transform player;         // The player object in the world space
    public RectTransform radarUI;    // The UI object (RectTransform) that acts as the radar
    public Canvas canvas;            // The canvas for UI elements (in case it's required)
    public GameObject wavePrefab;    // Prefab for the sonar wave
    public float maxSize = 5f;       // Maximum size of the sonar waves
    public float waveInterval = 2f;  // Time between sonar waves
    public float scaleSpeed = 1f;    // Speed multiplier for scaling

    private void Start()
    {
        // Start the sonar wave generation
        StartCoroutine(TriggerSonarWaves());
    }

    private void Update()
    {
        MatchPlayerPosition();
    }

    private void MatchPlayerPosition()
    {
        // Get the player's world position and convert it to screen space
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

        // Convert screen space to UI space and update the radarUI's position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        // Assign the local position to the radarUI element
        radarUI.localPosition = localPoint;
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
        // Instantiate the wave prefab at the radarUI's position
        GameObject waveInstance = Instantiate(wavePrefab, radarUI.position, Quaternion.identity, radarUI);

        Transform waveTransform = waveInstance.transform;
        float currentSize = 0f;

        // Increase size until it reaches the maximum size
        while (currentSize < maxSize)
        {
            currentSize += Time.deltaTime * scaleSpeed; // Adjust size by scaleSpeed
            waveTransform.localScale = new Vector3(currentSize, currentSize, 1f); // Scale in X and Y
            yield return null; // Wait for the next frame
        }

        // Destroy the wave instance after it has finished expanding
        Destroy(waveInstance); // Optionally destroy after reaching max size
    }
}
