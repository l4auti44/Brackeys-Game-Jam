using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyAndRepearSys : MonoBehaviour
{
    public enum Modules{
        MissileModule,
        ShieldModule,
        ArrowModule
    }

    [SerializeField] private float BreakTime = 15f;


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

    

    private Queue<SoundButton> disabledSystems = new Queue<SoundButton>();

    private void Start()
    {
        MissileModule = GameObject.Find("MissileModule").GetComponent<SoundButton>();
        ShieldModule = GameObject.Find("ShieldModule").GetComponent<SoundButton>();
        ArrowModule = GameObject.Find("ArrowModule").GetComponent<SoundButton>();
        //var engineButtons = GameObject.Find("EngineButtons");
        //engineButtonUp = engineButtons.transform.GetChild(0).GetComponent<SoundButton>();
        //engineButtonDown = engineButtons.transform.GetChild(1).GetComponent<SoundButton>();
        //var radarButtons = GameObject.Find("RadarButtons");
        //radarButtonUp = radarButtons.transform.GetChild(0).GetComponent<SoundButton>();
        //radarButtonDown = radarButtons.transform.GetChild(1).GetComponent<SoundButton>();
        //repairModuleIsActiveImage = GameObject.Find("IsActiveRepairSys");
        //repairModuleIsActiveImage.SetActive(false);
        Systems.Add(MissileModule);
        Systems.Add(ShieldModule);
        Systems.Add(ArrowModule);
        //Systems.Add(engineButtonUp);
        //Systems.Add(engineButtonDown);
        //Systems.Add(radarButtonUp);
        //Systems.Add(radarButtonDown);

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
        //var ramdomChoice = Systems[Random.Range(0, Systems.Count)];
        List<SoundButton> enabledSystems = new List<SoundButton>();
        foreach (var system in Systems)
        {
            if (!system.isBroken)
            {
                enabledSystems.Add(system);
            }
        }

        if (enabledSystems.Count > 0)
        {
            int ramdomChoice = Random.Range(0, enabledSystems.Count);
            SoundButton currentSystemBroken = Systems[ramdomChoice];
            Debug.Log("System " + currentSystemBroken.name + " is broken");
            SystemBlueprint currentSys = currentSystemBroken.GetComponent<SystemBlueprint>();
            currentSys.broken = true;
            EventManager.Game.OnBrokenSystem.Invoke(currentSys);
            disabledSystems.Enqueue(currentSystemBroken);
        }


        

    }

    public void Repair()
    {
        if (disabledSystems.Count > 0)
        {

            SoundButton systemToEnable = disabledSystems.Dequeue();
            Debug.Log("System " + systemToEnable.name + " has been repaired");
            
            systemToEnable.GetComponent<SystemBlueprint>().broken = false;
        }
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
            default:
                breakSis = Systems[0];
                break;
        }
        SystemBlueprint currentSys = breakSis.GetComponent<SystemBlueprint>();
        if (!currentSys.broken)
        {
            currentSys.broken = true;
            EventManager.Game.OnBrokenSystem.Invoke(currentSys);
            disabledSystems.Enqueue(breakSis);
            Debug.Log("System " + breakSis.name + " is broken");
            
        }
    }
}
