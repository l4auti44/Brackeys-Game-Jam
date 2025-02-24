using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    // --- Game Progression ---
    [Header("Game Progression")]
    public float gameProgress;
    private float gameProgressSpeed;
    public GameObject gameProgressSlider;
    public float[] gameProgressSpeedPositionLVS = new float[5];

    // --- Game Objects ---
    [Header("Game Objects")]
    public GameObject ship;
    public GameObject asteroidSpawner, energySpawner, shield, radar, radarUI;
    public ParallarEffect parallax;
    public Camera mainCamera;

    // --- Radar & Position Modules ---
    [Header("Radar & Position Modules")]
    public GameObject radarModuleSpriteLV1;
    public GameObject radarModuleSpriteLV2, radarModuleSpriteLV3, radarModuleSpriteLV4;
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
    public float minAsteroidSpeedLV1, minAsteroidSpeedLV2, minAsteroidSpeedLV3, minAsteroidSpeedLV4, minAsteroidSpeedLV5;
    public float maxAsteroidSpeedLV1, maxAsteroidSpeedLV2, maxAsteroidSpeedLV3, maxAsteroidSpeedLV4, maxAsteroidSpeedLV5;
    public float minRotationSpeedLV1, minRotationSpeedLV2, minRotationSpeedLV3, minRotationSpeedLV4, minRotationSpeedLV5;
    public float maxRotationSpeedLV1, maxRotationSpeedLV2, maxRotationSpeedLV3, maxRotationSpeedLV4, maxRotationSpeedLV5;
    public float speedDecreaseRateWhenGameOver;

    // --- Radar Wave Intervals ---
    [Header("Radar Wave Intervals")]
    public float radarWaveIntervalLV3;
    public float radarWaveIntervalLV2, radarWaveIntervalLV1, radarWaveIntervalLV4;

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
    private bool winCondition = false;

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
        positionModuleSpriteLVS[0].color = Color.green;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

        //Start level with radar LV1
        radarLV = 1;
        ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;
        radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV1;
        radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
        radarModuleSpriteLV1.GetComponent<Image>().color = Color.green;

        //Start level with game progress speed LV1
        gameProgressSpeed = gameProgressSpeedPositionLVS[0];

        //Start game with no arrow spawn
        asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameProgress > 15f && eventFlag == false)
        {
            DialogSystem.DialogEvents[] dialogEvents = new DialogSystem.DialogEvents[]
            {
                DialogSystem.DialogEvents.KeepEngineAt,
                DialogSystem.DialogEvents.KeepRadarAt,
                DialogSystem.DialogEvents.NoGetHit
            };

            EventManager.Game.OnDialog.Invoke(dialogEvents[Random.Range(0,dialogEvents.Length)]);
            eventFlag = true;
        }

        if (!SceneController.isGamePaused && !winCondition)
        {
            //Increae the game progress over time
            gameProgress += gameProgressSpeed * 0.01f;

            //Update the slider filler
            gameProgressSlider.GetComponent<Slider>().value = gameProgress * 0.01f;
            if (gameProgress * 0.01f >= 1f)   
            {
                EventManager.Game.OnWin.Invoke(this);
                winCondition = true;
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
        if (!radarModules[0].isBroken)
        {
            //Increase radar LV
            if (radarLV == 3)
            {
                //Update the radar LV and sprite
                radarLV = 4;
                radarModuleSpriteLV4.GetComponent<Image>().color = Color.green;

                //Activate radar LV4
                ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV4;
                radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV4;

                //Decrease energy
                radarModEnergyDecreaseLV4 = radarModEnergyDecreaseLV4_original;

            }

            else if (radarLV == 2)
            {
                //Update the radar LV and sprite
                radarLV = 3;
                radarModuleSpriteLV3.GetComponent<Image>().color = Color.green;

                //Activate radar LV3
                ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV3;
                radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV3;

                //Decrease energy
                radarModEnergyDecreaseLV3 = radarModEnergyDecreaseLV3_original;

            }

            else if (radarLV == 1)
            {
                //Update the radar LV and sprite
                radarLV = 2;
                radarModuleSpriteLV2.GetComponent<Image>().color = Color.green;

                //Activate radar LV2
                ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV2;
                radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV2;

                //Decrease energy
                radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV2_original;
            }

            //else if (radarLV == 0)
            //{
            //    //Update the radar LV and sprite
            //    radarLV = 1;
            //    radarModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            //    //Activate radar LV1
            //    ship.GetComponent<RadarController>().isSpawning = true;
            //    radarUI.GetComponent<RadarControllerUI>().isSpawning = true;
            //    ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;
            //    radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV1;

            //    //Decrease energy
            //    radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
            //}

            EventManager.Game.OnRadarChange.Invoke((int)radarLV);
        }


    }

    public void DecreaseRadar()
    {
        if (!radarModules[1].isBroken)
        {

            //if (radarLV == 1)
            //{
            //    //Update the radar LV and sprite
            //    radarLV = 0;
            //    radarModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //    //Activate radar LV0
            //    ship.GetComponent<RadarController>().isSpawning = false;
            //    radarUI.GetComponent<RadarControllerUI>().isSpawning = false;

            //    //Decrease energy
            //    radarModEnergyDecreaseLV1 = 0;

            //}

            //Decrease radar LV
            if (radarLV == 2)
            {
                //Update the radar LV and sprite
                radarLV = 1;
                radarModuleSpriteLV2.GetComponent<Image>().color = Color.white;

                //Activate radar LV1
                ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;
                radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV1;

                //Decrease energy
                radarModEnergyDecreaseLV2 = 0;
                radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
            }

            else if (radarLV == 3)
            {
                //Update the radar LV and sprite
                radarLV = 2;
                radarModuleSpriteLV3.GetComponent<Image>().color = Color.white;

                //Activate radar LV2
                ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV2;
                radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV2;

                //Decrease energy
                radarModEnergyDecreaseLV3 = 0;
                radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV2_original;
            }

            else if (radarLV == 4)
            {
                //Update the radar LV and sprite
                radarLV = 3;
                radarModuleSpriteLV4.GetComponent<Image>().color = Color.white;

                //Activate radar LV3
                ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV3;
                radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV3;

                //Decrease energy
                radarModEnergyDecreaseLV4 = 0;
                radarModEnergyDecreaseLV3 = radarModEnergyDecreaseLV3_original;
            }

            EventManager.Game.OnRadarChange.Invoke((int)radarLV);
        }
        
    }


    public void ActivateShield()
    {
        if (!isShieldCooldown)
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
        shieldUI.GetComponent<Image>().color = Color.green;

        // Wait for the shield duration
        yield return new WaitForSeconds(shieldDuration);

        // Deactivate the shield
        shieldObject.GetComponent<CapsuleCollider2D>().enabled = false;
        shieldObject.GetComponent<SpriteRenderer>().enabled = false;

        isShieldActive = false;
        shieldModEnergyDecrease = 0;
        shieldUI.GetComponent<Image>().color = Color.red;

        // Start the cooldown
        yield return new WaitForSeconds(shieldCooldown);

        // Allow shield to be reactivated after cooldown
        isShieldCooldown = false;
        shieldUI.GetComponent<Image>().color = Color.white;
    }

    public void ToggleArrows()
    {

            //if the number of tumes that the key was pressed is an odd number, the shield will be activated.
            //the count starts with 0, so the first hit will be a 1 => odd number => activate shield. Next press is 2 => even number => inactivate
            asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow = !asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow;
            energySpawner.GetComponent<EnergySpawner>().spawnArrow = !energySpawner.GetComponent<EnergySpawner>().spawnArrow;

            if (asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow)
            {
                arrowModEnergyDecrease = arrowModEnergyDecrease_original;
                arrowUI.GetComponent<Image>().color = Color.green;
            }

            else
            {
                arrowModEnergyDecrease = 0;
                arrowUI.GetComponent<Image>().color = Color.white;
            }
        
        
    }

    #region EngineRelatedRegion
    public void IncreasePositionShip ()
    {
        if (!engineModules[0].isBroken && positionLV != 5)
        {

            positionLV++;
            ship.GetComponent<ShipMovement>().MoveToLevel(positionLVS[positionLV - 1]);
            positionModuleSpriteLVS[positionLV - 1].color = Color.green;
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
        if (positionLV != 5)
        {
            ToggleAsteroidSpeed();
            EventManager.Game.OnEngineChange.Invoke((int)positionLV);
        }
            
        

    }

    public void DecreasePositionShip()
    {
        if (!engineModules[1].isBroken && positionLV != 1)
        {
            positionModuleSpriteLVS[positionLV - 1].color = Color.white;
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
        if (positionLV != 1)
        {
            ToggleAsteroidSpeed();
            EventManager.Game.OnEngineChange.Invoke((int)positionLV);
        }

        
        
    }

    #endregion
    public void ToggleAsteroidSpeed()
    {

        //Increase speed movement
        if (positionLV == 5)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV5;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = minAsteroidSpeedLV5;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV5;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV5;

            //Change speed of all spawned asteroids in the scene to the average number of the range in that level
            ChangeAsteroidSpeeds(maxAsteroidSpeedLV5 - minAsteroidSpeedLV5);
        }

        else if (positionLV == 4)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV4;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV4;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV4;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV4;

            //Change speed of all spawned asteroids in the scene to the average number of the range in that level
            ChangeAsteroidSpeeds(maxAsteroidSpeedLV5 - minAsteroidSpeedLV4);
        }

        else if (positionLV == 3)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV3;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV3;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV3;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV3;

            //Change speed of all spawned asteroids in the scene to the average number of the range in that level
            ChangeAsteroidSpeeds(maxAsteroidSpeedLV5 - minAsteroidSpeedLV3);
        }

        else if (positionLV == 2)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV2;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV2;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV2;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV2;

            //Change speed of all spawned asteroids in the scene to the average number of the range in that level
            ChangeAsteroidSpeeds(maxAsteroidSpeedLV5 - minAsteroidSpeedLV2);
        }

        else if (positionLV == 1)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV1;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV1;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV1;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV1;

            //Change speed of all spawned asteroids in the scene to the average number of the range in that level
            ChangeAsteroidSpeeds(maxAsteroidSpeedLV5 - minAsteroidSpeedLV1);
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

}
