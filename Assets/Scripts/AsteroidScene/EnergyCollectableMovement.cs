using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCollectableMovement : MonoBehaviour
{
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void StopMovement()
    {
        rb.simulated = false;
    }

    private void ResumeMovement()
    {
        rb.simulated = true;
    }

    void Awake()
    {
        EventManager.Game.OnGameStopped += StopMovement;
        EventManager.Game.OnPerkPicked += ResumeMovement;
    }

    void OnDestroy()
    {
        EventManager.Game.OnGameStopped -= StopMovement;
        EventManager.Game.OnPerkPicked -= ResumeMovement;
    }

}

