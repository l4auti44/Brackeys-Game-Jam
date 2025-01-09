using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemsController : MonoBehaviour
{
    //SYSTEMS
    private SystemBlueprint missileModule, shieldModule, arrowModule;

    private Transform wheelIndicator;
    private int currentSys = 0;

    void Start()
    {
        wheelIndicator = GameObject.Find("WheelIndicator").GetComponent<Transform>();

        missileModule = GameObject.Find("MissileModule").GetComponent<SystemBlueprint>();
        shieldModule = GameObject.Find("ShieldModule").GetComponent<SystemBlueprint>();
        arrowModule = GameObject.Find("ArrowModule").GetComponent<SystemBlueprint>();

        missileModule.SwitchAvailable();
        arrowModule.SwitchAvailable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            wheelIndicator.Rotate(0, 0, wheelIndicator.rotation.z + 90);
            currentSys += 1;

            if (currentSys > 2)
            {
                currentSys = 0;
            }

            if (currentSys == 2)
            {
                wheelIndicator.Rotate(0, 0, wheelIndicator.rotation.z + 90);
            }
            

            switch (currentSys) {
                case 0:
                    shieldModule.SwitchAvailable();
                    arrowModule.SwitchAvailable();
                    break;
                case 1:
                    missileModule.SwitchAvailable();
                    shieldModule.SwitchAvailable();
                    break;
                case 2:
                    arrowModule.SwitchAvailable();
                    missileModule.SwitchAvailable();
                    break;
                default:
                    break;
            }
        }
    }
}
