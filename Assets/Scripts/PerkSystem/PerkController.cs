using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static PerkScriptableObject;

public class PerkController : MonoBehaviour
{
    [Header("Perk Stat Mods")]

    [Tooltip("Determines how steep of a decline the benefits are for the perk effectiveness")]
    [SerializeField] float missile_perk_decayrate = 0;
    [Tooltip("The base number of the calculation, starting at 1")]
    [SerializeField] float missile_perk_startN = 0;

    [Tooltip("Determines how steep of a decline the benefits are for the perk effectiveness")]
    [SerializeField] float energy_pickup_perk_decayrate=0;
    [Tooltip("Base number of the calc, it represents the percentage of ShipMovement's energyCollectable")]
    [SerializeField] float energy_pickup_perk_startN = 0;

    int energy_perks=0;

    [Tooltip("Determines how steep of a decline the benefits are for the perk effectiveness")]
    [SerializeField] float repairtime_perk_decayrate = 0;
    [Tooltip("Base number of the calc, it represents the percentage of ShipMovement's energyCollectable")]
    [SerializeField] float repairtime_pickup_perk_startN = 0;

    int repairtime_perks = 0;

    [Tooltip("Determines how steep of a decline the benefits are for the perk effectiveness")]
    [SerializeField] float magnetism_perk_decayrate = 0;
    [Tooltip("Base number of the calc, it represents the percentage of ShipMovement's energyCollectable")]
    [SerializeField] float magnetism_perk_startN = 0;

    public int magnetism_perks = 0;



    [SerializeField] private GameObject perkSys;
    [SerializeField] private GameObject inputBlocker;
    private GameManager gameManager;
    private MissileModuleSys missileModule;
    private RepairModuleSys repairModule;





    private void OnEnable()
    {
        EventManager.Game.OnWin += SwitchEnable;
        
    }

    private void OnDisable()
    {
        EventManager.Game.OnWin -= SwitchEnable;
    }

    public void SwitchEnable()
    {
        perkSys.SetActive(!perkSys.activeSelf);
        if (perkSys.activeSelf)
        {
            EventManager.Game.OnGameStopped.Invoke();
            StartCoroutine(PlaySounds());
            gameManager.ResetCameraZoom();
        }
        else 
        {
            gameManager.ZoomOut(gameManager.cameraPosLVS[gameManager.positionLV - 1]);
        }
        
        inputBlocker.SetActive(!inputBlocker.activeSelf); 
        SceneController.isGameStopped = !SceneController.isGameStopped;
    }

    private void Start()
    {
        perkSys.SetActive(false);
        inputBlocker.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        missileModule = GameObject.Find("MissileModule").GetComponent<MissileModuleSys>();
        repairModule = GameObject.Find("RepairModule").GetComponent<RepairModuleSys>();
    }

    private IEnumerator PlaySounds()
    {
        SoundManager.PlaySound(SoundManager.Sound.PerkText);
        yield return new WaitForSeconds(2.25f);
        SoundManager.PlaySound(SoundManager.Sound.PerkButtons);
    }

    public void PerformAction(PerkScriptableObject perk)
    {
        PerkScriptableObject.Perks perk_yuh = perk.action;
        ManageMagnetism();

        //switch (perk_yuh)
        //{
        //    case PerkScriptableObject.Perks.MaxEnergyRestore:
        //        gameManager.EnergyToMax();
        //        break;
        //    case PerkScriptableObject.Perks.DecreaseMissileCoolDown:
        //        if (missileModule.cooldown > 0.15f)
        //        {
        //            MissileModCalc();
        //        }
        //        break;
        //    case PerkScriptableObject.Perks.EnergyCollectionBuff:
        //        {
        //            EnergyCollectionModCalc();
        //        }
        //        break;
        //    case PerkScriptableObject.Perks.DecreaseRepairTime:
        //        {
        //            RepairTimeModCalc();
        //        }
        //        break;
        //        break;
        //    case PerkScriptableObject.Perks.Magnetism:
        //        {
        //            ManageMagnetism();
        //        }
        //        break;
        //}
    }

    void ManageMagnetism()
    {
        gameManager.magnetic_field_Object.SetActive(true);
        magnetism_perks++;
            float f = magnetism_perk_startN * magnetism_perk_decayrate;

            gameManager.magnetic_field.RecieveMod(magnetism_perks, f);
        
  
    }
    void MissileModCalc()
    {
        //OG reduction was 0.15 increments before this system.
        missileModule.missile_mod++;
        float m = missileModule.missile_mod;
        float d = missile_perk_decayrate;
        float s = missile_perk_startN;

        float f = s / (d * m);

        missileModule.cooldown -= f;
    }

    void RepairTimeModCalc()
    {
        //OG reduction was 0.15 increments before this system.
        repairtime_perks++;

        float m = repairtime_perks;
        float d = repairtime_perk_decayrate;
        float s = repairtime_pickup_perk_startN;

        float f = s / (d * m);

        repairModule.cooldown -= f;
    }

    void EnergyCollectionModCalc()
    {
        energy_perks++;

        float eC = gameManager.ship_script.energyCollectable;

        float b = energy_pickup_perk_startN;
        float d = energy_pickup_perk_decayrate;
        int m = energy_perks; 

        float f = (b * eC) / (d * m);

        gameManager.ship_script.EnergyMod += f;
    }


    public void DeclineButton()
    {
        EventManager.Game.OnPerkPicked.Invoke();
    }
}
