using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarWaveController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "Enemy" or "EnergyCollectable"
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnergyCollectable"))
        {
            // Activate all child objects of the collided object
            foreach (Transform child in collision.transform)
            {
                child.gameObject.SetActive(true); // Activate each child object

            }

            //Activate the Light Fader
            collision.GetComponent<LightController>().enabled = true;
            collision.GetComponent<LightController>().IncreaseLightToMax();
        }
    }
}
