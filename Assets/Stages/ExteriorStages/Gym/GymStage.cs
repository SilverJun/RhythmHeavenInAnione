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

    void Start ()
    {
        _speakerAnim = _speaker.GetComponent<Animator>();
        _playerAnim = _player.GetComponent<Animator>();

    }

    void FixedUpdate () {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
	}


    /// Gym Stage Functions
    public void SetPlayerAction()
    {
        _playerAnim.SetTrigger("Action");
    }

    public void StartSpeakerSay()
    {
        _speakerAnim.SetBool("Say", true);
    }

    public void EndSpeakerSay()
    {
        _speakerAnim.SetBool("Say", false);
    }

    public void SetSpeakerSuccess()
    {
        _speakerAnim.SetTrigger("Success");
    }

    public void SetSpeakerFail()
    {
        _speakerAnim.SetTrigger("Success");
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
            
        }
    }

    public override void OnSuccess()
    {
    }

    public override void OnFail()
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
