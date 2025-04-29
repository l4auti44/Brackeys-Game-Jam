using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkController : MonoBehaviour
{
    [SerializeField] private GameObject perkSys;
    [SerializeField] private GameObject inputBlocker;

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
        if(perkSys.activeSelf) StartCoroutine(PlaySounds());
        inputBlocker.SetActive(!inputBlocker.activeSelf); 
        SceneController.isGameStopped = !SceneController.isGameStopped;
    }

    private void Start()
    {
        perkSys.SetActive(false);
        inputBlocker.SetActive(false);
    }

    private IEnumerator PlaySounds()
    {
        SoundManager.PlaySound(SoundManager.Sound.PerkText);
        yield return new WaitForSeconds(2.25f);
        SoundManager.PlaySound(SoundManager.Sound.PerkButtons);
    }
}
