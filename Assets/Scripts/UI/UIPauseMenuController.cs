using System.Collections;
using UnityEngine;


public class UIPauseMenuController : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject resumeButton;
    private bool diedOrWinConditions = false;
    private TMPro.TMP_Text titleText;

    private void Awake()
    {
        pauseMenu = transform.Find("PauseMenu").gameObject;
        resumeButton = pauseMenu.transform.Find("ResumeButton").gameObject;
        titleText = pauseMenu.transform.Find("Title").GetComponent<TMPro.TMP_Text>();
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !diedOrWinConditions)
        {
            SwitchPauseMenu();
        }
    }

    private void OnEnable()
    {
        EventManager.Game.OnDie += SwitchPauseMenuOnDie;
        EventManager.Game.OnWin += SwitchPauseMenuOnWin;
    }
    private void OnDisable()
    {
        EventManager.Game.OnDie -= SwitchPauseMenuOnDie;
        EventManager.Game.OnWin -= SwitchPauseMenuOnWin;
    }


    public void SwitchPauseMenu()
    {
        bool isActive = pauseMenu.activeSelf;
        if (!diedOrWinConditions) titleText.text = "GAME PAUSED";

        Time.timeScale = isActive ? 1f : 0f;

        pauseMenu.SetActive(!isActive);
        SceneController.isGamePaused = pauseMenu.activeSelf;
    }

    private void SwitchPauseMenuOnDie(Component comp)
    {

        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1.4f);
        resumeButton.SetActive(false);
        titleText.text = "YOU DIED!";
        diedOrWinConditions = true;
        SwitchPauseMenu();
    }
    private void SwitchPauseMenuOnWin(Component comp)
    {
        resumeButton.SetActive(false);
        titleText.text = "YOU WON!!";
        diedOrWinConditions = true;
        SwitchPauseMenu();

    }
    
    
}
