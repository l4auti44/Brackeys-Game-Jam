using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFeelManager : MonoBehaviour
{
    [HideInInspector]
    public Transform cameraTransform; // The camera that will shake
    public float shakeDuration = 0.5f; // How long the camera shakes
    public float shakeMagnitude = 0.2f; // How intense the shake is
    private bool isShaking = false;

    private Vector3 originalPosition; // Store the camera's original position

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Default to the main camera if not set
        }
        originalPosition = cameraTransform.localPosition; // Save the camera's initial position
    }

    // Call this function to start the screen shake
    public void StartShake(float duration, float magnitude)
    {
        if (isShaking) return; // Skip if already shaking

        shakeDuration = duration;
        shakeMagnitude = magnitude;
        isShaking = true;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Generate a random offset for the camera position
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0);

            // Apply shake to the camera
            cameraTransform.localPosition = originalPosition + shakeOffset;

            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Reset the camera to its original position
        cameraTransform.localPosition = originalPosition;
        isShaking = false;
    }


    public void StopFrame(float duration)
    {
        Time.timeScale = 1f;
        StartCoroutine(DoFrameStop(duration));
    }

    private IEnumerator DoFrameStop(float duration)
    {
        // Save the current time scale
        float originalTimeScale = Time.timeScale;

        // Set the time scale to zero to pause the game
        Time.timeScale = 0f;

        // Wait for the specified duration (scaled by unscaled time to avoid getting affected by Time.timeScale)
        yield return new WaitForSecondsRealtime(duration);

        // Restore the original time scale to resume the game
        Time.timeScale = originalTimeScale;
    }


}
