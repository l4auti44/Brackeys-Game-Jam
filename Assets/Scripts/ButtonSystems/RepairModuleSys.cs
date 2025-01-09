using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairModuleSys : SystemBlueprint
{
    private DestroyAndRepearSys destroySys;
    private GameManager gameManager;
    public float energyCost = 10f;


    override public void Start()
    {
        base.Start();
        destroySys = GameObject.Find("RepairAndDestroySystem").GetComponent<DestroyAndRepearSys>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        module = SubSys.repairModule;
        
    }


    override public void DoAction()
    {
        if (CanDoAction())
        {
            destroySys.OnRepairButtonClicked();
            gameManager.DecreaseEnergy(energyCost);
        }


    }
}
