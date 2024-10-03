using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFeelManager : MonoBehaviour
{
    [HideInInspector]
    public Transform cameraTransform; // The camera that will shake
    public float shakeDuration = 0.5f; // How long the camera shakes
    public float shakeMagnitude = 0.2f; // How intense the shake is
    public List<Transform> unaffectedObjects; // List of objects that should not shake

    private Vector3 originalPosition; // Store the camera's original position
    private Dictionary<Transform, Vector3> originalObjectPositions; // Store the unaffected objects' original positions

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Default to the main camera if not set
        }
        originalPosition = cameraTransform.localPosition; // Save the camera's initial position

        // Initialize the dictionary for original positions
        originalObjectPositions = new Dictionary<Transform, Vector3>();

        // Store the original positions for unaffected objects
        foreach (var obj in unaffectedObjects)
        {
            if (obj != null)
            {
                originalObjectPositions[obj] = obj.position; // Save each object's initial position
            }
        }
    }

    // Call this function to start the screen shake
    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
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

            // Counteract shake for unaffected objects
            foreach (var obj in unaffectedObjects)
            {
                if (obj != null)
                {
                    obj.position = originalObjectPositions[obj] - shakeOffset; // Use the stored original position
                }
            }

            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Reset the camera to its original position
        cameraTransform.localPosition = originalPosition;

        // Reset unaffected objects to their original positions
        foreach (var obj in unaffectedObjects)
        {
            if (obj != null)
            {
                obj.position = originalObjectPositions[obj]; // Use the stored original position
            }
        }
    }

    // Method to add an object to the unaffected list
    public void AddUnaffectedObject(Transform obj)
    {
        if (!unaffectedObjects.Contains(obj))
        {
            unaffectedObjects.Add(obj);
            originalObjectPositions[obj] = obj.position; // Save the original position
        }
    }

    public void StopFrame(float duration)
    {
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
