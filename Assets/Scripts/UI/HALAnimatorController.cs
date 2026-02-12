using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HALAnimatorController : MonoBehaviour
{

    Animator _animator;
    [SerializeField] private float timerToSwitch = 2f;
    private float _timerToSwitch;
    private int randomNum = 0;
    private bool isTalking = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _timerToSwitch = timerToSwitch;
    }

    private void Update()
    {
        if (isTalking && _animator.GetCurrentAnimatorStateInfo(0).IsName("HAL_still")) isTalking = false;


        if (!isTalking)
            _timerToSwitch -= Time.deltaTime;
        if (_timerToSwitch < 0)
        {
            randomNum = Random.Range(0, 3);
            CicleBtwAnims(randomNum);
            _timerToSwitch = timerToSwitch;
        }
    }
    private void CicleBtwAnims(int num)
    {
        _animator.SetInteger("Random", num);
        _animator.SetTrigger("SwitchIdle");
    }

    private void SetTimerAnimDuration(float duration)
    {
        if (_animator != null)
        {
            _animator.SetFloat("TimerSpeed", duration);
        }
    }

    private void DisableCicling(DialogSystem.DialogEvents ev)
    {
        isTalking = true;
    }

    private void OnEnable()
    {
        EventManager.Game.OnTaskWaiting += SetTimerAnimDuration;
        EventManager.Game.OnDialog += DisableCicling;
    }

    private void OnDisable()
    {
        EventManager.Game.OnTaskWaiting -= SetTimerAnimDuration;
        EventManager.Game.OnDialog -= DisableCicling;

    }
}
