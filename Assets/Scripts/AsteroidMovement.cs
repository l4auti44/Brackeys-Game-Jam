using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float minSpeed = 2f; // Minimum speed for asteroid movement
    public float maxSpeed = 5f; // Maximum speed for asteroid movement
    public float minRotationSpeed = 20f; // Minimum rotation speed
    public float maxRotationSpeed = 100f; // Maximum rotation speed
    public float minAngleDirection = 20f; // Min angle direction
    public float maxAngleDirection = 100f; // Maximum angle direction

    private Vector2 movementDirection; // The direction in which the asteroid will move
    private float moveSpeed; // The speed of movement
    public float rotationSpeed; // The speed at which the asteroid rotates

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

