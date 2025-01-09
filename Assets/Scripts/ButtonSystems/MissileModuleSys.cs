using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileModuleSys : SystemBlueprint
{
    private ShipMovement shipMovementScript;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        shipMovementScript = GameObject.Find("player_ship").GetComponent<ShipMovement>();
        module = Modules.MissileModule;
    }


    override public void DoAction()
    {
        if(CanDoAction())
        shipMovementScript.ShootMissile();
    }
}
