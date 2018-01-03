using System.Collections;
using RAS;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuditoriumStage : AbstractStage
{
    [SerializeField] private GameObject _king;
    private Animator _kingAnim;
    [SerializeField] private GameObject _drum;
    private Animator _drumAnim;
    [SerializeField] private GameObject _guitar;
    private Animator _guitarAnim;

    [SerializeField] private GameObject _crown;

    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _miss;

    public AudioSource _audioSource;
    public AudioClip _everyone;
    public AudioClip _notice;
    public AudioClip _wow;

    bool _isDown = true;
    bool _isSnare = true;

    void Start ()
    {
        _kingAnim = _king.GetComponent<Animator>();
        _drumAnim = _drum.GetComponent<Animator>();
        _guitarAnim = _guitar.GetComponent<Animator>();

        InvokeRepeating("SetAutoGuitarAnim", 0.0f, _baseStage._fourBeatSecond);
        InvokeRepeating("SetAutoDrumAnim", 0.0f, _baseStage._fourBeatSecond);
    }
	
	void FixedUpdate ()
    {
        if (TouchManager.IsTouch)
        {
            PlayCrownAnim();
        }
    }

    /// Auditorium Stage Functions
    public void PlayClearAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _clear.SetActive(true));
        seq.Append(_clear.transform.DORotate(new Vector3(0.0f, 0.0f, 3.0f), _baseStage._fourBeatSecond / 8.0f));
        seq.Append(_clear.transform.DORotate(new Vector3(0.0f, 0.0f, -3.0f), _baseStage._fourBeatSecond / 8.0f));
        seq.SetLoops(2);
        seq.AppendCallback(() => _clear.SetActive(false));
        seq.Play();
    }

    public void PlayMissAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _miss.SetActive(true));
        seq.Append(_miss.transform.DORotate(new Vector3(0.0f, 0.0f, 3.0f), _baseStage._fourBeatSecond / 8.0f));
        seq.Append(_miss.transform.DORotate(new Vector3(0.0f, 0.0f, -3.0f), _baseStage._fourBeatSecond / 8.0f));
        seq.SetLoops(2);
        seq.AppendCallback(() => _miss.SetActive(false));
        seq.Play();
    }

    public void PlayCrownAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_crown.transform.DOMoveY(0.25f, _baseStage._fourBeatSecond / 8.0f));
        seq.Append(_crown.transform.DOMoveY(0.0f, _baseStage._fourBeatSecond / 8.0f));
        seq.SetLoops(2);
        seq.Play();
    }

    void SetToSing()
    {
        _kingAnim.SetTrigger("ToSing");
    }

    void SetEveryone()
    {
        _kingAnim.SetTrigger("SayYo");
    }

    void SetIdle()
    {
        _kingAnim.SetTrigger("GoIdle");
    }
    
    void SetAutoGuitarAnim()
    {
        if (_isDown)
        {
            _isDown = false;
            _guitarAnim.SetTrigger("Up");
        }
        else
        {
            _isDown = true;
            _guitarAnim.SetTrigger("Down");
        }
    }

    void SetAutoDrumAnim()
    {
        if (_isSnare)
        {
            _isSnare = false;
            _drumAnim.SetTrigger("Tom");
        }
        else
        {
            _isSnare = true;
            _drumAnim.SetTrigger("Snare");
        }
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
            // SetPattern(Three, t1n, t2n, t3n, tn, t1c, t2c, t3c)
            case "t1n":
                SetToSing();
                _audioSource.PlayOneShot(_notice);
                break;
            case "t2n":
            case "t3n":
                _audioSource.PlayOneShot(_notice);
                break;
            case "tn":
                SetEveryone();
                _audioSource.PlayOneShot(_everyone);
                break;
            case "tn1":
                SetIdle();
                break;
            // SetPattern(Four, f1n, f2n, f3n, f4n, fn, f1c, f2c, f3c, f4c, fn1)
            case "f1n":
                SetToSing();
                _audioSource.PlayOneShot(_notice);
                break;
            case "f2n":
            case "f3n":
            case "f4n":
                _audioSource.PlayOneShot(_notice);
                break;
            case "fn":
                SetEveryone();
                _audioSource.PlayOneShot(_everyone);
                break;
            case "fn1":
                SetIdle();
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        _audioSource.PlayOneShot(_wow);
        PlayClearAnim();
    }

    public override void OnFail(Note note)
    {
        _audioSource.PlayOneShot(_wow);
        PlayMissAnim();
    }

    public override void OnEnd(EndStageUI ui)
    {
        CancelInvoke();
        MenuInitializer._initStageName = "SchoolMenu";
    }

    public override void OnExit()
    {
        SceneManager.LoadScene("MainSplash");
    }
}
