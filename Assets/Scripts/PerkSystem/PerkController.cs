using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static PerkScriptableObject;

public class PerkController : MonoBehaviour
{
    [Header("Perk Stat Mods")]

    [Tooltip("Determines how steep of a decline the benefits are for the perk effectiveness")]
    [SerializeField] float missile_perk_decayrate;
    [Tooltip("The base number of the calculation, starting at 1")]
    [SerializeField] float missile_perk_startN;



    [SerializeField] private GameObject perkSys;
    [SerializeField] private GameObject inputBlocker;
    private GameManager gameManager;
    private MissileModuleSys missileModule;


   


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
        
        switch (perk_yuh)
        {
            case PerkScriptableObject.Perks.MaxEnergyRestore:
                gameManager.EnergyToMax();
                break;
            case PerkScriptableObject.Perks.DecreaseMissileCoolDown:
                if (missileModule.cooldown > 0.15f)
                {
                    MissileModCalc();
                }
                break;
                //case PerkScriptableObject.Perks.Energy25:
                //    gameManager.energy += 25;
                //    break;
        }
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


    public void DeclineButton()
    {
        EventManager.Game.OnPerkPicked.Invoke();
    }
}
