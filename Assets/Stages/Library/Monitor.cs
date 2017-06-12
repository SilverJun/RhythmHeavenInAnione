using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monitor : MonoBehaviour
{
    public Animator _animator;
    private AnimatorStateInfo _currentBaseState;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Animation
        _currentBaseState = _animator.GetCurrentAnimatorStateInfo(0);
        if (_currentBaseState.IsName("Monitor_O") || _currentBaseState.IsName("Monitor_X"))
        {
            if (_currentBaseState.normalizedTime > 1.0f)
            {
                _animator.SetBool("idle", true);
            }
        }
    }

    public void SetO()
    {
        _animator.SetBool("idle", false);
        _animator.SetTrigger("O");
    }

    public void SetX()
    {
        _animator.SetBool("idle", false);
        _animator.SetTrigger("X");
    }
}
