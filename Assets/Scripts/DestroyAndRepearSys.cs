using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyAndRepearSys : MonoBehaviour
{
    public enum Modules{
        MissileModule,
        ShieldModule,
        ArrowModule,
        EngineModule,
        radarModule
    }

    [SerializeField] private float BreakTime = 15f;

    private GameObject repairModuleIsActiveImage;

    //SYSTEMS
    private SoundButton MissileModule;
    private SoundButton ShieldModule;
    private SoundButton ArrowModule;

    //Only One button for now
    private SoundButton engineButtonUp;
    private SoundButton engineButtonDown;
    private SoundButton radarButtonUp;
    private SoundButton radarButtonDown;

    private List<SoundButton> Systems = new List<SoundButton>();

    private SoundButton currentSystemBroken;

    [HideInInspector] public bool canRepair = false;

    private void Start()
    {
        MissileModule = GameObject.Find("MissileModule").GetComponent<SoundButton>();
        ShieldModule = GameObject.Find("ShieldModule").GetComponent<SoundButton>();
        ArrowModule = GameObject.Find("ArrowModule").GetComponent<SoundButton>();
        var engineButtons = GameObject.Find("EngineButtons");
        engineButtonUp = engineButtons.transform.GetChild(0).GetComponent<SoundButton>();
        engineButtonDown = engineButtons.transform.GetChild(1).GetComponent<SoundButton>();
        var radarButtons = GameObject.Find("RadarButtons");
        radarButtonUp = radarButtons.transform.GetChild(0).GetComponent<SoundButton>();
        radarButtonDown = radarButtons.transform.GetChild(1).GetComponent<SoundButton>();
        repairModuleIsActiveImage = GameObject.Find("IsActiveRepairSys");
        repairModuleIsActiveImage.SetActive(false);
        Systems.Add(MissileModule);
        Systems.Add(ShieldModule);
        Systems.Add(ArrowModule);
        Systems.Add(engineButtonUp);
        Systems.Add(engineButtonDown);
        Systems.Add(radarButtonUp);
        Systems.Add(radarButtonDown);

    }
    private void OnEnable()
    {
        EventManager.Game.OnTaskDialogFailed += BreakSomething;
    }

    private void OnDisable()
    {
        EventManager.Game.OnTaskDialogFailed -= BreakSomething;
    }

    public void BreakSomething()
    {
        var ramdomChoice = Systems[Random.Range(0, Systems.Count)];
        currentSystemBroken = ramdomChoice;
        Debug.Log("System " + currentSystemBroken.name +" is broken");
        StartCoroutine(BreakAndWait());
    }

    private IEnumerator BreakAndWait()
    {
        currentSystemBroken.isBroken = true;
        if (currentSystemBroken == Systems[3])
        {
            Systems[4].isBroken = true;
        }
        if (currentSystemBroken == Systems[5])
        {
            Systems[6].isBroken = true;
        }
        if (ColorUtility.TryParseHtmlString("#C8C8C8", out Color disabledColor))
        {
            var colors = currentSystemBroken.colors;
            colors.disabledColor = disabledColor;
            currentSystemBroken.colors = colors;
        }

        yield return new WaitForSeconds(BreakTime);
        Repair();
    }

    public void Repair()
    {
        if (currentSystemBroken != null)
        {

            canRepair = false;
            Debug.Log("System " + currentSystemBroken.name + " has been repaired");
            repairModuleIsActiveImage.SetActive(false);
            StopAllCoroutines();
            currentSystemBroken.isBroken = false;
            if (currentSystemBroken == Systems[3])
            {
                Systems[4].isBroken = false;
            }
            if (currentSystemBroken == Systems[5])
            {
                Systems[6].isBroken = false;
            }
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color enabledColor))
            {
                var colors = currentSystemBroken.colors;
                colors.disabledColor = enabledColor;
                currentSystemBroken.colors = colors;
            }

            currentSystemBroken = null;
        }
    }

    public void OnRepairButtonClicked()
    {
        if (canRepair == false)
        {
            StartCoroutine(RepairButtonClicked());
        } 
    }

    private IEnumerator RepairButtonClicked()
    {
        canRepair = true;
        //repairModuleIsActiveImage.SetActive(true);
        yield return new WaitForSeconds(5f);
        repairModuleIsActiveImage.SetActive(false);
        canRepair = false;
    }


    public void TestBreakSomething(Modules module)
    {
        SoundButton breakSis;
        switch (module){
            case Modules.MissileModule:
                breakSis = Systems[0];
                break;
            case Modules.ShieldModule:
                breakSis = Systems[1];
                break;
            case Modules.ArrowModule:
                breakSis = Systems[2];
                break;
            case Modules.EngineModule:
                breakSis = Systems[3];
                break;
            case Modules.radarModule:
                breakSis = Systems[5];
                break;
            default:
                breakSis = Systems[0];
                break;
        }
        currentSystemBroken = breakSis;
        Debug.Log("System " + currentSystemBroken.name + " is broken");
        StartCoroutine(BreakAndWait());
    }
}
