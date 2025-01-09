using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModuleSys : SystemBlueprint
{
    private GameManager gameManager;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        module = SubSys.ShieldModule;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }


    override public void DoAction()
    {
        if (CanDoAction())
        {
            gameManager.ActivateShield();
        }
        
    }
}

