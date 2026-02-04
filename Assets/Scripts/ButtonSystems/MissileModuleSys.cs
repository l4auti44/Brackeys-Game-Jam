using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissileModuleSys : SystemBlueprint
{
    private ShipMovement shipMovementScript;
    private GameManager gameManager;
    public float energyCost = 10f;
    public float cooldown = 1f;
    public float missile_mod;
    private bool isOnCooldown = false;
    private TextMeshPro text;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        shipMovementScript = GameObject.Find("player_ship").GetComponent<ShipMovement>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        module = SubSys.MissileModule;
        text = transform.GetComponentInChildren<TextMeshPro>();
        text.text = "0";
    }


    override public void DoAction()
    {
        if (!GetAvailable() && !broken) subSystemsController.ChangePositonToSystem(SubSystemsController.SlotType.Missile);
        if (CanDoAction() && !isOnCooldown)
        {
            shipMovementScript.ShootMissile();
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
