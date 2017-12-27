using JetBrains.Annotations;
using RAS;
using System.Collections;
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
        _playerAnim.SetFloat("AnimSpeed", 1.0f);
        _playerAnim.SetBool("walk", true);
    }

    public void SetHeartBreak()
    {
        _heartAnim.SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
        _heartAnim.SetTrigger("Break");
    }

    public void SetGuageOne()
    {
        _guageLeft[0].SetActive(true);
        _guageLeft[1].SetActive(false);
        _guageLeft[2].SetActive(false);
        _guageRight[0].SetActive(true);
        _guageRight[1].SetActive(false);
        _guageRight[2].SetActive(false);
    }

    public void SetGuageTwo()
    {
        _guageLeft[0].SetActive(true);
        _guageLeft[1].SetActive(true);
        _guageLeft[2].SetActive(false);
        _guageRight[0].SetActive(true);
        _guageRight[1].SetActive(true);
        _guageRight[2].SetActive(false);
    }

    public void SetGuageThree()
    {
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
            
        }
    }

    public override void OnSuccess(Note note)
    {

    }

    public override void OnFail(Note note)
    {

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
