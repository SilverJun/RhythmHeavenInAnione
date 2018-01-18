using System.Collections;
using RAS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StandStage : AbstractStage
{
    // 태훈이
    [SerializeField] private GameObject _taehoon;
    private Animator _taehoonAnim;
    [SerializeField] private GameObject _game;
    private Animator _gameAnim;
    [SerializeField] private GameObject _ani;
    private Animator _aniAnim;
    [SerializeField] private GameObject _player; // cartoon
    private Animator _playerAnim;

    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _fail;

    public AudioSource _audioSource;
    public AudioClip _beep;
    public AudioClip _one;
    public AudioClip _two;
    public AudioClip _three;

    void Start ()
    {
        _taehoonAnim = _taehoon.GetComponent<Animator>();
        _gameAnim = _game.GetComponent<Animator>();
        _aniAnim = _ani.GetComponent<Animator>();
        _playerAnim = _player.GetComponent<Animator>();

        SetAnimSpeed();
        _audioSource.volume = StageManager.Instance._fxVolume;
    }
	
	void FixedUpdate ()
    {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
	}

    /// StandStage functions
    void SetPlayerAction()
    {
        _playerAnim.SetTrigger("Action");
    }

    void SetNPCAction()
    {
        _taehoonAnim.SetTrigger("Action");
        _gameAnim.SetTrigger("Action");
        _aniAnim.SetTrigger("Action");
    }

    void SetAnimSpeed()
    {
        _taehoonAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _gameAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _aniAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _playerAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
    }

    void SetClear()
    {
        _clear.SetActive(true);
        StartCoroutine(AutoDisactive(_clear));
    }

    void SetFail()
    {
        _fail.SetActive(true);
        StartCoroutine(AutoDisactive(_fail));
    }

    IEnumerator AutoDisactive(GameObject obj)
    {
        yield return new WaitForSeconds(_baseStage._fourBeatSecond / 2.0f);
        obj.SetActive(false);
    }
    /// 

    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (note._noteName)
        {
            case "PatternNotice":
                _audioSource.PlayOneShot(_beep);
                break;
            case "check":
                SetNPCAction();
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        SetClear();
    }

    public override void OnFail(Note note)
    {
        SetFail();
    }

    public override void OnEnd(EndStageUI ui)
    {
    }

    public override void OnExit()
    {
        MenuInitializer._initStageName = "ExteriorMenu";
        SceneManager.LoadScene("MainSplash");
    }
}
