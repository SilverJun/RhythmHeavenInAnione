using RAS;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuditoriumStage : AbstractStage
{
    [SerializeField] private GameObject _king;
    [SerializeField] private GameObject _drum;
    [SerializeField] private GameObject _guitar;

    [SerializeField] private GameObject _crown;

    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _miss;

    void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        if (TouchManager.IsTouch)
        {
            PlayClearAnim();
            PlayCrownAnim();
        }
    }

    /// Auditorium Stage Functions
    public void PlayClearAnim()
    {
        //3.0f, -3.0f
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _clear.SetActive(true));
        seq.Append(_clear.transform.DORotate(new Vector3(0.0f, 0.0f, 3.0f), 0.1f));
        seq.Append(_clear.transform.DORotate(new Vector3(0.0f, 0.0f, -3.0f), 0.1f));
        seq.SetLoops(3);
        seq.AppendCallback(() => _clear.SetActive(false));
        seq.Play();
    }

    public void PlayMissAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _miss.SetActive(true));
        seq.Append(_miss.transform.DORotate(new Vector3(0.0f, 0.0f, 3.0f), 0.1f));
        seq.Append(_miss.transform.DORotate(new Vector3(0.0f, 0.0f, -3.0f), 0.1f));
        seq.SetLoops(3);
        seq.AppendCallback(() => _miss.SetActive(false));
        seq.Play();
    }

    public void PlayCrownAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_crown.transform.DOMoveY(0.25f, 0.1f));
        seq.Append(_crown.transform.DOMoveY(0.0f, 0.1f));
        seq.SetLoops(3);
        seq.Play();
    }

    public void SetPlayerAction()
    {
        //_playerAnim.SetTrigger("Action");
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
