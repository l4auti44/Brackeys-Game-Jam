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
    [SerializeField] private TextMeshPro text;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        shipMovementScript = GameObject.Find("player_ship").GetComponent<ShipMovement>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        module = SubSys.MissileModule;
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
        yield return StartCoroutine(UpdateCooldownNumber());
        isOnCooldown = false;

    }

    private IEnumerator UpdateCooldownNumber()
    {
        float currentCooldown = cooldown;

        while (currentCooldown > 0)
        {

            if (SceneController.isGameStopped)
            {
                yield return null;
                continue;
            }

            currentCooldown -= Time.deltaTime;
            float secondsLeft = Mathf.Clamp(currentCooldown, 0f, cooldown);
            text.text = secondsLeft.ToString("F1");
            yield return null;
        }

        text.text = "0";
    }
}
