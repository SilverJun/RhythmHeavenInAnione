using RAS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GymStage : AbstractStage
{
    [SerializeField]
    private GameObject _speaker;
    private Animator _speakerAnim;
    [SerializeField]
    private GameObject _player;
    private Animator _playerAnim;

    public AudioSource _audioSource;
    public AudioClip _whistle;
    public AudioClip _clear;
    public AudioClip _fail;
    public AudioClip _swing;

    void Start ()
    {
        _speakerAnim = _speaker.GetComponent<Animator>();
        _playerAnim = _player.GetComponent<Animator>();
        _speakerAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
    }

    void FixedUpdate ()
    {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
	}


    /// Gym Stage Functions
    public void SetPlayerAction()
    {
        _audioSource.PlayOneShot(_swing);
        _playerAnim.SetTrigger("Action");
    }

    public void SetSpeakerSay()
    {
        _speakerAnim.SetTrigger("Speak");
    }

    public void SetSpeakerSuccess()
    {
        _speakerAnim.SetTrigger("Success");
    }

    public void SetSpeakerFail()
    {
        _speakerAnim.SetTrigger("Fail");
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
            // SetPattern(One, on1, oc1)
            // SetPattern(Three, tn1, tn2, tn3, tc1, tc2, tc3)
            case "on1":
            case "tn1":
            case "tn2":
            case "tn3":
                SetSpeakerSay();
                _audioSource.PlayOneShot(_whistle);
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        _audioSource.PlayOneShot(_clear);
        SetSpeakerSuccess();
    }

    public override void OnFail(Note note)
    {
        _audioSource.PlayOneShot(_fail);
        SetSpeakerFail();
    }

    public override void OnEnd(EndStageUI ui)
    {
        MenuInitializer._initStageName = "ExteriorMenu";
    }

    public override void OnExit()
    {
        SceneManager.LoadScene("MainSplash");
    }
}
