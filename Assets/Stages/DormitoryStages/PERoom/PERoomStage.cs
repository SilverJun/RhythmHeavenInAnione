using RAS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PERoomStage : AbstractStage
{
    [SerializeField] private GameObject _heart;
    private Animator _heartAnim;
    [SerializeField] private GameObject _player;
    private Animator _playerAnim;
    [SerializeField] private List<GameObject> _guageLeft = new List<GameObject>();
    [SerializeField] private List<GameObject> _guageRight = new List<GameObject>();

    public AudioSource _audioSource;
    public AudioClip _patternNotice;
    public AudioClip _clear;
    public AudioClip _fail;

    // Use this for initialization
    void Start ()
    {
        _heartAnim = _heart.GetComponent<Animator>();
        _playerAnim = _player.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
    }

    /// PERoom Stage Functions
    public void SetPlayerAction()
    {
        _playerAnim.SetTrigger("Walk");
    }

    public void SetHeartBreak()
    {
        _heartAnim.SetTrigger("Break");
    }

    public void SetGuageOne()
    {
        _heartAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed() * 0.5f);
        _playerAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed() * 0.5f);

        _guageLeft[0].SetActive(true);
        _guageLeft[1].SetActive(false);
        _guageLeft[2].SetActive(false);
        _guageRight[0].SetActive(true);
        _guageRight[1].SetActive(false);
        _guageRight[2].SetActive(false);
    }

    public void SetGuageTwo()
    {
        _heartAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _playerAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());

        _guageLeft[0].SetActive(true);
        _guageLeft[1].SetActive(true);
        _guageLeft[2].SetActive(false);
        _guageRight[0].SetActive(true);
        _guageRight[1].SetActive(true);
        _guageRight[2].SetActive(false);
    }

    public void SetGuageThree()
    {
        _heartAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed() * 2);
        _playerAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed() * 2);

        _guageLeft[0].SetActive(true);
        _guageLeft[1].SetActive(true);
        _guageLeft[2].SetActive(true);
        _guageRight[0].SetActive(true);
        _guageRight[1].SetActive(true);
        _guageRight[2].SetActive(true);
    }
    ///

    /// Interface Implement.
    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (note._noteName)
        {
            //NoteSetting(lowan, 2, Notice)
            //NoteSetting(lowc, 2, Touch)
            //
            //NoteSetting(normalan, 4, Notice)
            //NoteSetting(normalc, 4, Touch)
            //
            //NoteSetting(fastan, 8, Notice)
            //NoteSetting(fastc, 8, Touch)
            case "lowan":
                SetGuageOne();
                _audioSource.PlayOneShot(_patternNotice);
                break;
            case "normalan":
                SetGuageTwo();
                _audioSource.PlayOneShot(_patternNotice);
                break;
            case "fastan":
                SetGuageThree();
                _audioSource.PlayOneShot(_patternNotice);
                break;
        }
}

    public override void OnSuccess(Note note)
    {
        switch (note._noteName)
        {
            case "lowc":
            case "normalc":
            case "fastc":
                _audioSource.PlayOneShot(_clear);
                break;
        }
    }

    public override void OnFail(Note note)
    {
        switch (note._noteName)
        {
            case "lowc":
            case "normalc":
            case "fastc":
                _audioSource.PlayOneShot(_fail);
                break;
        }
        _heartAnim.SetTrigger("Break");
    }

    public override void OnEnd(EndStageUI ui)
    {
        MenuInitializer._initStageName = "DormitoryMenu";
    }

    public override void OnExit()
    {
        SceneManager.LoadScene("MainSplash");
    }
}
