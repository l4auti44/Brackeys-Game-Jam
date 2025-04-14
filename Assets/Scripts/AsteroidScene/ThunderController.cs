using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ThunderController : MonoBehaviour
{

    public float energyToIncrease = 10f;
    public float timeToEnableCol = 1f;
    public float timeToDestroy = 1.5f;

    private bool energyEarned = false;
    private GameManager gameManager; // Reference to GameManager
    private CircleCollider2D _collider;
    private GameObject lightning;
    private SpriteRenderer crosshair;

    private void Start()
    {
        crosshair = GetComponent<SpriteRenderer>();
        lightning = transform.GetChild(0).gameObject;
        lightning.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();  // Find the GameManager in the scene
        _collider = GetComponent<CircleCollider2D>();
        _collider.enabled = false;
        Destroy(gameObject, timeToDestroy);
        // Start the state change process
        //StartCoroutine(ChangeState());
    }

    private void Update()
    {
        timeToEnableCol -= Time.deltaTime;
        if (timeToEnableCol < 0)
        {
            _collider.enabled = true;
            crosshair.enabled = false;
            lightning.SetActive(true);
            timeToEnableCol = 900f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with the thunder
        if (collision.CompareTag("Player"))
        {
            if (!energyEarned)
            {
                Debug.Log("Energy earned");
                // Increase the energy of the game manager
                gameManager.energy += energyToIncrease;
                energyEarned = true;

                //Give player feedback
            }
        }
    }





}
