using UnityEngine;

public class UIPauseMenuController : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject resumeButton;
    private bool died = false;

    private void Awake()
    {
        pauseMenu = transform.Find("PauseMenu").gameObject;
        resumeButton = pauseMenu.transform.Find("ResumeButton").gameObject;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !died)
        {
            SwitchPauseMenu();
        }
    }

    private void OnEnable()
    {
        EventManager.Game.OnDie += SwitchPauseMenuOnDie;
    }
    private void OnDisable()
    {
        EventManager.Game.OnDie -= SwitchPauseMenuOnDie;
    }


    public void SwitchPauseMenu()
    {
        bool isActive = pauseMenu.activeSelf;

        Time.timeScale = isActive ? 1f : 0f;

        pauseMenu.SetActive(!isActive);
        SceneController.isGamePaused = pauseMenu.activeSelf;
    }

    private void SwitchPauseMenuOnDie(Component comp)
    {
        resumeButton.SetActive(false);
        died = true;
        SwitchPauseMenu();
        
    }
    
}
