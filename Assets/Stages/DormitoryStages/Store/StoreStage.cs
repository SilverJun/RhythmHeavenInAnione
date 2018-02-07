using System.Collections;
using System.Collections.Generic;
using RAS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreStage : AbstractStage
{
    [SerializeField] private GameObject _player;
    private Animator _playerAnimator;
    [SerializeField] private GameObject _guest;
    private Animator _guestAnimator;
    [SerializeField] private List<Sprite> _guestList = new List<Sprite>();
    [SerializeField] private List<Sprite> _breadList = new List<Sprite>();
    [SerializeField] private GameObject _bread;
    private GameObject _bread1;
    private Animator _bread1Animator;
    private GameObject _bread2;
    private Animator _bread2Animator;

    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _fail;

    public AudioSource _audioSource;
    public AudioClip _grapBread;
    public AudioClip _giveBread;
    public AudioClip _clearClip;
    public AudioClip _failClip;

    private bool _isOne = false;
    private bool _isTwo = false;

    void Start ()
    {
        _playerAnimator = _player.GetComponent<Animator>();
        _guestAnimator = _guest.GetComponent<Animator>();

        _playerAnimator.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _guestAnimator.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());

        _bread1 = null;
        _bread2 = null;

        _audioSource.volume = StageManager.Instance._fxVolume;
    }
    
    /// Store Stage Functions
    void SetGuestRandom()
    {
        _guest.GetComponent<SpriteRenderer>().sprite = _guestList[Random.Range(0, _guestList.Count - 1)];
    }

    void SetBreadRandom(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sprite = _breadList[Random.Range(0, _guestList.Count - 1)];
    }
    
    void SetGuestOne()
    {
        _guestAnimator.SetTrigger("One");
    }

    void SetGuestTwo()
    {
        _guestAnimator.SetTrigger("Two");
    }

    void SetGuestOut()
    {
        _guestAnimator.SetTrigger("Out");
    }

    void SetGrapBread()
    {
        if ((_isOne || _isTwo) && _bread1 == null)
        {
            _bread1 = Instantiate(_bread);
            _bread1.SetActive(true);
            _bread1Animator = _bread1.GetComponent<Animator>();
            _bread1Animator.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
            SetBreadRandom(_bread1);
        }
        else if (_isTwo && _bread2 == null)
        {
            _bread2 = Instantiate(_bread);
            _bread2.SetActive(true);
            _bread2Animator = _bread2.GetComponent<Animator>();
            _bread2Animator.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
            SetBreadRandom(_bread2);
        }

        _audioSource.PlayOneShot(_grapBread);
        _playerAnimator.SetTrigger("Grap");
    }

    void SetPushBread()
    {
        if (_isOne)
        {
            if (_bread1 != null)
            {
                _bread1Animator.SetTrigger("Move");
            }
        }
        else if (_isTwo)
        {
            if (_bread1 != null)
            {
                _bread1Animator.SetTrigger("Move");
            }
            if (_bread2 != null)
            {
                _bread2Animator.SetTrigger("Move");
            }
        }
        _audioSource.PlayOneShot(_giveBread);
        _playerAnimator.SetTrigger("Give");
    }

    void SetDeleteBread()
    {
        Destroy(_bread1);
        Destroy(_bread2);
        _bread1 = null;
        _bread2 = null;
        _bread1Animator = null;
        _bread2Animator = null;
    }

    void SetClear()
    {
        _clear.SetActive(true);
        _audioSource.PlayOneShot(_clearClip);
        StartCoroutine(AutoDisactive(_clear));
    }

    void SetFail()
    {
        _fail.SetActive(true);
        _audioSource.PlayOneShot(_failClip);
        StartCoroutine(AutoDisactive(_fail));
    }

    IEnumerator AutoDisactive(GameObject obj)
    {
        yield return new WaitForSeconds(_baseStage._fourBeatSecond / 2.0f);
        obj.SetActive(false);
    }

    ///

    /// Interface Implement.
    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (note._noteName)
        {
            //SetPattern(One, come, one, onecheck, givecheck, out)
            //SetPattern(Two, come, two, twocheck, twocheck, givecheck, out)
            case "onenotice":
                _isOne = true;
                SetGuestRandom();
                SetGuestOne();
                break;
            case "twonotice":
                _isTwo = true;
                SetGuestRandom();
                SetGuestTwo();
                break;
            case "out":
                _isOne = false;
                _isTwo = false;
                SetGuestOut();
                SetDeleteBread();
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        SetClear();
        switch (note._noteName)
        {
            case "givecheck":
                SetPushBread();
                break;
            case "onecheck":
                SetGrapBread();
                SetBreadRandom(_bread1);
                break;
            case "twocheck":
                SetGrapBread();
                SetBreadRandom(_bread1 != null ? _bread2 : _bread1);
                break;
        }
    }

    public override void OnFail(Note note)
    {
        SetFail();

        switch (note._noteName)
        {
            case "givecheck":
                _isOne = false;
                _isTwo = false;
                SetGuestOut();
                SetDeleteBread();
                break;
        }
    }

    public override void OnEnd(EndStageUI ui)
    {
    }

    public override void OnExit()
    {
        MenuInitializer._initStageName = "DormitoryMenu";
        SceneManager.LoadScene("MainSplash");
    }
}
