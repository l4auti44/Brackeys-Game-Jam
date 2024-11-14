using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyAndRepearSys : MonoBehaviour
{
    [SerializeField] private float BreakTime = 15f;

    private GameObject repairModuleIsActiveImage;

    //SYSTEMS
    private SoundButton MissileModule;
    private SoundButton ShieldModule;
    private SoundButton ArrowModule;
    

    private List<SoundButton> Systems = new List<SoundButton>();

    private SoundButton currentSystemBroken;

    [HideInInspector] public bool canRepair = false;

    private void Start()
    {
        MissileModule = GameObject.Find("MissileModule").GetComponent<SoundButton>();
        ShieldModule = GameObject.Find("ShieldModule").GetComponent<SoundButton>();
        ArrowModule = GameObject.Find("ArrowModule").GetComponent<SoundButton>();
        repairModuleIsActiveImage = GameObject.Find("IsActiveRepairSys");
        repairModuleIsActiveImage.SetActive(false);
        Systems.Add(MissileModule);
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
        repairModuleIsActiveImage.SetActive(true);
        yield return new WaitForSeconds(5f);
        repairModuleIsActiveImage.SetActive(false);
        canRepair = false;
    }
}
