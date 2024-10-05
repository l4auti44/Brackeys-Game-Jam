using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public float gameProgress, gameProgressSpeed;
    public GameObject gameProgressSlider;
    public GameObject ship, asteroidSpawner, shield, radar, radarUI;
    //public GameObject engineModuleSpriteLV1, engineModuleSpriteLV2, engineModuleSpriteLV3;
    public GameObject radarModuleSpriteLV1, radarModuleSpriteLV2, radarModuleSpriteLV3;
    public GameObject positionModuleSpriteLV1, positionModuleSpriteLV2, positionModuleSpriteLV3, positionModuleSpriteLV4, positionModuleSpriteLV5;
    public float shipSpeed, radarLV, positionLV;
    public float speedLV0, speedLV1, speedLV2, speedLV3, speedLV4, speedLV5; //Speed variations for each module level
    public float minAsteroidSpeedLV1, minAsteroidSpeedLV2, minAsteroidSpeedLV3, minAsteroidSpeedLV4, minAsteroidSpeedLV5; //Speed variations for each module level
    public float maxAsteroidSpeedLV1, maxAsteroidSpeedLV2, maxAsteroidSpeedLV3, maxAsteroidSpeedLV4, maxAsteroidSpeedLV5; //Speed variations for each module level
    public float minRotationSpeedLV1, minRotationSpeedLV2, minRotationSpeedLV3, minRotationSpeedLV4, minRotationSpeedLV5; //Speed variations for each module level
    public float maxRotationSpeedLV1, maxRotationSpeedLV2, maxRotationSpeedLV3, maxRotationSpeedLV4, maxRotationSpeedLV5; //Speed variations for each module level
    public float radarWaveIntervalLV3, radarWaveIntervalLV2, radarWaveIntervalLV1, radarWaveIntervalLV0;
    public Transform positionLV1, positionLV2, positionLV3, positionLV4, positionLV5; //Position variations for each module level
    public GameObject lightPlayerLV1, lightPlayerLV2; //Radar light variations for each module level

    public float energy = 100f; // Amount of energy at any time
    public float energyDecreaseSpeed; //Speed at which the energy is depleted
    public float totalEnergy = 100f; // Amount of total energy

    //public float engineModEnergyDecreaseLV1, engineModEnergyDecreaseLV2, engineModEnergyDecreaseLV3;
    //private float engineModEnergyDecreaseLV1_original, engineMoEnergyDecreaseLV2_original, engineModEnergyDecreaseLV3_original;

    public float radarModEnergyDecreaseLV1, radarModEnergyDecreaseLV2, radarModEnergyDecreaseLV3;
    private float radarModEnergyDecreaseLV1_original, radarModEnergyDecreaseLV2_original, radarModEnergyDecreaseLV3_original;

    public float shieldModEnergyDecrease;
    private float shieldModEnergyDecrease_original;
    private float numberOfKeyShieldPressed = 0;

    public float positionModEnergyDecreaseLV1, positionModEnergyDecreaseLV2, positionModEnergyDecreaseLV3, positionModEnergyDecreaseLV4, positionModEnergyDecreaseLV5;
    private float positionModEnergyDecreaseLV1_original, positionModEnergyDecreaseLV2_original, positionModEnergyDecreaseLV3_original, positionModEnergyDecreaseLV4_original, positionModEnergyDecreaseLV5_original;

    public GameObject energySlider;

    public bool isOverload = false;  // Tracks overload state
    public float overloadDuration = 5f;  // Duration of the overload state
    private float numberOfKeyOverloadPressed = 0;


    // Start is called before the first frame update
    void Start()
    {
        //Get ship speed
        ship.GetComponent<ShipMovement>().speed = shipSpeed;

        //store values for all energy decrease mods
        //engineModEnergyDecreaseLV1_original = engineModEnergyDecreaseLV1;
        //engineMoEnergyDecreaseLV2_original = engineModEnergyDecreaseLV2;
        //engineModEnergyDecreaseLV3_original = engineModEnergyDecreaseLV3;

        radarModEnergyDecreaseLV1_original = radarModEnergyDecreaseLV1;
        radarModEnergyDecreaseLV2_original = radarModEnergyDecreaseLV2;
        radarModEnergyDecreaseLV3_original = radarModEnergyDecreaseLV3;

        shieldModEnergyDecrease_original = shieldModEnergyDecrease;

        positionModEnergyDecreaseLV1_original = positionModEnergyDecreaseLV1;
        positionModEnergyDecreaseLV2_original = positionModEnergyDecreaseLV2;
        positionModEnergyDecreaseLV3_original = positionModEnergyDecreaseLV3;
        positionModEnergyDecreaseLV4_original = positionModEnergyDecreaseLV4;
        positionModEnergyDecreaseLV5_original = positionModEnergyDecreaseLV5;


        //set all energy decrease mods at 0 after storing their values
        //engineModEnergyDecreaseLV1 = 0;
        //engineModEnergyDecreaseLV2 = 0;
        //engineModEnergyDecreaseLV3 = 0;

        radarModEnergyDecreaseLV1 = 0;
        radarModEnergyDecreaseLV2 = 0;
        radarModEnergyDecreaseLV3 = 0;

        shieldModEnergyDecrease = 0;

        positionModEnergyDecreaseLV1 = 0;
        positionModEnergyDecreaseLV2 = 0;
        positionModEnergyDecreaseLV3 = 0;
        positionModEnergyDecreaseLV4 = 0;
        positionModEnergyDecreaseLV5 = 0;


        //Start game with energy equals to total
        energy = totalEnergy;
        
        //start game with engine module speed LV1
        shipSpeed = speedLV1;
        //engineModuleSpriteLV1.GetComponent<Image>().color = Color.green;
        //engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;

        //start game with light module LV1
        

        //start game with position LV1
        positionLV = 1;
        positionModuleSpriteLV1.GetComponent<Image>().color = Color.green;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

        //Start level with radar LV1
        radarLV = 1;
        ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;
        radarModEnergyDecreaseLV1 = radarModEnergyDecreaseLV1_original;
        radarModuleSpriteLV1.GetComponent<Image>().color = Color.green;

    }

    // Update is called once per frame
    void Update()
    {
        //Increae the game progress over time
        gameProgress += gameProgressSpeed * 0.01f;

        //Update the slider filler
        gameProgressSlider.GetComponent<Slider>().value = gameProgress * 0.01f;

        
        //Determination of energy decrease speed
        energyDecreaseSpeed =
            //engineModEnergyDecreaseLV1 +
            //engineModEnergyDecreaseLV2 +
            //engineModEnergyDecreaseLV3 +

            radarModEnergyDecreaseLV1 +
            radarModEnergyDecreaseLV2 +
            radarModEnergyDecreaseLV3 +

            shieldModEnergyDecrease +

            positionModEnergyDecreaseLV1 +
            positionModEnergyDecreaseLV2 +
            positionModEnergyDecreaseLV3 +
            positionModEnergyDecreaseLV4 +
            positionModEnergyDecreaseLV5

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
        }

        else if (shipSpeed == speedLV3)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV4;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = engineModEnergyDecreaseLV3_original;
        }

        else if (shipSpeed == speedLV2)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV3;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = engineModEnergyDecreaseLV3_original;
        }

        else if (shipSpeed == speedLV1)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            //engineModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV2 = engineMoEnergyDecreaseLV2_original;
        }

        else if (shipSpeed == speedLV0)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV1;
            //engineModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;
        }
    }

    public void DecelerateShip()
    {
        //Decrease speed movement
        if (shipSpeed == speedLV1)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV0;
            //engineModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 0;
            //engineModEnergyDecreaseLV2 = 0;
            //engineModEnergyDecreaseLV1 = 0;
        }

        else if (shipSpeed == speedLV2)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV1;
            //engineModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 0;
            //engineModEnergyDecreaseLV2 = 0;
            //engineModEnergyDecreaseLV1 = 0;
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
        }

        else if (shipSpeed == speedLV4)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV3;
            //engineModuleSpriteLV2.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV2 = 0;
            //engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;
        }

        else if (shipSpeed == speedLV5)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV4;
            //engineModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 0;
            //engineModEnergyDecreaseLV2 = engineMoEnergyDecreaseLV2_original;
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

            //Decrease energy
            radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV2_original;
        }

        else if (radarLV == 0)
        {
            //Update the radar LV and sprite
            radarLV = 1;
            radarModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            //Activate radar LV1
            ship.GetComponent<RadarController>().waveInterval = radarWaveIntervalLV1;

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
            ship.GetComponent<RadarController>().waveInterval = 0;

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

            //Decrease energy
            radarModEnergyDecreaseLV3 = 0;
            radarModEnergyDecreaseLV2 = radarModEnergyDecreaseLV1_original;
        }
    }


    public void ActivateShield()
    {
        numberOfKeyShieldPressed += 1;

        if (numberOfKeyShieldPressed % 2f != 0)
        {
            //if the number of tumes that the key was pressed is an odd number, the shield will be activated.
            //the count starts with 0, so the first hit will be a 1 => odd number => activate shield. Next press is 2 => even number => inactivate
            shield.SetActive(true);

            shieldModEnergyDecrease = shieldModEnergyDecrease_original;
        }

        else
        {
            shield.SetActive(false);

            shieldModEnergyDecrease = 0;
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
        }

        else if (positionLV == 3)
        {
            //Update the position LV and sprite
            positionLV = 4;
            positionModuleSpriteLV4.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV4);

            positionModEnergyDecreaseLV4 = positionModEnergyDecreaseLV4_original;
        }

        else if (positionLV == 2)
        {
            //Update the position LV and sprite
            positionLV = 3;
            positionModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV3);

            positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV3_original;
        }

        else if (positionLV == 1)
        {
            positionLV = 2;
            positionModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV2);

            positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV2_original;

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
        }

        else if (positionLV == 4)
        {
            //Update the position LV and sprite
            positionLV = 3;
            positionModuleSpriteLV4.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV3);

            positionModEnergyDecreaseLV4 = 0;
            positionModEnergyDecreaseLV3 = positionModEnergyDecreaseLV3_original;
        }

        else if (positionLV == 3)
        {
            //Update the position LV and sprite
            positionLV = 2;
            positionModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV2);

            positionModEnergyDecreaseLV3 = 0;
            positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV2_original;
        }

        else if (positionLV == 2)
        {
            positionLV = 1;
            positionModuleSpriteLV2.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV1);

            positionModEnergyDecreaseLV2 = 0;
            positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;

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
        }

        else if (positionLV == 4)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV4;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV4;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV4;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV4;
        }

        else if (positionLV == 3)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV3;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV3;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV3;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV3;
        }

        else if (positionLV == 2)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV2;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV2;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV2;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV2;
        }

        else if (positionLV == 1)
        {
            asteroidSpawner.GetComponent<AsteroidSpawner>().minAsteroidSpeed = minAsteroidSpeedLV1;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxAsteroidSpeed = maxAsteroidSpeedLV1;
            asteroidSpawner.GetComponent<AsteroidSpawner>().minRotationSpeed = minRotationSpeedLV1;
            asteroidSpawner.GetComponent<AsteroidSpawner>().maxRotationSpeed = maxRotationSpeedLV1;
        }
    }


    public void TriggerOverload()
    {
        numberOfKeyOverloadPressed += 1;

        if (numberOfKeyOverloadPressed % 2f != 0)
        {
            isOverload = true; // Set the overload state to true
            StartCoroutine(OverloadCountdown()); // Start the countdown
        }

        else
        {
            isOverload = false; // Set the overload state to false
        }
    }

    // Coroutine to handle the overload countdown
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
