using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DialogSystem;


public class GameManager : MonoBehaviour
{
    [System.Serializable]
    private class AsteroidMovementLevel
    {
        public Levels level;
        public float minSpeed;
        public float maxSpeed;
        public float minRotation;
        public float maxRotation;
    }

    // --- Game Progression ---
    [Header("Game Progression")]
    public float gameProgress;
    public float gameProgressSpeed;
    public GameObject gameProgressSlider;
    public float[] gameProgressSpeedPositionLVS = new float[5];

    private bool dead = false;

    // --- Game Objects ---
    [Header("Game Objects")]
    public GameObject ship;
    public ShipMovement ship_script;
    public AsteroidSpawner asteroidSpawner;
    public GameObject  energySpawner, shield, radar;
    public ParallarEffect parallax;
    public Camera mainCamera;

    // --- Radar & Position Modules ---
    [Header("Radar & Position Modules")]
    public Image[] radarModuleSpriteLVS = new Image[4];
    public Image[] positionModuleSpriteLVS = new Image[5];
    public GameObject arrowUI, shieldUI;

    // --- Ship & Movement Settings ---
    [Header("Ship & Movement Settings")]
    public float shipSpeed;
    public int radarLV, positionLV;
    public float minX, maxX; // Minimum and maximum X boundary for player movement
    public float asteroidFadeLightSpeed; // Asteroid light fade speed

    // --- Speed Variations ---
    [Header("Speed Variations")]
    public float[] speedLVS = new float[5];
    [SerializeField] private AsteroidMovementLevel[] asteroidMovementLevels = new AsteroidMovementLevel[5];
    public float speedDecreaseRateWhenGameOver;

    // --- Radar Wave Intervals ---
    [Header("Radar Wave Intervals")]
    public float[] radarWaveIntervalLVS = new float[4];

    // --- Position Settings ---
    [Header("Position Settings")]
    public Transform[] positionLVS = new Transform[5];

    public float[] durationToPositions = new float[5];

    // --- Parallax Speed Settings ---
    [Header("Parallax Speed Settings")]
    public float[] speedParallaxLVS = new float[5];

    // --- Camera Settings ---
    [Header("Camera Settings")]
    public float[] cameraPosLVS = new float[5];
    public float cameraZoomOutSpeed = 2;
    private float targetZoom; // Target orthographic size
    private float _originalZoom; // Target orthographic size

    // --- Energy Settings ---
    [Header("Energy Settings")]
    public float energy = 100f; // Amount of energy at any time
    private float energyDecreaseSpeed; // Speed at which the energy is depleted
    public float maxEnergy = 100f; // Amount of total energy

    // --- Energy Modifiers ---
    [Header("Energy Modifiers")]
    public float radarModEnergyDecreaseLV1;
    public float radarModEnergyDecreaseLV2, radarModEnergyDecreaseLV3, radarModEnergyDecreaseLV4;
    private float radarModEnergyDecreaseLV1_original, radarModEnergyDecreaseLV2_original, radarModEnergyDecreaseLV3_original, radarModEnergyDecreaseLV4_original;
    public float shieldModEnergyDecrease;
    private float shieldModEnergyDecrease_original;
    public float positionModEnergyDecreaseLV1, positionModEnergyDecreaseLV2, positionModEnergyDecreaseLV3, positionModEnergyDecreaseLV4, positionModEnergyDecreaseLV5;
    private float positionModEnergyDecreaseLV1_original, positionModEnergyDecreaseLV2_original, positionModEnergyDecreaseLV3_original, positionModEnergyDecreaseLV4_original, positionModEnergyDecreaseLV5_original;
    public float arrowModEnergyDecrease;
    private float arrowModEnergyDecrease_original;
    public Slider energySlider, energyMax;

    // --- Shield Settings ---
    [Header("Shield Settings")]
    public GameObject shieldObject;      // The shield GameObject
    public float shieldDuration = 5f;    // How long the shield stays active
    public float shieldCooldown = 10f;   // Cooldown before shield can be reactivated
    public bool isShieldActive = false;
    public bool isShieldCooldown = false;

    // --- Magnetism Settings ---

    public GameObject magnetic_field_Object;

    public MagnetismField magnetic_field;

    // --- Overload Settings ---
    [Header("Overload Settings")]
    public bool isOverload = false;  // Tracks overload state
    public float overloadDuration = 5f;  // Duration of the overload state

    // --- Game Over Timer ---
    [Header("Game Over Timer")]
    public float timerForGameOver = 10f;

    // --- Sound Modules ---
    [Header("Sound Modules")]
    private SoundButton shieldModule;
    //private SoundButton arrowModule;
    private List<SoundButton> engineModules = new List<SoundButton>();
    private List<SoundButton> radarModules = new List<SoundButton>();

    // --- Repair System ---
    private DestroyAndRepearSys repairSys;

    private TextMeshPro totalRateEnergyDecrease;
    //temporal flags
    private bool eventFlag = false;
    private float eventTimer = 20f;

    // --- Perk Related ---

    [SerializeField] private PerkController perkController;



    [SerializeField] private Image EngineModuleMeterImage;

  
    [SerializeField] private Image RadarModuleMeterImage;

    private float scoreTimer = 0f;

    enum Levels
    {
        Lv1,
        Lv2,
        Lv3,
        Lv4,
        Lv5
    }
    void Start()
    {
        SceneController.StartGame();
        ScoreManager.ResetScore();
        InitializeShip();
        StoreOriginalEnergyModifiers();
        ResetEnergyModifiers();
        InitializeGameSettings();
        InitializeModules();
        InitializeUIReferences();
        perkController.ReInitializeCards();
    }
    #region StartMethods
    private void InitializeShip()
    {
        ship.GetComponent<ShipMovement>().speed = shipSpeed;
    }

    private void StoreOriginalEnergyModifiers()
    {
        radarModEnergyDecreaseLV1_original = radarModEnergyDecreaseLV1;
        radarModEnergyDecreaseLV2_original = radarModEnergyDecreaseLV2;
        radarModEnergyDecreaseLV3_original = radarModEnergyDecreaseLV3;
        radarModEnergyDecreaseLV4_original = radarModEnergyDecreaseLV4;

        shieldModEnergyDecrease_original = shieldModEnergyDecrease;

        positionModEnergyDecreaseLV1_original = positionModEnergyDecreaseLV1;
        positionModEnergyDecreaseLV2_original = positionModEnergyDecreaseLV2;
        positionModEnergyDecreaseLV3_original = positionModEnergyDecreaseLV3;
        positionModEnergyDecreaseLV4_original = positionModEnergyDecreaseLV4;
        positionModEnergyDecreaseLV5_original = positionModEnergyDecreaseLV5;

        arrowModEnergyDecrease_original = arrowModEnergyDecrease;
    }

    private void ResetEnergyModifiers()
    {
        radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV3 = radarModEnergyDecreaseLV4 = 0;
        shieldModEnergyDecrease = 0;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV4 = positionModEnergyDecreaseLV5 = 0;
        arrowModEnergyDecrease = 0;
    }

    private void InitializeGameSettings()
    {
        //energy = maxEnergy;
        shipSpeed = speedLVS[0];
        positionLV = 1;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

        radarLV = 1;
        ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLVS[0];
        radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;

        gameProgressSpeed = gameProgressSpeedPositionLVS[0];
        asteroidSpawner.spawnArrow = false;
        parallax.IncreaseSpeed(speedParallaxLVS[0]);

        mainCamera = Camera.main;
        targetZoom = mainCamera.orthographicSize;
        _originalZoom = mainCamera.orthographicSize;
    }

    private void InitializeModules()
    {
        shieldModule = GameObject.Find("ShieldModule").GetComponent<SoundButton>();
        //arrowModule = GameObject.Find("ArrowModule").GetComponent<SoundButton>();

        var engineButtons = GameObject.Find("EngineButtons");
        engineModules.Add(engineButtons.transform.GetChild(0).GetComponent<SoundButton>());
        engineModules.Add(engineButtons.transform.GetChild(1).GetComponent<SoundButton>());

        var radarButtons = GameObject.Find("RadarButtons");
        radarModules.Add(radarButtons.transform.GetChild(0).GetComponent<SoundButton>());
        radarModules.Add(radarButtons.transform.GetChild(1).GetComponent<SoundButton>());

        repairSys = GetComponentInChildren<DestroyAndRepearSys>();
    }

    private void InitializeUIReferences()
    {
        totalRateEnergyDecrease = GameObject.Find("Total Rate").GetComponentInChildren<TextMeshPro>();
        energyMax.gameObject.SetActive(false);

    }
    #endregion
    
    

    void Update()
    {
        if (SceneController.isGameStopped) return;

        HandleDialogEvents();
        if (!SceneController.isGamePaused)
        {
            if (!dead)
                UpdateGameProgress();
            UpdateEnergy();
            HandleGameOver();
            ClampEnergy();
            UpdateCameraZoom();
            UpdateUI();
            if (!dead)
                UpdateScore();
        }
    }
    #region UpdateMethods

    private void UpdateScore()
    {
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 0.2f) // Cada 1 segundo
        {
            if (gameProgress > 1)
            {
                ScoreManager.AddScore(Mathf.FloorToInt(gameProgress * 1f));
            }
            else
            {
                ScoreManager.AddScore(1);
            }
            scoreTimer = 0f; // Reinicia el timer
        }
    }
    private void HandleDialogEvents()
    {
        if (!eventFlag) eventTimer -= Time.deltaTime;
        else eventTimer = 20f;


        if (gameProgress > 8f && !eventFlag && eventTimer <= 0)
        {
            var dialogEvents = new DialogSystem.DialogEvents[]
            {
                DialogSystem.DialogEvents.KeepEngineAt,
                DialogSystem.DialogEvents.KeepRadarAt,
                DialogSystem.DialogEvents.NoGetHit
            };

            EventManager.Game.OnDialog.Invoke(dialogEvents[Random.Range(0, dialogEvents.Length)]);
            eventFlag = true;
        }
    }

    private void UpdateGameProgress()
    {
        gameProgress += gameProgressSpeed * Time.deltaTime;
        gameProgressSlider.GetComponent<Slider>().value = gameProgress;

        if (gameProgress >= 100f)
        {
            perkController.ReInitializeCards();
            EventManager.Game.OnWin.Invoke();
        }
    }

    private void UpdateEnergy()
    {
        energyDecreaseSpeed = CalculateEnergyDecreaseSpeed();
        energy -= energyDecreaseSpeed * 0.01f;
        energySlider.value = energy * 0.01f;
    }

    private float CalculateEnergyDecreaseSpeed()
    {
        return (radarModEnergyDecreaseLV1 +
                radarModEnergyDecreaseLV2 +
                radarModEnergyDecreaseLV3 +
                radarModEnergyDecreaseLV4 +
                shieldModEnergyDecrease +
                positionModEnergyDecreaseLV1 +
                positionModEnergyDecreaseLV2 +
                positionModEnergyDecreaseLV3 +
                positionModEnergyDecreaseLV4 +
                positionModEnergyDecreaseLV5 +
                arrowModEnergyDecrease_original) * 0.1f;
    }

    private void HandleGameOver()
    {
        if (energy >= 0) 
        {
            timerForGameOver = 10f;
            return;
        }

        shipSpeed = Mathf.Max(0, shipSpeed - speedDecreaseRateWhenGameOver * Time.deltaTime);
        radarLV = 0;
        SoundManager.PlaySound(SoundManager.Sound.EnergyAtZeroWarning);

        timerForGameOver -= Time.deltaTime;
        if (timerForGameOver <= 0f)
        {
            timerForGameOver = 99999;
            dead = true;
            EventManager.Game.OnDie.Invoke(this);
        }
    }

    private void ClampEnergy()
    {
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    private void UpdateCameraZoom()
    {
        mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, targetZoom, cameraZoomOutSpeed * Time.deltaTime);
    }

    public void ResetCameraZoom()
    {
        mainCamera.orthographicSize = _originalZoom;
    }

    private void UpdateUI()
    {
        totalRateEnergyDecrease.text = energyDecreaseSpeed.ToString();
    }
    #endregion

    #region OnChosenPerk

    public void EnergyToMax()
    {
        energy = maxEnergy;
        RestartMaxEnergy();
    }
    

    #endregion




    public void DecreaseEnergy(float energyToDecrease)
    {
        energy -= energyToDecrease;
    }

    public void IncreaseEnergy(float energyToIncrease)
    {
        energy += energyToIncrease;
    }

    public void DecreaseMaxEnergy(float amount)
    {
        maxEnergy -= amount;
        if (!energyMax.gameObject.activeSelf)
        {
            energyMax.gameObject.SetActive(true);
        }
        energyMax.value += amount * 0.01f;
    }
    public void RestartMaxEnergy()
    {
        maxEnergy = 1000;
        energyMax.value = 0;
        energyMax.gameObject.SetActive(false);
    }
    


    #region RadarRelatedRegion

    public void IncreaseRadar()
    {
        if (SceneController.isGameStopped || radarModules[0].isBroken || radarLV == 4) return;

        radarLV++;
        UpdateRadarSettings();
        UpdateEnergyModifiersForRadar();
        TriggerRadarChangeEvents();
    }

    public void DecreaseRadar()
    {
        if (SceneController.isGameStopped || radarModules[1].isBroken || radarLV == 1) return;

        radarLV--;
        UpdateRadarSettings();
        ResetEnergyModifiersForRadar();
        TriggerRadarChangeEvents();
    }

    private void UpdateRadarSettings()
    {
        ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLVS[radarLV - 1];
    }

    private void UpdateEnergyModifiersForRadar()
    {
        switch (radarLV)
        {
            case 4:
                radarModEnergyDecreaseLV4 = radarModEnergyDecreaseLV4_original;
                break;
            case 3:
                radarModEnergyDecreaseLV3 = radarModEnergyDecreaseLV3_original;
                break;
            case 2:
                radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV2_original;
                break;
        }
    }

    private void ResetEnergyModifiersForRadar()
    {
        switch (radarLV)
        {
            case 3:
                radarModEnergyDecreaseLV4 = 0;
                radarModEnergyDecreaseLV3 = radarModEnergyDecreaseLV3_original;
                break;
            case 2:
                radarModEnergyDecreaseLV3 = 0;
                radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV2_original;
                break;
            case 1:
                radarModEnergyDecreaseLV2 = 0;
                radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
                break;
        }
    }

    private void TriggerRadarChangeEvents()
    {
        ChangeRadarSprite();
        EventManager.Game.OnRadarChange.Invoke(radarLV);
    }

        public void ChangeRadarSprite()
    {

        switch (radarLV)
        {
            case 4:
                RadarModuleMeterImage.fillAmount = 1f;
                break;
            case 3:
                RadarModuleMeterImage.fillAmount = 0.75f;
                break;
            case 2:
                RadarModuleMeterImage.fillAmount = 0.50f;
                break;
            case 1:
                RadarModuleMeterImage.fillAmount = 0.25f;
                break;

        }
    }

    #endregion

    public void ActivateShield()
    {
        if (isShieldCooldown && SceneController.isGameStopped) return;
        
        StartCoroutine(ShieldRoutine());
        
    }

    private IEnumerator ShieldRoutine()
    {
        Debug.Log("Shield Activated");
        isShieldActive = true;
        isShieldCooldown = true;

        // Activate the shield
        shieldObject.GetComponent<CapsuleCollider2D>().enabled = true;
        shieldObject.GetComponent<SpriteRenderer>().enabled = true;

        shieldModEnergyDecrease = shieldModEnergyDecrease_original;


        // Wait for the shield duration
        yield return new WaitForSeconds(shieldDuration);

        // Deactivate the shield
        shieldObject.GetComponent<CapsuleCollider2D>().enabled = false;
        shieldObject.GetComponent<SpriteRenderer>().enabled = false;

        isShieldActive = false;
        shieldModEnergyDecrease = 0;


        // Start the cooldown
        yield return new WaitForSeconds(shieldCooldown);

        // Allow shield to be reactivated after cooldown
        isShieldCooldown = false;

    }

    public void ToggleArrows()
    {
        if (!SceneController.isGameStopped) return;
        
        //if the number of tumes that the key was pressed is an odd number, the shield will be activated.
        //the count starts with 0, so the first hit will be a 1 => odd number => activate shield. Next press is 2 => even number => inactivate
        asteroidSpawner.spawnArrow = !asteroidSpawner.spawnArrow;
        energySpawner.GetComponent<EnergySpawner>().spawnArrow = !energySpawner.GetComponent<EnergySpawner>().spawnArrow;

        if (asteroidSpawner.spawnArrow)
        {
            arrowModEnergyDecrease = arrowModEnergyDecrease_original;
        }

        else
        {
            arrowModEnergyDecrease = 0;
        }
        
           
    }

    #region EngineRelatedRegion

    public void IncreasePositionShip()
    {
        if (SceneController.isGameStopped || engineModules[0].isBroken || positionLV == 5) return;

        positionLV++;
        UpdateShipPosition();
        UpdateEnergyModifiersForPosition();
        TriggerPositionChangeEvents();
    }

    public void DecreasePositionShip()
    {
        if (SceneController.isGameStopped || engineModules[1].isBroken || positionLV == 1) return;

        positionLV--;
        UpdateShipPosition();
        ResetEnergyModifiersForPosition();
        TriggerPositionChangeEvents();
    }

    private void UpdateShipPosition()
    {
        ship.GetComponent<ShipMovement>().MoveToLevel(positionLVS[positionLV - 1]);
        parallax.IncreaseSpeed(speedParallaxLVS[positionLV - 1]);
        ship.GetComponent<ShipMovement>().moveDuration = durationToPositions[positionLV - 1];
        ZoomOut(cameraPosLVS[positionLV - 1]);
    }

    private void UpdateEnergyModifiersForPosition()
    {
        switch (positionLV)
        {
            case 5:
                positionModEnergyDecreaseLV5 = positionModEnergyDecreaseLV5_original;
                break;
            case 4:
                positionModEnergyDecreaseLV4 = positionModEnergyDecreaseLV4_original;
                break;
            case 3:
                positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV3_original;
                break;
            case 2:
                positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV2_original;
                break;
        }
    }

    private void ResetEnergyModifiersForPosition()
    {
        switch (positionLV)
        {
            case 4:
                positionModEnergyDecreaseLV5 = 0;
                positionModEnergyDecreaseLV4 = positionModEnergyDecreaseLV4_original;
                break;
            case 3:
                positionModEnergyDecreaseLV4 = 0;
                positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV3_original;
                break;
            case 2:
                positionModEnergyDecreaseLV3 = 0;
                positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV2_original;
                break;
            case 1:
                positionModEnergyDecreaseLV2 = 0;
                positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;
                break;
        }
    }

    private void TriggerPositionChangeEvents()
    {
        ToggleAsteroidSpeed();
        ChangeEngineSprite();
        EventManager.Game.OnEngineChange.Invoke(positionLV);
    }

    public void AccelerateShip()
    {
        for (int i = 0; i < speedLVS.Length; i++)
        { 
            if (shipSpeed == speedLVS[i])
            {
                if (i < speedLVS.Length - 1)
                {
                    shipSpeed = speedLVS[i + 1];
                    gameProgressSpeed = gameProgressSpeedPositionLVS[i + 1];
                }
                break;
            }
        }

        
    }

 public void DecelerateShip()
{
    for (int i = 0; i < speedLVS.Length; i++)
    {
        if (shipSpeed == speedLVS[i])
        {
            if (i > 0)
            {
                shipSpeed = speedLVS[i - 1];
                gameProgressSpeed = gameProgressSpeedPositionLVS[i - 1];
            }
            break;
        }
    }
}

    #endregion

    public void ToggleAsteroidSpeed()
    {
        Levels currentLV = GetCurrentLV(positionLV);

        var asteroidSpeed = asteroidMovementLevels.FirstOrDefault<AsteroidMovementLevel>(x => x.level == currentLV);
        asteroidSpawner.minAsteroidSpeed = asteroidSpeed.minSpeed;
        asteroidSpawner.maxAsteroidSpeed = asteroidSpeed.maxSpeed;
        asteroidSpawner.minRotationSpeed = asteroidSpeed.minRotation;
        asteroidSpawner.maxRotationSpeed = asteroidSpeed.maxRotation;
        //Change speed of all spawned asteroids in the scene to the average number of the range in that level
        var lv5MaxSpeed = asteroidMovementLevels.FirstOrDefault<AsteroidMovementLevel>(x => x.level == Levels.Lv5).maxSpeed;
        ChangeAsteroidSpeeds(lv5MaxSpeed - asteroidSpeed.minSpeed);
        
    }

    private Levels GetCurrentLV(int positionLV)
    {
        switch (positionLV)
        {
            case 5:
                return Levels.Lv5;
            case 4:
                return  Levels.Lv4;
            case 3:
                return Levels.Lv3;
            case 2:
                return Levels.Lv2;
            case 1:
                return Levels.Lv1;
            default:
                return Levels.Lv1;
        }
    }

    public void ChangeAsteroidSpeeds(float speed)
    {
        // Find all asteroids in the scene
        AsteroidMovement[] asteroids = FindObjectsOfType<AsteroidMovement>();

        // Change the speed of each asteroid
        foreach (AsteroidMovement asteroid in asteroids)
        {
            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate the direction of the current movement
                Vector2 currentDirection = rb.velocity.normalized;

                // Apply force in the current direction
                rb.AddForce(currentDirection * speed, ForceMode2D.Impulse);
            }
        }
    }

    public void ZoomOut(float newZoom)
    {
        if (mainCamera != null)
        {
            targetZoom = newZoom; // Set the target orthographic size
        }
    }


    private IEnumerator OverloadCountdown()
    {
        // Optional: Add any visual or gameplay effect for overload state
        Debug.Log("Overload started!");

        // Wait for the overload duration
        yield return new WaitForSeconds(overloadDuration);

        // Reset overload state to false after the countdown
        isOverload = false;

        // Optional: Add any effect when overload ends
        Debug.Log("Overload ended.");
    }

    public void ChangeEngineSprite()
    {
        
        switch (positionLV)
        {
            case 5:
                EngineModuleMeterImage.fillAmount = 1f;
                break;
            case 4:
                EngineModuleMeterImage.fillAmount = 0.8f;
                break;
            case 3:
                EngineModuleMeterImage.fillAmount = 0.6f;
                break;
            case 2:
                EngineModuleMeterImage.fillAmount = 0.4f;
                break;
            case 1:
                EngineModuleMeterImage.fillAmount = 0.2f;
                break;

        }
    }



    public void RestartGameProgress()
    {
        gameProgress = 0f;
        EventManager.Game.OnPerkPicked.Invoke();
    }

    private void TaskEnded(DialogEvents ev)
    {
        eventFlag = false;
    }
    private void TaskEnded()
    {
        eventFlag = false;
    }


    private void OnEnable()
    {
        EventManager.Game.OnTaskDialogCompleted += TaskEnded;
        EventManager.Game.OnTaskDialogFailed += TaskEnded;
        EventManager.Game.OnGameStopped += TaskEnded;
    }

    private void OnDisable()
    {
        EventManager.Game.OnTaskDialogCompleted -= TaskEnded;
        EventManager.Game.OnTaskDialogFailed -= TaskEnded;
        EventManager.Game.OnGameStopped -= TaskEnded;
    }

}
