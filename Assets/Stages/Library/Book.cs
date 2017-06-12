using UnityEngine;
using System.Collections;

public class Book : MonoBehaviour
{
    public Animator _animator;
    private AnimatorStateInfo _currentBaseState;

    private Vector2 startPos;
    // Use this for initialization
    void Start()
    {
        startPos = new Vector2(3.67f, -1.66f);
        gameObject.transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetIdle()
    {
        _animator.SetBool("Idle", true);
    }
    public void SetOne()
    {
        _animator.SetBool("Idle", false);
        _animator.SetTrigger("One");
    }
    public void SetTwo()
    {
        _animator.SetBool("Idle", false);
        _animator.SetTrigger("Two");
    }
    public void SetThree()
    {
        _animator.SetBool("Idle", false);
        _animator.SetTrigger("Three");
    }
}
