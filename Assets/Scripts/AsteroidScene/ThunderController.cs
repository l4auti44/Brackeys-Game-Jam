using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ThunderState { Idle, ThunderYellow, ThunderRed }

public class ThunderController : MonoBehaviour
{
    public ThunderState currentState = ThunderState.Idle; // Initial state
    public float timeToYellow = 5f;  // Time to switch to ThunderYellow
    public float timeToRed = 3f;     // Time to switch to ThunderRed after ThunderYellow
    public float timeToIdle = 4f;    // Time to switch back to Idle from ThunderRed
    public float energyToDecrease = 10f;
    public float timeInactivateText = 1f;
    public float timeToDestroy = 1.5f;



    private SpriteRenderer spriteRenderer;
    public GameManager gameManager; // Reference to GameManager
    public GameObject textParrySucess; //Reference to the text that appears when you do a parry successfully

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        gameManager = FindObjectOfType<GameManager>();  // Find the GameManager in the scene
        textParrySucess = GameObject.Find("ParryIndicator");

        Destroy(gameObject, timeToDestroy);
        // Start the state change process
        //StartCoroutine(ChangeState());
    }

    private IEnumerator ChangeState()
    {
        while (true)
        {
            // Switch to ThunderYellow after timeToYellow seconds
            yield return new WaitForSeconds(timeToYellow);
            SetState(ThunderState.ThunderYellow);

            // Switch to ThunderRed after timeToRed seconds
            yield return new WaitForSeconds(timeToRed);
            SetState(ThunderState.ThunderRed);

            // Destroy after timeToIdle seconds
            yield return new WaitForSeconds(timeToIdle);
            SetState(ThunderState.Idle);
        }
    }

    private void SetState(ThunderState newState)
    {
        currentState = newState;

        // Update the sprite based on the new state
        switch (newState)
        {
            case ThunderState.Idle:
                Destroy(gameObject);
                break;
            case ThunderState.ThunderYellow:
                //spriteRenderer.sprite = yellowSprite;
                break;
            case ThunderState.ThunderRed:
                //spriteRenderer.sprite = redSprite;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the player collided with the thunder
        if (collision.CompareTag("Player") && currentState == ThunderState.ThunderRed && gameManager.isShieldActive)
        {
            // Increase the energy of the game manager
            gameManager.energy = gameManager.maxEnergy;

            //Give player feedback
            textParrySucess.gameObject.SetActive(true);
            TriggerInactivation(timeInactivateText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player collided with the thunder
        if (collision.CompareTag("Player") && !gameManager.isShieldActive)
        {
            Debug.Log("Decrease enegy");
            // Increase the energy of the game manager
            gameManager.energy -= energyToDecrease;
        }
    }

    public void TriggerInactivation(float customDelay)
    {
        StartCoroutine(InactivateObjectAfterDelay(customDelay));
    }

    IEnumerator InactivateObjectAfterDelay(float customDelay)
    {
        yield return new WaitForSeconds(customDelay);
        if (textParrySucess != null)
        {
            textParrySucess.gameObject.SetActive(false);
        }
    }



}
