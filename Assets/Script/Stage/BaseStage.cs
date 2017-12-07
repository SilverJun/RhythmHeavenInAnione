using RAS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BaseStage : MonoBehaviour
{
    // RAS
    [SerializeField]
    private TextAsset _script;
    private Parser _parser;
    //

    // Stage, Judgement
    public AbstractStage _stage;
    public GameObject _stageObject;
    public List<Note> _stageNostes = new List<Note>();
    //

    public AudioSource _stageBgm;
    public AudioSource _metronome;

    private int _totalNote;
    private int _hitNote;

    private float _startDelay;
    private float _bpm;
    private float _fourBeatSecond;

    public void Awake()
    {
        _stageBgm.playOnAwake = false;
        _metronome.playOnAwake = false;

        // RAS 파싱작업.
        _parser = new Parser(_script);
        _parser.ParseSource();

        _startDelay = 0.0f;
        _hitNote = 0;
        //_totalNote = _parser._actions.Values.Sum((x) => x._noteSetting.Count((y) => y._type != "Notice"));

        foreach (var item in _parser._initCommands)
        {
            switch (item._commandName)
            {
                case "SetBpm":
                case "SetStage":
                case "SetPattern":
                    item._action(this);
                    break;
                default:
                    Debug.LogError("Undefined Command String in InitCommands : " + item._commandName);
                    break;
            }
        }

        _stageBgm.Play();
        //StartCoroutine(PlayMetronome());
        StartCoroutine(StartStage());
    }

    public IEnumerator PlayMetronome()
    {
        yield return new WaitForSeconds(_startDelay);
        while (true)
        {
            _metronome.Play();
            yield return new WaitForSeconds(_fourBeatSecond);
        }
    }

    /// <summary>
    /// 1. 스테이지 RAS에서 읽은 Action 메소드들을 준비.
    /// 2. Action리스트를 순회하면서 다음 실행할 노트를 미리 current Note에 저장
    /// 3. 해당 액션의 시간이 되면 액션 실행.
    /// 4. 2~3 반복.
    /// </summary>
    public IEnumerator StartStage()
    {
        yield return new WaitForSeconds(_startDelay);
        
        //StartCoroutine(CheckHit());
        
        foreach (var item in _parser._sheet)
        {
            switch (item._commandName)
            {
                case "SetBpm":
                case "SetStage":
                case "SetPattern":
                    yield return new WaitWhile(() => _stageBgm.time <= BeatToRealSecond(item._beat));
                    item._action(this);
                    break;
                case "Action":
                    yield return StartCoroutine(DoAction(item));
                    break;
                default:
                    Debug.LogError("Undefined Command String in Sheet : " + item._commandName);
                    break;
            }
        }
        
        yield return new WaitWhile(() => _stageBgm.isPlaying);
        yield return StartCoroutine(GoEndScene());
    }

    // Action 명령을 수행하는 코루틴. (Pattern에 맞는 Note생성)
    IEnumerator DoAction(Command action)
    {
        var pattern = _parser._patterns.Find((x) => x._name == action._patternName);
        var genTime = BeatToRealSecond(action._beat);
        var noteList = new List<Note>();

        float offsetTime = 0.0f;

        foreach (var note in pattern._noteSetting)
        {
            noteList.Add(new Note(genTime + offsetTime, note._type, note._name));
            offsetTime += BeatToRealSecond(note._beat);

            if (noteList.Last()._type != NoteType.Notice)
                _totalNote++;
        }

        foreach (var note in noteList)
        {
            if (note._type != NoteType.Notice)
            {
                StartCoroutine(CheckHit(note));
            }
            yield return new WaitWhile(() => _stageBgm.time <= note._genTime);
            _stage.OnNote(note);
        }
    }

    IEnumerator CheckHit(Note note)
    {
        yield return new WaitForSeconds(note._genTime - 0.033f - _stageBgm.time);

        while (_stageBgm.time <= note._genTime + 0.033f && !note._isHit)
        {
            // TODO : Touch, Swipe 판정 노트와 비교해서 맞추기.
            if (TouchManager.IsTouch || TouchManager.IsSwipe)
            {
                Debug.Log("Hit Success!");
                Debug.LogFormat("{0}, {1}", note._genTime, _stageBgm.time);
                _stage.OnSuccess();
                note._isHit = true;
                _hitNote++;
            }
            yield return new WaitForFixedUpdate();
        }

        if (!note._isHit)
        {
            Debug.Log("Hit Fail!");
            Debug.LogFormat("{0}, {1}", note._genTime, _stageBgm.time);
            _stage.OnFail();
        }
    }

    IEnumerator GoEndScene()
    {
        yield return new WaitForSeconds(2.0f);
        Instantiate(Resources.Load("Prefab/FadeOut") as GameObject);
        yield return new WaitForSeconds(2.0f);

        var ui = UIManager.OpenUI<EndStageUI>(Resources.Load("Prefab/EndStageUI") as GameObject);
        _stage.OnEnd(ui);
        ui.SetScore(_hitNote, _totalNote);
        _stageObject.SetActive(false);
    }

    public float BeatToRealMillisecond(float beat)
    {
        return beat * _fourBeatSecond * 1000.0f;
    }

    public float BeatToRealSecond(float beat)
    {
        return beat * _fourBeatSecond;
    }

    public void SetBpm(float bpm)
    {
        _bpm = bpm;
        _fourBeatSecond = 1.0f / (_bpm / 60.0f);
    }

    public void ExitStage()
    {
        _stage.OnExit();
    }
}
