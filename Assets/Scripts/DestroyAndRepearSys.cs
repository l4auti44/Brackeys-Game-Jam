using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAndRepearSys : MonoBehaviour
{
    [SerializeField] private float BreakTime = 15f;
    
    //SYSTEMS
    private SoundButton MissileModule;
    private SoundButton RepairModule;
    private SoundButton ShieldModule;
    private SoundButton ArrowModule;

    private List<SoundButton> Systems = new List<SoundButton>();

    private SoundButton currentSystemBroken;

    private void Start()
    {
        MissileModule = GameObject.Find("MissileModule").GetComponent<SoundButton>();
        RepairModule = GameObject.Find("RepairModule").GetComponent<SoundButton>();
        ShieldModule = GameObject.Find("ShieldModule").GetComponent<SoundButton>();
        ArrowModule = GameObject.Find("ArrowModule").GetComponent<SoundButton>();

        Systems.Add(MissileModule);
        Systems.Add(RepairModule);
        Systems.Add(ShieldModule);
        Systems.Add(ArrowModule);

    }
    private void OnEnable()
    {
        EventManager.Game.OnTaskDialogFailed += BreakSomething;
    }

    private void OnDisable()
    {
        EventManager.Game.OnTaskDialogFailed -= BreakSomething;
    }

    private void BreakSomething()
    {
        var ramdomChoice = Systems[Random.Range(0, Systems.Count)];
        currentSystemBroken = ramdomChoice;
        StartCoroutine(BreakAndWait());
    }

    private IEnumerator BreakAndWait()
    {
        currentSystemBroken.interactable = false;
        yield return new WaitForSeconds(BreakTime);
        Repair();
    }

    private void Repair()
    {
        if (currentSystemBroken != null)
        {
            currentSystemBroken.interactable = true;
            currentSystemBroken = null;
        }
    }
}
