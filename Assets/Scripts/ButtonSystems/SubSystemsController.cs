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
    public float speed = 1.5f;

    [SerializeField] private Transform missilePos;
    [SerializeField] private Transform shieldPos;
    [SerializeField] private Transform repairPos;
    [SerializeField] private RectTransform arrowPos;

    [SerializeField] private float initalX;
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

        //initalX = wheelIndicator.GetComponent<RectTransform>().localPosition.x;


        StartCoroutine(GoToPos(arrowPos.localPosition));
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isRotating)
        {
            TurnOffSystems();
            //StartCoroutine(GoToPos());
            //StartCoroutine(RotateWheel(timeForRotation));
        }
    }

    private void TurnOffSystems()
    {
        arrowModule.SwitchAvailable(false);
        missileModule.SwitchAvailable(false);
        repairModule.SwitchAvailable(false);
        shieldModule.SwitchAvailable(false);
    }

    public IEnumerator GoToPos(Vector3 targetPos)
    {
        RectTransform rectTransform = wheelIndicator.GetComponent<RectTransform>();

        Vector3 goToInitialX = new Vector3(initalX, rectTransform.localPosition.y, rectTransform.localPosition.z);
        while (Vector3.Distance(rectTransform.localPosition, goToInitialX) > 0.01f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(
                rectTransform.localPosition,
                goToInitialX,
                speed * Time.deltaTime
            );
            yield return null;
        }


        Vector3 yTarget = new Vector3(rectTransform.localPosition.x, targetPos.y, rectTransform.localPosition.z);
        while (Vector3.Distance(rectTransform.localPosition, yTarget) > 0.01f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(
                rectTransform.localPosition,
                yTarget,
                speed * Time.deltaTime
            );
            yield return null;
        }


        Vector3 xTarget = new Vector3(targetPos.x, rectTransform.localPosition.y, rectTransform.localPosition.z);
        while (Vector3.Distance(rectTransform.localPosition, xTarget) > 0.01f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(
                rectTransform.localPosition,
                xTarget,
                speed * Time.deltaTime
            );
            yield return null;
        }

        rectTransform.localPosition = targetPos;
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
                if (!shieldModule.broken)
                    shieldModule.SwitchAvailable();
                
                break;
            case 1:
                if(!missileModule.broken)
                    missileModule.SwitchAvailable();
                break;
            case 2:
                repairModule.SwitchAvailable();
                break;
            case 3:
                if(!arrowModule.broken)
                arrowModule.SwitchAvailable();
                break;
            default:
                break;
        }


        isRotating = false;
    }

    private void CheckIfCurrentSysIsBroken(SystemBlueprint brokenSys)
    {
        if (brokenSys.GetAvailable()) brokenSys.SwitchAvailable();

    }
    private void OnEnable()
    {
        EventManager.Game.OnBrokenSystem += CheckIfCurrentSysIsBroken;
    }

    
}
