using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public float gameProgress, gameProgressSpeed;
    public GameObject gameProgressSlider;
    public GameObject ship, asteroidSpawner, energySpawner, shield, radar, radarUI, parallax;
    public Camera mainCamera;
    //public GameObject engineModuleSpriteLV1, engineModuleSpriteLV2, engineModuleSpriteLV3;
    public GameObject radarModuleSpriteLV1, radarModuleSpriteLV2, radarModuleSpriteLV3;
    public GameObject positionModuleSpriteLV1, positionModuleSpriteLV2, positionModuleSpriteLV3, positionModuleSpriteLV4, positionModuleSpriteLV5;
    public GameObject arrowUI, shieldUI;
    public float shipSpeed, radarLV, positionLV;
    public float speedLV0, speedLV1, speedLV2, speedLV3, speedLV4, speedLV5; //Speed variations for each module level
    public float minAsteroidSpeedLV1, minAsteroidSpeedLV2, minAsteroidSpeedLV3, minAsteroidSpeedLV4, minAsteroidSpeedLV5; //Speed variations for each module level
    public float maxAsteroidSpeedLV1, maxAsteroidSpeedLV2, maxAsteroidSpeedLV3, maxAsteroidSpeedLV4, maxAsteroidSpeedLV5; //Speed variations for each module level
    public float minRotationSpeedLV1, minRotationSpeedLV2, minRotationSpeedLV3, minRotationSpeedLV4, minRotationSpeedLV5; //Speed variations for each module level
    public float maxRotationSpeedLV1, maxRotationSpeedLV2, maxRotationSpeedLV3, maxRotationSpeedLV4, maxRotationSpeedLV5; //Speed variations for each module level
    public float radarWaveIntervalLV3, radarWaveIntervalLV2, radarWaveIntervalLV1, radarWaveIntervalLV0;
    public Transform positionLV1, positionLV2, positionLV3, positionLV4, positionLV5; //Position variations for each module level
    public float durationToPosition5, durationToPosition4, durationToPosition3, durationToPosition2, durationToPosition1; //Speed variations for each module level when moving to another position
    public float speedParallaxLV0, speedParallaxLV1, speedParallaxLV2, speedParallaxLV3, speedParallaxLV4, speedParallaxLV5; //Speed variations for each module level
    public float cameraPosLV0, cameraPosLV1, cameraPosLV2, cameraPosLV3, cameraPosLV4, cameraPosLV5;
    public float cameraZoomOutSpeed = 2;
    private float targetZoom;        // Target orthographic size

    //public GameObject lightPlayerLV1, lightPlayerLV2; //Radar light variations for each module level

    public float energy = 100f; // Amount of energy at any time
    public float energyDecreaseSpeed; //Speed at which the energy is depleted
    public float maxEnergy = 100f; // Amount of total energy

    //public float engineModEnergyDecreaseLV1, engineModEnergyDecreaseLV2, engineModEnergyDecreaseLV3;
    //private float engineModEnergyDecreaseLV1_original, engineMoEnergyDecreaseLV2_original, engineModEnergyDecreaseLV3_original;

    public float radarModEnergyDecreaseLV1, radarModEnergyDecreaseLV2, radarModEnergyDecreaseLV3;
    private float radarModEnergyDecreaseLV1_original, radarModEnergyDecreaseLV2_original, radarModEnergyDecreaseLV3_original;

    public float shieldModEnergyDecrease;
    private float shieldModEnergyDecrease_original;

    public float positionModEnergyDecreaseLV1, positionModEnergyDecreaseLV2, positionModEnergyDecreaseLV3, positionModEnergyDecreaseLV4, positionModEnergyDecreaseLV5;
    private float positionModEnergyDecreaseLV1_original, positionModEnergyDecreaseLV2_original, positionModEnergyDecreaseLV3_original, positionModEnergyDecreaseLV4_original, positionModEnergyDecreaseLV5_original;

    public float arrowModEnergyDecrease;
    private float arrowModEnergyDecrease_original;

    public GameObject energySlider;

    public bool isOverload = false;  // Tracks overload state
    public float overloadDuration = 5f;  // Duration of the overload state

    public GameObject shieldObject;      // The shield GameObject
    public float shieldDuration = 5f;    // How long the shield stays active
    public float shieldCooldown = 10f;   // Cooldown before shield can be reactivated
    public bool isShieldActive = false;
    public bool isShieldCooldown = false;

    
    private float timerForGameOver = 10f;

    private float timer = 10f;

    // Start is called before the first frame update
    void Start()
    {
        //Get ship speed
        ship.GetComponent<ShipMovement>().speed = shipSpeed;

        //store values for all energy decrease mods
        radarModEnergyDecreaseLV1_original = radarModEnergyDecreaseLV1;
        radarModEnergyDecreaseLV2_original = radarModEnergyDecreaseLV2;
        radarModEnergyDecreaseLV3_original = radarModEnergyDecreaseLV3;

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
        shipSpeed = speedLV1;

        //start game with position LV1
        positionLV = 1;
        positionModuleSpriteLV1.GetComponent<Image>().color = Color.green;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

        //Start level with radar LV1
        radarLV = 1;
        ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;
        radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV1;
        radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
        radarModuleSpriteLV1.GetComponent<Image>().color = Color.green;

        //Start game with no arrow spawn
        asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow = false;

        //Start game with parallax effect LV1
        parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV1);

        //Reference to the main camera
        mainCamera = Camera.main;
        targetZoom = mainCamera.orthographicSize; // Set initial target to current zoom level

    }

    // Update is called once per frame
    void Update()
    {
        if (!SceneController.isGamePaused)
        {
            //Increae the game progress over time
            gameProgress += gameProgressSpeed * 0.01f;

            //Update the slider filler
            gameProgressSlider.GetComponent<Slider>().value = gameProgress * 0.01f;
            if (gameProgress * 0.01f >= 1f)   
            {
                EventManager.Game.OnWin.Invoke(this);
            }


            //Determination of energy decrease speed
            energyDecreaseSpeed =

            (radarModEnergyDecreaseLV1 +
            radarModEnergyDecreaseLV2 +
            radarModEnergyDecreaseLV3 +

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
            energySlider.GetComponent<Slider>().value = energy * 0.01f;

            //Deactivate all modules if energy is 0
            if (energy < 0)
            {
                shipSpeed = 0;
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
        

    }


    public void DecreaseEnergy(float energyToDecrease)
    {
        energy -= energyToDecrease;
    }

    public void IncreaseEnergy(float energyToIncrease)
    {
        energy += energyToIncrease;
    }

    public void AccelerateShip()
    {
        //Increase speed movement
        if (shipSpeed == speedLV4)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV5;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = engineModEnergyDecreaseLV3_original;
            gameProgressSpeed += 1;
        }

        else if (shipSpeed == speedLV3)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV4;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = engineModEnergyDecreaseLV3_original;
            gameProgressSpeed += 1;
        }

        else if (shipSpeed == speedLV2)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV3;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = engineModEnergyDecreaseLV3_original;
            gameProgressSpeed += 1;
        }

        else if (shipSpeed == speedLV1)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            //engineModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV2 = engineMoEnergyDecreaseLV2_original;
            gameProgressSpeed += 1;
        }
    }

    public void DecelerateShip()
    {
        //Decrease speed movement
        if (shipSpeed == speedLV2)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV1;
            //engineModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 0;
            //engineModEnergyDecreaseLV2 = 0;
            //engineModEnergyDecreaseLV1 = 0;
            gameProgressSpeed -= 1;
        }

        else if (shipSpeed == speedLV3)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            //engineModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 0;
            //engineModEnergyDecreaseLV2 = 0;
            //engineModEnergyDecreaseLV1 = 0;
            gameProgressSpeed -= 1;
        }

        else if (shipSpeed == speedLV4)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV3;
            //engineModuleSpriteLV2.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV2 = 0;
            //engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;
            gameProgressSpeed -= 1;
        }

        else if (shipSpeed == speedLV5)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV4;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 0;
            //engineModEnergyDecreaseLV2 = engineMoEnergyDecreaseLV2_original;
            gameProgressSpeed -= 1;
        }

    }


    public void IncreaseRadar()
    {
        //Increase radar LV
        if (radarLV == 2)
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

        else if (radarLV == 0)
        {
            //Update the radar LV and sprite
            radarLV = 1;
            radarModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            //Activate radar LV1
            ship.GetComponent<RadarController>().isSpawning = true;
            radarUI.GetComponent<RadarControllerUI>().isSpawning = true;
            ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;
            radarUI.GetComponent<RadarControllerUI>().waveInterval = radarWaveIntervalLV1;

            //Decrease energy
            radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
        }
    }

    public void DecreaseRadar()
    {
        //Decrease radar LV
        if (radarLV == 1)
        {
            //Update the radar LV and sprite
            radarLV = 0;
            radarModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //Activate radar LV0
            ship.GetComponent<RadarController>().isSpawning = false;
            radarUI.GetComponent<RadarControllerUI>().isSpawning = false;

            //Decrease energy
            radarModEnergyDecreaseLV1 = 0;

        }

        else if (radarLV == 2)
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
            radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV1_original;
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


    public void IncreasePositionShip ()
    {
        //Increase position
        if (positionLV == 4)
        {
            //Update the position LV and sprite
            positionLV = 5;
            positionModuleSpriteLV5.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV5);

            positionModEnergyDecreaseLV5 = positionModEnergyDecreaseLV5_original;

            //Increase parallax
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV5);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition5;

            //Zoom out camera
            ZoomOut(cameraPosLV5);
        }

        else if (positionLV == 3)
        {
            //Update the position LV and sprite
            positionLV = 4;
            positionModuleSpriteLV4.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV4);

            positionModEnergyDecreaseLV4 = positionModEnergyDecreaseLV4_original;

            //Increase parallax
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV4);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition4;

            //Zoom out camera
            ZoomOut(cameraPosLV4);
        }

        else if (positionLV == 2)
        {
            //Update the position LV and sprite
            positionLV = 3;
            positionModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV3);

            positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV3_original;

            //Increase parallax
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV3);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition3;

            //Zoom out camera
            ZoomOut(cameraPosLV3);
        }

        else if (positionLV == 1)
        {
            positionLV = 2;
            positionModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV2);

            positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV2_original;

            //Increase parallax
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV2);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition2;

            //Zoom out camera
            ZoomOut(cameraPosLV2);
        }

    }

    public void DecreasePositionShip()
    {
        //Decrease position
        if (positionLV == 5)
        {
            //Update the position LV and sprite
            positionLV = 4;
            positionModuleSpriteLV5.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV4);

            positionModEnergyDecreaseLV5 = 0;
            positionModEnergyDecreaseLV4 = positionModEnergyDecreaseLV4_original;

            //Decrease parallax speed
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV4);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition4;

            //Zoom out camera
            ZoomOut(cameraPosLV4);
        }

        else if (positionLV == 4)
        {
            //Update the position LV and sprite
            positionLV = 3;
            positionModuleSpriteLV4.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV3);

            positionModEnergyDecreaseLV4 = 0;
            positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV3_original;

            //Decrease parallax speed
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV3);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition3;

            //Zoom out camera
            ZoomOut(cameraPosLV3);
        }

        else if (positionLV == 3)
        {
            //Update the position LV and sprite
            positionLV = 2;
            positionModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV2);

            positionModEnergyDecreaseLV3 = 0;
            positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV2_original;

            //Decrease parallax speed
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV2);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition2;

            //Zoom out camera
            ZoomOut(cameraPosLV2);
        }

        else if (positionLV == 2)
        {
            positionLV = 1;
            positionModuleSpriteLV2.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV1);

            positionModEnergyDecreaseLV2 = 0;
            positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

            //Decrease parallax spee1
            parallax.GetComponent<ParallarEffect>().IncreaseSpeed(speedParallaxLV1);

            //Increase speed when transitioning to the next position
            ship.GetComponent<ShipMovement>().moveDuration = durationToPosition1;

            //Zoom out camera
            ZoomOut(cameraPosLV1);
        }
    }


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

    public void TriggerOverload()
    {
      
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
