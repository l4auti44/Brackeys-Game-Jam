using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

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
    private float gameProgressSpeed;
    public GameObject gameProgressSlider;
    public float[] gameProgressSpeedPositionLVS = new float[5];

    // --- Game Objects ---
    [Header("Game Objects")]
    public GameObject ship;
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

    // --- Overload Settings ---
    [Header("Overload Settings")]
    public bool isOverload = false;  // Tracks overload state
    public float overloadDuration = 5f;  // Duration of the overload state

    // --- Game Over Timer ---
    [Header("Game Over Timer")]
    public float timerForGameOver = 10f;
    private float timer = 10f;

    // --- Sound Modules ---
    [Header("Sound Modules")]
    private SoundButton shieldModule;
    private SoundButton arrowModule;
    private List<SoundButton> engineModules = new List<SoundButton>();
    private List<SoundButton> radarModules = new List<SoundButton>();

    // --- Repair System ---
    private DestroyAndRepearSys repairSys;

    private TextMeshPro totalRateEnergyDecrease;
    //temporal flags
    private bool eventFlag = false;


    private Image sourceImageEngineModule;
    [SerializeField] private Sprite[] EngineModuleImages = new Sprite[6];

    private Image sourceImageRadarModule;
    [SerializeField] private Sprite[] RadarModuleImages = new Sprite[5];

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
        //Get ship speed
        ship.GetComponent<ShipMovement>().speed = shipSpeed;

        //store values for all energy decrease mods
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


        //set all energy decrease mods at 0 after storing their values
        radarModEnergyDecreaseLV1 = 0;
        radarModEnergyDecreaseLV2 = 0;
        radarModEnergyDecreaseLV3 = 0;
        radarModEnergyDecreaseLV4 = 0;

        shieldModEnergyDecrease = 0;

        positionModEnergyDecreaseLV1 = 0;
        positionModEnergyDecreaseLV2 = 0;
        positionModEnergyDecreaseLV3 = 0;
        positionModEnergyDecreaseLV4 = 0;
        positionModEnergyDecreaseLV5 = 0;

        arrowModEnergyDecrease = 0;


        //Start game with energy equals to total
        energy = maxEnergy;
        
        //start game with engine module speed LV1
        shipSpeed = speedLVS[0];

        //start game with position LV1
        positionLV = 1;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

        //Start level with radar LV1
        radarLV = 1;
        ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLVS[0];
        //radarUI.waveInterval = radarWaveIntervalLVS[0];
        radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;

        //Start level with game progress speed LV1
        gameProgressSpeed = gameProgressSpeedPositionLVS[0];

        //Start game with no arrow spawn
        asteroidSpawner.spawnArrow = false;

        //Start game with parallax effect LV1
        parallax.IncreaseSpeed(speedParallaxLVS[0]);

        //Reference to the main camera
        mainCamera = Camera.main;
        targetZoom = mainCamera.orthographicSize; // Set initial target to current zoom level


        shieldModule = GameObject.Find("ShieldModule").GetComponent<SoundButton>();
        arrowModule = GameObject.Find("ArrowModule").GetComponent<SoundButton>();
        var engineButtons = GameObject.Find("EngineButtons");
        engineModules.Add(engineButtons.transform.GetChild(0).GetComponent<SoundButton>());
        engineModules.Add(engineButtons.transform.GetChild(1).GetComponent<SoundButton>());
        var radarButtons = GameObject.Find("RadarButtons");
        radarModules.Add(radarButtons.transform.GetChild(0).GetComponent<SoundButton>());
        radarModules.Add(radarButtons.transform.GetChild(1).GetComponent<SoundButton>());

        repairSys = this.GetComponentInChildren<DestroyAndRepearSys>();

        totalRateEnergyDecrease = GameObject.Find("Total Rate").GetComponentInChildren<TextMeshPro>();

        sourceImageEngineModule = GameObject.Find("EngineModule").transform.GetComponentInChildren<Image>();
        sourceImageRadarModule = GameObject.Find("RadarModule").transform.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SceneController.isGameStopped)
        {
            if (gameProgress > 15f && eventFlag == false)
            {
                DialogSystem.DialogEvents[] dialogEvents = new DialogSystem.DialogEvents[]
                {
                DialogSystem.DialogEvents.KeepEngineAt,
                DialogSystem.DialogEvents.KeepRadarAt,
                DialogSystem.DialogEvents.NoGetHit
                };

                EventManager.Game.OnDialog.Invoke(dialogEvents[Random.Range(0, dialogEvents.Length)]);
                eventFlag = true;
            }

            if (!SceneController.isGamePaused)
            {
                //Increae the game progress over time
                gameProgress += gameProgressSpeed * 0.01f;

                //Update the slider filler
                gameProgressSlider.GetComponent<Slider>().value = gameProgress * 0.01f;
                if (gameProgress * 0.01f >= 1f)
                {
                    EventManager.Game.OnWin.Invoke();
                    
                    return;
                }

                //Determination of energy decrease speed
                energyDecreaseSpeed =

                (radarModEnergyDecreaseLV1 +
                radarModEnergyDecreaseLV2 +
                radarModEnergyDecreaseLV3 +
                radarModEnergyDecreaseLV4 +

                shieldModEnergyDecrease +

                positionModEnergyDecreaseLV1 +
                positionModEnergyDecreaseLV2 +
                positionModEnergyDecreaseLV3 +
                positionModEnergyDecreaseLV4 +
                positionModEnergyDecreaseLV5 +

                arrowModEnergyDecrease_original
                )
                * 0.1f
                ;

                //Decrease energy over time
                energy -= energyDecreaseSpeed * 0.01f;

                //Update the energy value of the slider
                energySlider.value = energy * 0.01f;

                //Deactivate all modules if energy is 0
                if (energy < 0)
                {
                    // Gradually decrease ship speed to zero during the game-over timer
                    shipSpeed = Mathf.Max(0, shipSpeed - speedDecreaseRateWhenGameOver * Time.deltaTime);

                    //Disable Input Windows


                    // Disable radar level and play warning sound
                    radarLV = 0;
                    SoundManager.PlaySound(SoundManager.Sound.EnergyAtZeroWarning);

                    timerForGameOver -= Time.deltaTime;
                    if (timerForGameOver <= 0f)
                    {

                        timerForGameOver = 99999;
                        EventManager.Game.OnDie.Invoke(this);
                    }
                }
                else
                {
                    timerForGameOver = 10f;
                }


                //Clamp energy
                energy = Mathf.Max(energy, 0);
                energy = Mathf.Min(energy, maxEnergy);

                //Move the camera when needed
                // Smoothly adjust the orthographic size towards the target zoom value
                mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, targetZoom, cameraZoomOutSpeed * Time.deltaTime);
            }


            totalRateEnergyDecrease.text = energyDecreaseSpeed.ToString();
        }
       
    }


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
        energyMax.value += amount * 0.01f;
    }
    public void AccelerateShip()
    {
        for (int i = 0; i < speedLVS.Length; i++)
        { 
            if (shipSpeed == speedLVS[i])
            {
                if (i != speedLVS.Length - 1)
                {
                    shipSpeed = speedLVS[i + 1];
                    gameProgressSpeed = gameProgressSpeedPositionLVS[i + 1];

                }

            }
        }
        
    }

    public void DecelerateShip()
    {
        for (int i = 0; i < speedLVS.Length; i++)
        {
            if (shipSpeed == speedLVS[i])
            {
                if (i != 0)
                {
                    shipSpeed = speedLVS[i - 1];
                    gameProgressSpeed = gameProgressSpeedPositionLVS[i - 1];

                }

            }
        }
        
    }


    public void IncreaseRadar()
    {
        if (!radarModules[0].isBroken && radarLV != 4 && !SceneController.isGameStopped)
        {
            radarLV++;
            ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLVS[radarLV - 1];

            //Decrease energy
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
            ChangeRadarSprite();
            EventManager.Game.OnRadarChange.Invoke(radarLV);


        }

    }

    public void DecreaseRadar()
    {
        if (!radarModules[1].isBroken && radarLV != 1 && !SceneController.isGameStopped)
        {
            radarLV--;
            ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLVS[radarLV - 1];

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

            ChangeRadarSprite();
            EventManager.Game.OnRadarChange.Invoke(radarLV);
        }
        
    }


    public void ActivateShield()
    {
        if (!isShieldCooldown && !SceneController.isGameStopped)
        {
            StartCoroutine(ShieldRoutine());
        }
    }

    private IEnumerator ShieldRoutine()
    {
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
        if (!SceneController.isGameStopped)
        {
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
           
    }

    #region EngineRelatedRegion
    public void IncreasePositionShip ()
    {
        if (!SceneController.isGameStopped)
        {
            if (!engineModules[0].isBroken && positionLV != 5)
            {

                positionLV++;
                ship.GetComponent<ShipMovement>().MoveToLevel(positionLVS[positionLV - 1]);
                //positionModuleSpriteLVS[positionLV - 1].color = Color.green;
                // Increase parallax
                parallax.IncreaseSpeed(speedParallaxLVS[positionLV - 1]);
                //Increase speed when transitioning to the next position
                ship.GetComponent<ShipMovement>().moveDuration = durationToPositions[positionLV - 1];
                //Zoom out camera
                ZoomOut(cameraPosLVS[positionLV - 1]);

            }

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


            //Moved events on the button here
            AccelerateShip();
            ToggleAsteroidSpeed();
            ChangeEngineSprite();
            EventManager.Game.OnEngineChange.Invoke((int)positionLV);
        }
    }

    public void DecreasePositionShip()
    {
        if (!SceneController.isGameStopped)
        {
            if (!engineModules[1].isBroken && positionLV != 1)
            {
                //positionModuleSpriteLVS[positionLV - 1].color = Color.white;
                positionLV--;
                ship.GetComponent<ShipMovement>().MoveToLevel(positionLVS[positionLV - 1]);
                //Decrease parallax speed
                parallax.IncreaseSpeed(speedParallaxLVS[positionLV - 1]);
                //Increase speed when transitioning to the next position
                ship.GetComponent<ShipMovement>().moveDuration = durationToPositions[positionLV - 1];
                //Zoom out camera
                ZoomOut(cameraPosLVS[positionLV - 1]);

            }
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


            //Moved events on the button here
            DecelerateShip();
            ToggleAsteroidSpeed();
            ChangeEngineSprite();
            EventManager.Game.OnEngineChange.Invoke((int)positionLV);
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
                sourceImageEngineModule.sprite = EngineModuleImages[5];
                break;
            case 4:
                sourceImageEngineModule.sprite = EngineModuleImages[4];
                break;
            case 3:
                sourceImageEngineModule.sprite = EngineModuleImages[3];
                break;
            case 2:
                sourceImageEngineModule.sprite = EngineModuleImages[2];
                break;
            case 1:
                sourceImageEngineModule.sprite = EngineModuleImages[1];
                break;

        }
    }

    public void ChangeRadarSprite()
    {

        switch (radarLV)
        {
            case 4:
                sourceImageRadarModule.sprite = RadarModuleImages[4];
                break;
            case 3:
                sourceImageRadarModule.sprite = RadarModuleImages[3];
                break;
            case 2:
                sourceImageRadarModule.sprite = RadarModuleImages[2];
                break;
            case 1:
                sourceImageRadarModule.sprite = RadarModuleImages[1];
                break;

        }
    }

    public void RestartGameProgress()
    {
        gameProgress = 0f;
    }

}
