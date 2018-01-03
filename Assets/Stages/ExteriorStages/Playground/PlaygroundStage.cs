using RAS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaygroundStage : AbstractStage
{
    [SerializeField] private GameObject _player;
    private Animator _playerAnim;

    public AudioSource _audioSource;
    public AudioClip _notice;
    public AudioClip _rope;

    void Start()
    {
        _playerAnim = _player.GetComponent<Animator>();
        _playerAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
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
        _playerAnim.SetTrigger("Action");
    }
    public void SetPlayerIdle()
    {
        _playerAnim.SetTrigger("Idle");
    }
    public void SetPlayerNotice()
    {
        _playerAnim.SetTrigger("Notice");
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
            case "n2":
            case "n4":
            case "n8":
                SetPlayerNotice();
                _audioSource.PlayOneShot(_notice);
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        _audioSource.PlayOneShot(_rope);
    }

    public override void OnFail(Note note)
    {
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
