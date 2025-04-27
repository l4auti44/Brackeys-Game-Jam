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

    public void SwitchEnable(Component comp)
    {
        perkSys.SetActive(!perkSys.activeSelf);
        inputBlocker.SetActive(!inputBlocker.activeSelf); 
        SceneController.isGameStopped = true;
    }

    private void Start()
    {
        perkSys.SetActive(false);
        inputBlocker.SetActive(false);
    }
}
