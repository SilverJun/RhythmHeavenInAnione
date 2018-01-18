using RAS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomStage : AbstractStage
{
    [SerializeField] private GameObject _trash;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<Sprite> _trashSprite = new List<Sprite>();
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioClip _trashSFX;
    [SerializeField] private AudioClip _canSFX;

    private Animator _playerAnim;
    private List<GameObject> _trashNoteQueue = new List<GameObject>();

    void Start ()
    {
		Debug.Log(_baseStage._fourBeatSecond);
        _playerAnim = _player.GetComponent<Animator>();
        _sfx.volume = StageManager.Instance._fxVolume;
    }

    void FixedUpdate()
    {
        if (TouchManager.IsTouch)
        {
            SetPlayerAction();
        }
    }

    /// Room Stage Functions
    public void SetPlayerAction()
    {
        _playerAnim.SetTrigger("Action");
    }
    
    public void SetTrashOne(Note note)
    {
        var obj = Instantiate(_trash);
        _trashNoteQueue.Add(obj);
        obj.GetComponent<Animator>().SetFloat("AnimSpeed", _baseStage.GetAnimSpeed() * (1 / note._beat));
    }

    public void SetCanOne(Note note)
    {
        var obj = Instantiate(_trash);
        _trashNoteQueue.Add(obj);
        obj.GetComponent<SpriteRenderer>().sprite = _trashSprite[Random.Range(0, 2)];
        obj.GetComponent<Animator>().SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
    }
    
    public void PlayTrashSFX()
    {
        _sfx.PlayOneShot(_trashSFX);
    }

    public void PlayCanSFX()
    {
        _sfx.PlayOneShot(_canSFX);
    }
    ///

    /// Interface Implement.
    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (note._noteName)
        {
            case "on1":
            case "fn1":
            case "fn2":
            case "fn3":
            case "fn4":
            case "fon1":
            case "lon1":
                SetTrashOne(note);
                break;

            case "tn1":
            case "tc1":
            case "tc2":
                SetCanOne(note);
                break;
        }
    }

    public override void OnSuccess(Note note)
    {
        switch (note._noteName)
        {
            case "oc1":
            case "fc1":
            case "fc2":
            case "fc3":
            case "fc4":
                PlayTrashSFX();
                break;
            case "tc1":
            case "tc2":
            case "tc3":
                PlayCanSFX();
                break;
        }
        
        Destroy(_trashNoteQueue[0]);
        _trashNoteQueue.RemoveAt(0);
    }

    public override void OnFail(Note note)
    {
        if (_trashNoteQueue.Count != 0)
        {
            _trashNoteQueue.RemoveAt(0);
        }
    }

    public override void OnEnd(EndStageUI ui)
    {
    }

    public override void OnExit()
    {
        MenuInitializer._initStageName = "DormitoryMenu";
        SceneManager.LoadScene("MainSplash");
    }
}
