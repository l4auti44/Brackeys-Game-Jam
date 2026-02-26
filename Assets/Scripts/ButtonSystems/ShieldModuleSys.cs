using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShieldModuleSys : SystemBlueprint
{
    private GameManager gameManager;
    public float cooldown = 10f;
    private bool isOnCooldown = false;
    [SerializeField] private TextMeshPro text;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        module = SubSys.ShieldModule;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text.text = "0";
    }


    override public void DoAction()
    {
        if (!GetAvailable() && !broken) subSystemsController.ChangePositonToSystem(SubSystemsController.SlotType.Shield);
        if (CanDoAction() && !isOnCooldown)
        {
            gameManager.ActivateShield();
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
            currentCooldown -= Time.deltaTime;
            int secondsLeft = Mathf.CeilToInt(Mathf.Clamp(currentCooldown, 0f, cooldown));
            text.text = secondsLeft.ToString();
            yield return null;
        }

        text.text = "0";
    }
}

