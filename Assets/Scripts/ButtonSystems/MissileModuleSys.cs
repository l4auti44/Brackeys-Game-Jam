using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileModuleSys : SystemBlueprint
{
    private ShipMovement shipMovementScript;
    private GameManager gameManager;
    public float energyCost = 10f;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        shipMovementScript = GameObject.Find("player_ship").GetComponent<ShipMovement>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        module = SubSys.MissileModule;
    }


    override public void DoAction()
    {
        if (CanDoAction())
        {
            shipMovementScript.ShootMissile();
            gameManager.DecreaseEnergy(energyCost);
        }

    }
}
