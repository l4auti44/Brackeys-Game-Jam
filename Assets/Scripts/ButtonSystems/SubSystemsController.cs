using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemsController : MonoBehaviour
{
    //SYSTEMS
    private SystemBlueprint missileModule, shieldModule, arrowModule, repairModule;

    private Transform wheelIndicator;
    private int currentSys = 0;

    void Start()
    {
        wheelIndicator = GameObject.Find("WheelIndicator").GetComponent<Transform>();

        missileModule = GameObject.Find("MissileModule").GetComponent<SystemBlueprint>();
        shieldModule = GameObject.Find("ShieldModule").GetComponent<SystemBlueprint>();
        arrowModule = GameObject.Find("ArrowModule").GetComponent<SystemBlueprint>();
        repairModule = GameObject.Find("RepairModule").GetComponent<SystemBlueprint>();

        missileModule.SwitchAvailable();
        arrowModule.SwitchAvailable();
        repairModule.SwitchAvailable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            wheelIndicator.Rotate(0, 0, wheelIndicator.rotation.z + 90);
            currentSys += 1;

            if (currentSys > 3)
            {
                currentSys = 0;
            }


            TurnOffSystems();
            switch (currentSys) {
                case 0:
                    shieldModule.SwitchAvailable();
                    break;
                case 1:
                    missileModule.SwitchAvailable();
                    break;
                case 2:
                    repairModule.SwitchAvailable();
                    break;
                case 3:
                    arrowModule.SwitchAvailable();
                    break;
                default:
                    break;
            }
        }
    }

    private void TurnOffSystems()
    {
        arrowModule.SwitchAvailable(false);
        missileModule.SwitchAvailable(false);
        repairModule.SwitchAvailable(false);
        shieldModule.SwitchAvailable(false);
    }
}
