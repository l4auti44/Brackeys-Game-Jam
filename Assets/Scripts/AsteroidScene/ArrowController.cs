using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private float lifetime;
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        Transform objectToAdd = this.transform; // Replace with the actual Transform you want to add

        // Destroy the arrow after 'lifetime' seconds
        Destroy(gameObject, lifetime);

    }
    public void SetLifetime(float time)
    {
        lifetime = time;
    }
}
