using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RAS;
using UnityEngine;

public class RoomStage : AbstractStage
{
    [SerializeField] private GameObject _trash;
    [SerializeField] private GameObject _player;

    private Animator _playerAnim;
    private Queue<Note> _hitCheckQueue = new Queue<Note>();

    void Start ()
    {
		Debug.Log(_baseStage._fourBeatSecond);
        _playerAnim = _player.GetComponent<Animator>();
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
    
    public void SetTrashOne()
    {
        var obj = Instantiate(_trash);
        StartCoroutine(AutoDestroyNote(obj));
        obj.GetComponent<Animator>().SetFloat("AnimSpeed", _baseStage.GetAnimSpeed());
    }

    IEnumerator AutoDestroyNote(GameObject note)
    {
        var animator = note.GetComponent<Animator>();
        yield return new WaitUntil(() => _hitCheckQueue.Count != 0 && (_hitCheckQueue.First()._isSucceed || animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f));
        _hitCheckQueue.Dequeue();
        Destroy(note);
    }
    ///

    /// Interface Implement.
    public override void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (note._noteName)
        {
            case "fn1": 
            case "fn2":
            case "fn3":
            case "fn4":
            case "ThreeOneNotice":
                SetTrashOne();
                break;

            case "fc1":
            case "fc2":
            case "fc3":
            case "fc4":
                _hitCheckQueue.Enqueue(note);
                break;

            case "ThreeOneCheck":
            case "ThreeTwoCheck":
                SetTrashOne();
                _hitCheckQueue.Enqueue(note);
                break;

            case "ThreeThreeCheck":
                _hitCheckQueue.Enqueue(note);
                break;
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
        //SceneManager.LoadScene("MainSplash");
    }
}
