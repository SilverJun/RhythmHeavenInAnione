using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public Animator _animator;
    private AnimatorStateInfo _currentBaseState;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        // Key Input Process
		if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("action");
            _animator.SetBool("idle", false);
        }

        // Animation
        _currentBaseState = _animator.GetCurrentAnimatorStateInfo(0);
        if (_currentBaseState.IsName("Player_Action") || _currentBaseState.IsName("Player_Beat"))
        {
            if (_currentBaseState.normalizedTime > 1.0f)
            {
                _animator.SetBool("idle", true);
            }
        }
    }
}
