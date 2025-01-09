using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsModuleSys : SystemBlueprint
{
    private GameManager gameManager;


    override public void Start()
    {
        base.Start();
        module = SubSys.ArrowModule;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    override public void DoAction()
    {
        if (CanDoAction())
        {
            gameManager.ToggleArrows();
        }
        

    }
}
