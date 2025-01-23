using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HALAnimatorController : MonoBehaviour
{

    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }


    private void SetTimerAnimDuration(float duration)
    {
        if (_animator != null)
        {
            _animator.SetFloat("TimerSpeed", duration);
        }
    }

    private void OnEnable()
    {
        EventManager.Game.OnTaskWaiting += SetTimerAnimDuration;
    }

    private void OnDisable()
    {
        EventManager.Game.OnTaskWaiting -= SetTimerAnimDuration;

    }
}
