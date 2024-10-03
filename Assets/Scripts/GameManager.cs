using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public float gameProgress, gameProgressSpeed;
    public GameObject gameProgressSlider;
    public GameObject ship, asteroidSpawner, shield;
    public GameObject engineModuleSpriteLV1, engineModuleSpriteLV2, engineModuleSpriteLV3;
    public GameObject lightModuleSpriteLV1, lightModuleSpriteLV2, lightModuleSpriteLV3;
    public GameObject positionModuleSpriteLV1, positionModuleSpriteLV2, positionModuleSpriteLV3;
    public float shipSpeed, radarLV, positionLV;
    public float speedLV0, speedLV1, speedLV2, speedLV3; //Speed variations for each module level
    public Transform positionLV1, positionLV2, positionLV3; //Position variations for each module level
    public GameObject lightPlayerLV1, lightPlayerLV2; //Radar light variations for each module level

    public float energy = 100f; // Amount of total energy at any time
    public float energyDecreaseSpeed; //Speed at which the energy is depleted

    public float engineModEnergyDecreaseLV1, engineModEnergyDecreaseLV2, engineModEnergyDecreaseLV3;
    private float engineModEnergyDecreaseLV1_original, engineMoEnergyDecreaseLV2_original, engineModEnergyDecreaseLV3_original;

    public float lightModEnergyDecreaseLV1, lightModEnergyDecreaseLV2, lightModEnergyDecreaseLV3;
    private float lightModEnergyDecreaseLV1_original, lightModEnergyDecreaseLV2_original, lightModEnergyDecreaseLV3_original;

    public float shieldModEnergyDecrease;
    private float shieldModEnergyDecrease_original;
    private float numberOfKeyShieldPressed = 0;

    public float positionModEnergyDecreaseLV1, positionModEnergyDecreaseLV2, positionModEnergyDecreaseLV3;
    private float positionModEnergyDecreaseLV1_original, positionModEnergyDecreaseLV2_original, positionModEnergyDecreaseLV3_original;

    public GameObject energySlider;


    // Start is called before the first frame update
    void Start()
    {
        ship.GetComponent<ShipMovement>().speed = shipSpeed;

        //store values for all energy decrease mods
        engineModEnergyDecreaseLV1_original = engineModEnergyDecreaseLV1;
        engineMoEnergyDecreaseLV2_original = engineModEnergyDecreaseLV2;
        engineModEnergyDecreaseLV3_original = engineModEnergyDecreaseLV3;

        lightModEnergyDecreaseLV1_original = lightModEnergyDecreaseLV1;
        lightModEnergyDecreaseLV2_original = lightModEnergyDecreaseLV2;
        lightModEnergyDecreaseLV3_original = lightModEnergyDecreaseLV3;

        shieldModEnergyDecrease_original = shieldModEnergyDecrease;

        positionModEnergyDecreaseLV1_original = positionModEnergyDecreaseLV1;
        positionModEnergyDecreaseLV2_original = positionModEnergyDecreaseLV2;
        positionModEnergyDecreaseLV3_original = positionModEnergyDecreaseLV3;


        //set all energy decrease mods at 0 after storing their values
        engineModEnergyDecreaseLV1 = 0;
        engineModEnergyDecreaseLV2 = 0;
        engineModEnergyDecreaseLV3 = 0;

        lightModEnergyDecreaseLV1 = 0;
        lightModEnergyDecreaseLV2 = 0;
        lightModEnergyDecreaseLV3 = 0;

        shieldModEnergyDecrease = 0;

        positionModEnergyDecreaseLV1 = 0;
        positionModEnergyDecreaseLV2 = 0;
        positionModEnergyDecreaseLV3 = 0;


        //start game with engine module speed LV1
        shipSpeed = speedLV1;
        engineModuleSpriteLV1.GetComponent<Image>().color = Color.green;
        engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;

        //start game with light module LV1
        radarLV = 1;
        lightPlayerLV1.SetActive(true);
        lightPlayerLV2.SetActive(false);
        lightModEnergyDecreaseLV1 = lightModEnergyDecreaseLV1_original;
        lightModuleSpriteLV1.GetComponent<Image>().color = Color.green;

        //start game with position LV1
        positionLV = 1;
        positionModuleSpriteLV1.GetComponent<Image>().color = Color.green;
        positionModEnergyDecreaseLV1 = positionModEnergyDecreaseLV1_original;
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
            engineModEnergyDecreaseLV1 +
            engineModEnergyDecreaseLV2 +
            engineModEnergyDecreaseLV3 +

            lightModEnergyDecreaseLV1 +
            lightModEnergyDecreaseLV2 +
            lightModEnergyDecreaseLV3 +

            shieldModEnergyDecrease +

            positionModEnergyDecreaseLV1 +
            positionModEnergyDecreaseLV2 +
            positionModEnergyDecreaseLV3

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
        if (shipSpeed == speedLV2)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV3;
            engineModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV3 = engineModEnergyDecreaseLV3_original;
        }

        else if (shipSpeed == speedLV1)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            engineModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV2 = engineMoEnergyDecreaseLV2_original;
        }

        else if (shipSpeed == speedLV0)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV1;
            engineModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;
        }
    }

    public void DecelerateShip()
    {
        //Decrease speed movement
        if (shipSpeed == speedLV1)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV0;
            engineModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV3 = 0;
            engineModEnergyDecreaseLV2 = 0;
            engineModEnergyDecreaseLV1 = 0;
        }

        else if (shipSpeed == speedLV2)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV1;
            engineModuleSpriteLV2.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV2 = 0;
            engineModEnergyDecreaseLV1 = engineModEnergyDecreaseLV1_original;
        }

        else if (shipSpeed == speedLV3)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            engineModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV3 = 0;
            engineModEnergyDecreaseLV2 = engineMoEnergyDecreaseLV2_original;
        }

    }


    public void IncreaseRadar()
    {
        //Increase radar LV
        if (radarLV == 2)
        {
            //Update the radar LV and sprite
            radarLV = 3;
            lightModuleSpriteLV3.GetComponent<Image>().color = Color.green;

            //Activate arrow spawning for LV3
            lightModEnergyDecreaseLV3 = lightModEnergyDecreaseLV3_original;
            asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow = true;

        }

        else if (radarLV == 1)
        {
            radarLV = 2;
            lightModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            lightPlayerLV1.SetActive(true);
            lightPlayerLV2.SetActive(true);

            lightModEnergyDecreaseLV2 = lightModEnergyDecreaseLV2_original;
        }

        else if (radarLV == 0)
        {
            radarLV = 1;
            lightModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            lightPlayerLV1.SetActive(true);
            lightPlayerLV2.SetActive(false);

            lightModEnergyDecreaseLV1 = lightModEnergyDecreaseLV1_original;
        }
    }

    public void DecreaseRadar()
    {
        //Decrease radar LV
        if (radarLV == 1)
        {
            radarLV = 0;
            lightModuleSpriteLV1.GetComponent<Image>().color = Color.white;

            lightPlayerLV1.SetActive(false);
            lightPlayerLV2.SetActive(false);

            lightModEnergyDecreaseLV1 = 0;
        }

        else if (radarLV == 2)
        {
            radarLV = 1;
            lightModuleSpriteLV2.GetComponent<Image>().color = Color.white;

            lightPlayerLV1.SetActive(true);
            lightPlayerLV2.SetActive(false);

            lightModEnergyDecreaseLV2 = 0;
            lightModEnergyDecreaseLV1 = lightModEnergyDecreaseLV1_original;
        }

        else if (radarLV == 3)
        {
            radarLV = 2;
            lightModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            lightPlayerLV1.SetActive(true);
            lightPlayerLV2.SetActive(true);

            lightModEnergyDecreaseLV3 = 0;
            lightModEnergyDecreaseLV2 = lightModEnergyDecreaseLV2_original;

            //Deactivate arrow spawn
            asteroidSpawner.GetComponent<AsteroidSpawner>().spawnArrow = false;
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
        if (positionLV == 2)
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
        if (positionLV == 3)
        {
            //Update the position LV and sprite
            positionLV = 2;
            positionModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            ship.GetComponent<ShipMovement>().MoveToLevel(positionLV2);

            positionModEnergyDecreaseLV3 = 0;
            positionModEnergyDecreaseLV2 = positionModEnergyDecreaseLV1_original;
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

}
