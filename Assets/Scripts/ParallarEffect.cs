using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallarEffect : MonoBehaviour
{
    public List<Transform> backgroundLayers; // List of child layers for the parallax effect
    public float standardSpeed;              // Base speed for all layers
    public float[] additionalSpeeds;         // Additional speed offsets for each layer
    public float resetThreshold;      // Y-position where the layer resets
    public float resetPosition;        // Y-position to reset the layer to

    private void Update()
    {
        for (int i = 0; i < backgroundLayers.Count; i++)
        {
            // Calculate the total speed for each layer (standard speed + additional speed)
            float totalSpeed = standardSpeed + additionalSpeeds[i];

            // Move each background layer downwards at the calculated speed
            backgroundLayers[i].localPosition += Vector3.down * totalSpeed * Time.deltaTime;

            // Check if the layer has moved past the reset threshold
            if (backgroundLayers[i].localPosition.y <= resetThreshold)
            {
                // Reset the layer's position to the resetPosition
                Vector3 newPos = backgroundLayers[i].localPosition;
                newPos.y = resetPosition;
                backgroundLayers[i].localPosition = newPos;
            }
        }
    }

    // Method to Modify the speed of all layers by a specified amount
    public void IncreaseSpeed(float newSpeed)
    {
        standardSpeed = newSpeed; // Modify the standard speed
    }
}
