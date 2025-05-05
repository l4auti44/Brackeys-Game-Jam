using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PerkScriptableObject;

public class PerkController : MonoBehaviour
{
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

    public void PerformAction(PerkScriptableObject.Perks perk)
    {
        
        switch (perk)
        {
            case PerkScriptableObject.Perks.FullEnergy:
                gameManager.RestartMaxEnergy();
                break;
            case PerkScriptableObject.Perks.MissileCooldown:
                if (missileModule.cooldown > 0.15f)
                {
                    missileModule.cooldown -= 0.15f;
                }
                break;
            case PerkScriptableObject.Perks.Energy25:
                gameManager.energy += 25;
                break;
        }
    }
    public void DeclineButton()
    {
        EventManager.Game.OnPerkPicked.Invoke();
    }
}
