using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimatorController : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Impact(Component comp)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("impact"))
        {
            _animator.SetTrigger("Impact");
        }
    }

    private void Win()
    {

        _animator.SetTrigger("Win");
        
    }

    private void OnEnable()
    {
        EventManager.Player.OnImpact += Impact;
        EventManager.Game.OnWin += Win;
        EventManager.Game.OnDie += Die;
    }

    private void Die(Component comp)
    {
        _animator.SetTrigger("GameOver");
    }
    

    private void OnDisable()
    {
        EventManager.Player.OnImpact -= Impact;
        EventManager.Game.OnWin -= Win;
        EventManager.Game.OnDie -= Die;
    }



}
