using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCollectableMovement : MonoBehaviour
{
    public float minSpeed = 2f; // Minimum speed of movement
    public float maxSpeed = 5f; // Maximum speed of movement
    public float minRotationSpeed = 20f; // Minimum rotation speed
    public float maxRotationSpeed = 100f; // Maximum rotation speed

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private float rotationSpeed; // The speed at which the collectable will rotate

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to the collectable
        rb = GetComponent<Rigidbody2D>();

        // Randomize the direction (angle between -45 and +45 degrees from straight down)
        float randomAngle = Random.Range(-45f, 45f);
        Vector2 movementDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.down;

        // Randomize the movement speed
        float moveSpeed = Random.Range(minSpeed, maxSpeed);

        // Apply velocity to the Rigidbody2D to move the collectable
        rb.velocity = movementDirection * moveSpeed;

        // Randomize the rotation speed
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneController.isGameStopped) return;
        // Rotate the collectable at the random rotation speed
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the asteroid upon collision 
        }
    }

}

