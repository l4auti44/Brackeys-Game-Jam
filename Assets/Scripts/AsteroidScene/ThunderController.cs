using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ThunderController : MonoBehaviour
{

    public float energyToIncrease = 10f;
    public float timeToEnableCol = 1f;
    public float timeToDestroy = 1.5f;

    [SerializeField] private int scoreToAdd = 10;
    private bool energyEarned = false;
    private GameManager gameManager; // Reference to GameManager
    private CircleCollider2D _collider;
    private GameObject lightning;
    private SpriteRenderer crosshair;
    private SpriteRenderer lightningSprite;

    private Animator lightningAnimator;
    private Animator crosshairAnimator;

    private void Start()
    {
        crosshair = GetComponent<SpriteRenderer>();
        lightningAnimator = transform.GetChild(0).GetComponent<Animator>();
        lightningSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        crosshairAnimator = GetComponent<Animator>();
        lightning = transform.GetChild(0).gameObject;
        lightning.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();  // Find the GameManager in the scene
        _collider = GetComponent<CircleCollider2D>();
        _collider.enabled = false;
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        if (SceneController.isGameStopped)
        {
            lightningAnimator.speed = 0;
            crosshairAnimator.speed = 0;
            lightningSprite.enabled = false;
            crosshair.enabled = false;
            return;
        }
        
        if (lightningAnimator.speed == 0)
        {
            lightningAnimator.speed = 1;
            crosshairAnimator.speed = 1;
            lightningSprite.enabled = true;
            crosshair.enabled = true;
        }
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
                gameManager.IncreaseEnergy(energyToIncrease);
                ScoreManager.AddScore(scoreToAdd, transform.position);
                energyEarned = true;

                //Give player feedback
            }
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        float timer = timeToDestroy;
        while (timer > 0)
        {
            if (SceneController.isGameStopped)
            {
                yield return null;
                continue;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }





}
