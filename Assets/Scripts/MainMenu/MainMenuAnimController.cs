using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimController : MonoBehaviour
{

    private Animator mainMenuAnim;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        EventManager.Login.OnLoginSuccess += PlayLoginAnim;
        EventManager.Login.OnLoginFailure += PlayLogFailedAnim;
        EventManager.Login.OnLoginLoading += () => mainMenuAnim.SetTrigger("Loading");
    }

    void OnDisable()
    {
        EventManager.Login.OnLoginSuccess -= PlayLoginAnim;
        EventManager.Login.OnLoginFailure -= PlayLogFailedAnim;
        EventManager.Login.OnLoginLoading -= () => mainMenuAnim.SetTrigger("Loading");
    }

    private void PlayLoginAnim()
    {
        mainMenuAnim.SetTrigger("Login");
    }

        private void PlayLogFailedAnim(string error)
    {
        mainMenuAnim.SetTrigger("Failed");
    }
}
