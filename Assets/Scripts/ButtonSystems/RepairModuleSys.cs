using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairModuleSys : SystemBlueprint
{
    private DestroyAndRepearSys destroySys;
    private GameManager gameManager;
    public float energyCost = 10f;
    private bool isOnCooldown = false;
    public float cooldown = 20f;
    [SerializeField] private TextMeshPro text;

    override public void Start()
    {
        base.Start();
        destroySys = GameObject.Find("RepairAndDestroySystem").GetComponent<DestroyAndRepearSys>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        module = SubSys.repairModule;
        text.text = "0";

    }


    override public void DoAction()
    {
        if (!GetAvailable() && !broken) subSystemsController.ChangePositonToSystem(SubSystemsController.SlotType.Repair);
        if (CanDoAction() && !isOnCooldown)
        {
            destroySys.Repair();
            gameManager.DecreaseEnergy(energyCost);
            StartCoroutine(StartCooldown());
        }


    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        // run the countdown coroutine and wait for it to finish; it already accounts for pauses
        yield return StartCoroutine(UpdateCooldownNumber());
        isOnCooldown = false;
    }


    private IEnumerator UpdateCooldownNumber()
    {
        float currentCooldown = cooldown;

        while (currentCooldown > 0)
        {
            // when game is stopped we should pause the countdown entirely
            if (SceneController.isGameStopped)
            {
                yield return null;
                continue; // skip decrement and text update until unpaused
            }

            currentCooldown -= Time.deltaTime;
            int secondsLeft = Mathf.CeilToInt(Mathf.Clamp(currentCooldown, 0f, cooldown));
            text.text = secondsLeft.ToString();
            yield return null;
        }

        text.text = "0";
    }
}
