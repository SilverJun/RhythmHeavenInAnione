using RAS;
using System.Collections.Generic;
using UnityEngine;

public class LibraryStage : AbstractStage
{
    [SerializeField]
    private GameObject _monitor;
    [SerializeField]
    private GameObject _book;
    [SerializeField]
    private GameObject _player;

    private Animator _playerAnimator;
    private Animator _bookAnimator;
    private Animator _monitorAnimator;

    public AudioSource _AudioSource = new AudioSource();
    public AudioClip _One;
    public AudioClip _Two;
    public AudioClip _TwoNote;
    public AudioClip _tOne;
    public AudioClip _tTwo;
    public AudioClip _tThree;
    public AudioClip _tFour;
    //public AudioClip _Success;
    
    void Start()
    {
        _playerAnimator = _player.GetComponent<Animator>();
        _bookAnimator = _book.GetComponent<Animator>();
        _monitorAnimator = _monitor.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
    }

    /// Library Stage Functions
    public void SetPlayerAction()
    {
        _playerAnimator.SetTrigger("action");
    }
    public void SetBookOne()
    {
        _bookAnimator.SetTrigger("One");
    }
    public void SetBookTwo()
    {
        _bookAnimator.SetTrigger("Two");
    }
    public void SetBookThree()
    {
        _bookAnimator.SetTrigger("Three");
    }
    public void SetMonitorO()
    {
        _monitorAnimator.SetTrigger("O");
    }
    public void SetMonitorX()
    {
        _monitorAnimator.SetTrigger("X");
    }
    ///

    /// Interface Implement.
    //public override void OnAction(float nBeat, Pattern pattern)
    //{
    //}

    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (note._noteName)
        {
            case "OneNotice":
                _AudioSource.PlayOneShot(_One);
                SetBookOne();
                break;
            case "TwoNotice":
                _AudioSource.PlayOneShot(_Two);
                SetBookTwo();
                break;
            case "ThreeOneNotice":
                _AudioSource.PlayOneShot(_tOne);
                SetBookThree();
                break;
            case "ThreeTwoNotice":
                SetBookThree();
                break;
            case "ThreeThreeNotice":
                SetBookThree();
                break;
            case "TwoCheck":
                _AudioSource.PlayOneShot(_TwoNote);
                break;
            case "OneCheck":
                _AudioSource.PlayOneShot(_One);
                break;
            case "ThreeOneCheck":
                _AudioSource.PlayOneShot(_tTwo);
                break;
            case "ThreeTwoCheck":
                _AudioSource.PlayOneShot(_tThree);
                break;
            case "ThreeThreeCheck":
                _AudioSource.PlayOneShot(_tFour);
                break;
        }
    }

    public override void OnSuccess()
    {
        SetMonitorO();
    }

    public override void OnFail()
    {
        SetMonitorX();
    }
}
