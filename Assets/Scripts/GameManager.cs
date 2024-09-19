using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public float gameProgress, gameProgressSpeed;
    public GameObject gameProgressSlider;
    public GameObject ship;
    public GameObject engineModule, engineModuleSpriteLV1, engineModuleSpriteLV2, engineModuleSpriteLV3;
    public float shipSpeed;
    public float speedLV0, speedLV1, speedLV2, speedLV3; //Speed variations for each module level

    public float energy = 100f; // Amount of total energy at any time
    public float energyDecreaseSpeed; //Speed at which the energy is depleted
    public float engineModEnergyDecreaseLV1, engineModEnergyDecreaseLV2, engineModEnergyDecreaseLV3;
    private float engineModuleEnergyDecreaseLV1_original, engineModuleEnergyDecreaseLV2_original, engineModuleEnergyDecreaseLV3_original;
    public GameObject energySlider;


    // Start is called before the first frame update
    void Start()
    {
        ship.GetComponent<ShipMovement>().speed = shipSpeed;

        //store values for all energy decrease mods
        engineModuleEnergyDecreaseLV1_original = engineModEnergyDecreaseLV1;
        engineModuleEnergyDecreaseLV2_original = engineModEnergyDecreaseLV2;
        engineModuleEnergyDecreaseLV3_original = engineModEnergyDecreaseLV3;

        //set all energy decrease mods at 0
        engineModEnergyDecreaseLV1 = 0;
        engineModEnergyDecreaseLV2 = 0;
        engineModEnergyDecreaseLV3 = 0;

        //start game with engine module speed LV1
        shipSpeed = speedLV1;
        engineModuleSpriteLV1.GetComponent<Image>().color = Color.green;
        engineModEnergyDecreaseLV1 = engineModuleEnergyDecreaseLV1_original;

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
            engineModEnergyDecreaseLV3;

        //Decrease energy over time
        energy -= energyDecreaseSpeed * 0.01f;

        //Update the energy value of the slider
        energySlider.GetComponent<Slider>().value = energy * 0.01f;

        //Deactivate all modules if energy is 0
        if (energy < 0)
        {
            shipSpeed = 0;
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
            engineModEnergyDecreaseLV3 = engineModuleEnergyDecreaseLV3_original;
            //engineModEnergyDecreaseLV2 = 1;
            //engineModEnergyDecreaseLV1 = 1;
        }

        else if (shipSpeed == speedLV1)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            engineModuleSpriteLV2.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 1;
            engineModEnergyDecreaseLV2 = engineModuleEnergyDecreaseLV2_original;
            //engineModEnergyDecreaseLV1 = 1;
        }

        else if (shipSpeed == speedLV0)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV1;
            engineModuleSpriteLV1.GetComponent<Image>().color = Color.green;

            //modify the energy decrease speed and reset the rest
            //engineModEnergyDecreaseLV3 = 1;
            //engineModEnergyDecreaseLV2 = 1;
            engineModEnergyDecreaseLV1 = engineModuleEnergyDecreaseLV1_original;
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
            engineModEnergyDecreaseLV3 = 0;
            engineModEnergyDecreaseLV2 = 0;
            engineModEnergyDecreaseLV1 = engineModuleEnergyDecreaseLV1_original;
        }

        else if (shipSpeed == speedLV3)
        {
            //modify horizontal ship movment
            shipSpeed = speedLV2;
            engineModuleSpriteLV3.GetComponent<Image>().color = Color.white;

            //modify the energy decrease speed and reset the rest
            engineModEnergyDecreaseLV3 = 0;
            engineModEnergyDecreaseLV2 = engineModuleEnergyDecreaseLV2_original;
        }

    }

    public void IncreaseRadar()
    {

    }

    public void DecreaseRadar()
    {

    }



}
