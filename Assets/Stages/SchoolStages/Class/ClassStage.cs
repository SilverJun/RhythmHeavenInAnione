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

    public List<GameObject> _decoSpriteList = new List<GameObject>();
    public List<GameObject> _graffitiSpriteList = new List<GameObject>();

    void Start()
    {
        _graffitiAnim = _graffiti.GetComponent<Animator>();
        _playerAnim = _player.GetComponent<Animator>();
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
        MenuInitializer._initStageName = "SchoolMenu";
    }

    public override void OnExit()
    {
        SceneManager.LoadScene("MainSplash");
    }
}
