using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_Movement : MonoBehaviour
{
    public float missileSpeed = 10f; // Speed of the missile
    public float destroyDelay = 5f; // Time after which missile is destroyed if it doesn't hit anything

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the missile after a delay to avoid clutter if it goes off-screen
        Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the missile upwards
        transform.Translate(Vector2.up * missileSpeed * Time.deltaTime);
    }

    // Detect collisions with other objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the missile hits an enemy (tagged as "Enemy"), destroy both the missile and the enemy
        if (collision.CompareTag("Enemy"))
        {
            //Trigger the destruction of asteroid
            collision.gameObject.GetComponent<AsteroidMovement>().DestroyAsteroid();

            Destroy(gameObject); // Destroy the missile
        }
    }
}
