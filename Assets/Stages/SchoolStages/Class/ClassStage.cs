using RAS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassStage : AbstractStage
{
    [SerializeField]
    private GameObject _graffiti;
    private Animator _graffitiAnim;
    [SerializeField]
    private GameObject _player;
    private Animator _playerAnim;

    private GameObject _deco;

    public List<GameObject> _decoSpriteList = new List<GameObject>();
    public GameObject _fstGraffiti;
    public GameObject _secGraffiti;
    public GameObject _trdGraffiti;

    public AudioSource _audioSource;
    public AudioClip _pop;
    public AudioClip _erase;
    public AudioClip _fail;
    
    void Start()
    {
        _graffitiAnim = _graffiti.GetComponent<Animator>();
        _graffitiAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _playerAnim = _player.GetComponent<Animator>();
        _audioSource.volume = StageManager.Instance._fxVolume;
    }

    void FixedUpdate()
    {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
    }


    /// Class Stage Functions
    public void SetPlayerAction()
    {
        _playerAnim.SetTrigger("Action");
    }

    // normal
    void SetPattern1()
    {
        _fstGraffiti.GetComponent<SpriteRenderer>().color = Color.yellow;
        _secGraffiti.GetComponent<SpriteRenderer>().color = Color.red;
        _trdGraffiti.GetComponent<SpriteRenderer>().color = Color.blue;
        _graffitiAnim.SetTrigger("Pattern1");
    }

    // fast
    void SetPattern2()
    {
        _fstGraffiti.GetComponent<SpriteRenderer>().color = Color.yellow;
        _secGraffiti.GetComponent<SpriteRenderer>().color = Color.red;
        _trdGraffiti.GetComponent<SpriteRenderer>().color = Color.blue;
        _graffitiAnim.SetTrigger("Pattern2");
    }

    void SetDeco()
    {
        _deco = _decoSpriteList[Random.Range(0, _decoSpriteList.Count - 1)];
        _deco.GetComponent<SpriteRenderer>().color = Color.white;
        _deco.SetActive(true);
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
            //SetPattern(Three, tn1, tn2, tn3, tn4, tc1, tc2, tc3, tc4)
            //SetPattern(Fast, fn1, fn2, fn3, fn4, fn5, fc1, fc2, fc3, fc4, fc5)
            case "tn1":
                SetPattern1();
                _audioSource.PlayOneShot(_pop);
                break;
            case "tn2":
                _audioSource.PlayOneShot(_pop);
                break;
            case "tn3":
                _audioSource.PlayOneShot(_pop);
                break;
            case "tn4":
                SetDeco();
                break;
            case "tc4":
                _deco.SetActive(false);
                _graffitiAnim.SetTrigger("Idle");
                break;
            case "fn1":
                SetPattern2();
                SetDeco();
                break;
            case "fn2":
                _audioSource.PlayOneShot(_pop);
                break;
            case "fn3":
                _audioSource.PlayOneShot(_pop);
                break;
            case "fn4":
                _audioSource.PlayOneShot(_pop);
                break;
            case "fn5":
                break;
            case "fc5":
                _deco.SetActive(false);
                _graffitiAnim.SetTrigger("Idle");
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        _audioSource.PlayOneShot(_erase);
        switch (note._noteName)
        {
            case "tc1":
            case "fc2":
                _fstGraffiti.GetComponent<SpriteRenderer>().color = Color.clear;
                break;
            case "tc2":
            case "fc3":
                _secGraffiti.GetComponent<SpriteRenderer>().color = Color.clear;
                break;
            case "tc3":
            case "fc4":
                _trdGraffiti.GetComponent<SpriteRenderer>().color = Color.clear;
                break;
        }
    }

    public override void OnFail(Note note)
    {
        _audioSource.PlayOneShot(_fail);
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
