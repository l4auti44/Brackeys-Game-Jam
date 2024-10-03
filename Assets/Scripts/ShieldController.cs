using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public Transform player;     // The player's transform that the object will follow
    public float followSpeed = 2f; // Speed at which the object follows the player

    void Update()
    {
        if (player != null) // Ensure the player transform is assigned
        {
            // Move towards the player's position
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy the asteroid upon collision 
        }
    }

}
