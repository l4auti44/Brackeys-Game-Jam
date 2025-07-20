using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemsController : MonoBehaviour
{
    //SYSTEMS
    private SystemBlueprint missileModule, shieldModule, repairModule;

    private Transform wheelIndicator;
    private int currentSys = 0;
    private RectTransform currentSysPos;
    private bool isMoving = false;
    public float speed = 1.5f;

    [SerializeField] private RectTransform missilePos;
    [SerializeField] private RectTransform shieldPos;
    [SerializeField] private RectTransform repairPos;
    //[SerializeField] private RectTransform arrowPos;

    [SerializeField] private float initalX;
    void Start()
    {
        wheelIndicator = GameObject.Find("WheelIndicator").GetComponent<Transform>();

        missileModule = GameObject.Find("MissileModule").GetComponent<SystemBlueprint>();
        shieldModule = GameObject.Find("ShieldModule").GetComponent<SystemBlueprint>();
        //arrowModule = GameObject.Find("ArrowModule").GetComponent<SystemBlueprint>();
        repairModule = GameObject.Find("RepairModule").GetComponent<SystemBlueprint>();
        missileModule.SwitchAvailable();
        //arrowModule.SwitchAvailable();
        repairModule.SwitchAvailable();

    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isMoving && !SceneController.isGameStopped)
        {
            TurnOffSystems();
            RotateSys();
            StartCoroutine(GoToPos(currentSysPos.localPosition));
        }
    }

    private void TurnOffSystems()
    {
        //arrowModule.SwitchAvailable(false);
        missileModule.SwitchAvailable(false);
        repairModule.SwitchAvailable(false);
        shieldModule.SwitchAvailable(false);
    }

    public IEnumerator GoToPos(Vector3 targetPos)
    {
        if (SceneController.isGameStopped) yield return null;
        isMoving = true;
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
        isMoving = false;
        EnableCurrentSys();
    }


    public void RotateSys()
    {

        currentSys += 1;

        if (currentSys > 2)
        {
            currentSys = 0;
        }

        switch (currentSys)
        {
            case 0:
                if (!shieldModule.broken)
                {
                    currentSysPos = shieldPos;
                }
                break;
            case 1:
                if (!missileModule.broken)
                {
                    currentSysPos = missilePos;
                }   
                break;
            case 2:
                currentSysPos = repairPos;
                break;
            default:
                break;
        }

    }

    private void EnableCurrentSys()
    {
        switch (currentSys)
        {
            case 0:
                if (!shieldModule.broken)
                {
                    shieldModule.SwitchAvailable();
                }
                break;
            case 1:
                if (!missileModule.broken)
                {
                    missileModule.SwitchAvailable();
                }
                break;
            case 2:
                repairModule.SwitchAvailable();
                break;
            default:
                break;
        }
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
