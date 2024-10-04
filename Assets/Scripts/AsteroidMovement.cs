using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    private float screenBottomY; // Y position of the bottom of the screen

    // Start is called before the first frame update
    void Start()
    {
        // Get the screen bottom Y position
        screenBottomY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the asteroid if it goes off the screen
        if (transform.position.y < screenBottomY - 1f) // Adding some margin
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the asteroid upon collision 
        }
    }

}

