using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemsController : MonoBehaviour
{
    //SYSTEMS
    private SystemBlueprint missileModule, shieldModule, arrowModule, repairModule;

    private Transform wheelIndicator;
    private int currentSys = 0;
    private bool isRotating = false;
    public float timeForRotation = 1.5f;
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
        if (Input.GetKeyDown(KeyCode.R) && !isRotating)
        {
            TurnOffSystems();
            StartCoroutine(RotateWheel(timeForRotation));
        }
    }

    private void TurnOffSystems()
    {
        arrowModule.SwitchAvailable(false);
        missileModule.SwitchAvailable(false);
        repairModule.SwitchAvailable(false);
        shieldModule.SwitchAvailable(false);
    }

    public IEnumerator RotateWheel(float duration)
    {
        
        Quaternion initialRotation = wheelIndicator.rotation;
        Quaternion finalRotation = Quaternion.Euler(wheelIndicator.rotation.eulerAngles + new Vector3(0, 0, 90));

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            isRotating = true;
            wheelIndicator.rotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wheelIndicator.rotation = finalRotation;
        currentSys += 1;

        if (currentSys > 3)
        {
            currentSys = 0;
        }

        
        switch (currentSys)
        {
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


        isRotating = false;
    }
}
