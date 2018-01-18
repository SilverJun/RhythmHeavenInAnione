using RAS;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioClip _beep;
    public AudioClip _patternNotice;
    
    void Start()
    {
        _playerAnimator = _player.GetComponent<Animator>();
        _bookAnimator = _book.GetComponent<Animator>();
        _monitorAnimator = _monitor.GetComponent<Animator>();
        _bookAnimator.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _AudioSource.volume = StageManager.Instance._fxVolume;
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

    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.

        // SetPattern(One, OneNotice, OneCheck)
        // SetPattern(Two, t1N, t2N, tN, t1C, t2C)
        switch (note._noteName)
        {
            case "OneNotice":
                _AudioSource.PlayOneShot(_patternNotice);
                SetBookOne();
                break;
            case "t1N":
                _AudioSource.PlayOneShot(_patternNotice);
                break;
            case "t2N":
                _AudioSource.PlayOneShot(_patternNotice);
                break;
            case "tN":
                SetBookTwo();
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        _AudioSource.PlayOneShot(_beep);
        SetMonitorO();
    }

    public override void OnFail(Note note)
    {
        SetMonitorX();
    }

    public override void OnEnd(EndStageUI ui)
    {
    }

    public override void OnExit()
    {
        MenuInitializer._initStageName = "SchoolMenu";
        SceneManager.LoadScene("MainSplash");
    }
}
