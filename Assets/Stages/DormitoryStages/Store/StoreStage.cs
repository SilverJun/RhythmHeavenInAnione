using System.Collections;
using System.Collections.Generic;
using RAS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreStage : AbstractStage
{
    [SerializeField] private GameObject _player;
    private GameObject _guest;
    [SerializeField] private List<Sprite> _guestList = new List<Sprite>();
    [SerializeField] private List<Sprite> _breadList = new List<Sprite>();
    private GameObject _bread;

    // position y = 2.5f, y = 2.7f

    // Use this for initialization
    void Start () {
		
	}

    void FixedUpdate()
    {
        if (TouchManager.IsTouch)
        {
        }
    }

    /// Store Stage Functions
    public void SetGuestRandom()
    {
        
    }

    public void SetGuestCome()
    {
        
    }

    public void SetGuestMoney()
    {
        
    }

    public void SetGuestAway()
    {
        
    }

    public void SetPlayerGrapBread()
    {
        
    }

    public void SetPlayerPushBread()
    {
        
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
