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
    private float cooldown = 20f;
    private TextMeshPro text;

    override public void Start()
    {
        base.Start();
        destroySys = GameObject.Find("RepairAndDestroySystem").GetComponent<DestroyAndRepearSys>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        module = SubSys.repairModule;
        text = transform.GetComponentInChildren<TextMeshPro>();
        text.text = "0";

    }


    override public void DoAction()
    {
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
        StartCoroutine(UpdateCooldownNumber());
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }


    private IEnumerator UpdateCooldownNumber()
    {
        float currentCooldown = cooldown;

        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime; // Reduce the cooldown by the time since the last frame
            text.text = Mathf.Clamp(currentCooldown, 0, cooldown).ToString(); // Update the text with formatted cooldown time
            yield return null; // Wait for the next frame
        }

        text.text = "0"; // Ensure the text shows zero when cooldown ends
    }
}
