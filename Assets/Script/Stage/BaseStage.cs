using RAS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BaseStage : MonoBehaviour
{
    // RAS
    [SerializeField]
    private TextAsset _script;
    protected string _scriptText;
    protected Parser _parser;
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

    public float _startDelay = 0.005f;
    public float _bpm;
    public float _fourBeatSecond;
    protected float _judgementSecond;

    protected PauseButton _pauseButton;

    public void Awake()
    {
        _pauseButton = FindObjectOfType<PauseButton>();

        _stageBgm.playOnAwake = false;
        _metronome.playOnAwake = false;

        // StageManger에서 스테이지 정보 불러와서 초기화.
        _stageBgm.clip = StageManager.Instance._stageBgm;
        if (_script == null)
        {
            _scriptText = StageManager.Instance._stageScript;
        }
        else
        {
            _scriptText = _script.text;
        }
        StageManager.Instance._baseStage = this;
        _stageBgm.volume = StageManager.Instance._bgVolume;
        _metronome.volume = StageManager.Instance._fxVolume;

        // RAS 파싱작업.
        _parser = new Parser(this, _scriptText);
        //_parser = new Parser(this, StageManager._stageScript);
        _parser.ParseSource();

        _totalNote = 0;
        _hitNote = 0;
        //_totalNote = _parser._actions.Values.Sum((x) => x._noteSetting.Count((y) => y._type != "Notice"));

        foreach (var item in _parser._initCommands)
        {
            switch (item._commandName)
            {
                case "SetBpm":
                case "SetStartDelay":
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
        //InvokeRepeating("PlayMetronome", _startDelay, _fourBeatSecond);
        StartCoroutine(StartStage());
    }

    public void PlayMetronome()
    {
        _metronome.Play();
    }
    
    /// <summary>
    /// 1. 스테이지 RAS에서 읽은 Action 메소드들을 준비.
    /// 2. Action리스트를 순회하면서 다음 실행할 노트를 미리 current Note에 저장
    /// 3. 해당 액션의 시간이 되면 액션 실행.
    /// 4. 2~3 반복.
    /// </summary>
    public IEnumerator StartStage()
    {
        //StartCoroutine(CheckHit());
        
        foreach (var item in _parser._sheet)
        {
            switch (item._commandName)
            {
                case "SetBpm":
                case "SetStartDelay":
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
        _judgementSecond = BeatToRealSecond(0.25f);

        var noteList = new List<Note>();

        float offsetTime = _startDelay;

        foreach (var note in pattern._noteSetting)
        {
            _stageNostes.Add(new Note(genTime + offsetTime, note._type, note._name, note._beat));
            noteList.Add(_stageNostes[_stageNostes.Count-1]);
            offsetTime += BeatToRealSecond(note._beat);
        }
        
        for (int i = 0; i < noteList.Count; i++)
        {
            if (noteList[i]._type != NoteType.Notice)
            {
                StartCoroutine(CheckHit(noteList[i]));
            }

            yield return new WaitWhile(() => _stageBgm.time <= noteList[i]._genTime + _startDelay);
            _stage.OnNote(noteList[i]);
            yield return new WaitWhile(() => _stageBgm.time <= noteList[i]._genTime + _judgementSecond + _startDelay);
        }
    }

    protected IEnumerator BeforeHit(Note note)
    {
        Debug.LogFormat("Before Start - {0}, {1}", note._genTime, _stageBgm.time);
        while (_stageBgm.time < note._genTime - _judgementSecond + _startDelay)
        {
            yield return new WaitForFixedUpdate();

            if (TouchManager.IsTouch)
            {
                Debug.LogFormat("Before Hit! - {0}, {1}", note._genTime, _stageBgm.time);
                _stage.OnFail(note);
                note._isHit = true;
                note._isSucceed = false;
                yield break;
            }
        }
        Debug.LogFormat("Before End - {0}, {1}", note._genTime, _stageBgm.time);
    }

    protected IEnumerator AfterHit(Note note)
    {
        Debug.LogFormat("After Start - {0}, {1}", note._genTime, _stageBgm.time);
        while (_stageBgm.time < note._genTime + _startDelay + note._beat * _fourBeatSecond - _judgementSecond)
        {
            yield return new WaitForFixedUpdate();

            if (TouchManager.IsTouch)
            {
                Debug.LogFormat("After Hit! - {0}, {1}", note._genTime, _stageBgm.time);
                _stage.OnFail(note);
                note._isHit = true;
                note._isSucceed = false;
                yield break;
            }
        }
        Debug.LogFormat("After End - {0}, {1}", note._genTime, _stageBgm.time);
    }

    protected IEnumerator CheckHit(Note note)
    {
        StartCoroutine(BeforeHit(note));

        yield return new WaitWhile(() => note._genTime - _judgementSecond >= _stageBgm.time + _startDelay);

        Debug.LogFormat("Check Start - {0}, {1}", note._genTime, _stageBgm.time);

        while (_stageBgm.time <= note._genTime + _judgementSecond + _startDelay)
        {
            if (note._type == NoteType.Touch && TouchManager.IsTouch)
            {
                if (note._isHit == false)
                {
                    Debug.LogFormat("Hit Success! - {0}, {1}", note._genTime, _stageBgm.time);
                    _stage.OnSuccess(note);
                    note._isSucceed = true;
                    note._isHit = true;
                }
                else
                {
                    Debug.LogFormat("Hit Already! - {0}, {1}", note._genTime, _stageBgm.time);
                    _stage.OnFail(note);
                    note._isSucceed = false;
                    note._isHit = true;
                }
            }
            yield return new WaitForFixedUpdate();
        }

        Debug.LogFormat("Check End - {0}, {1}", note._genTime, _stageBgm.time);

        if (!note._isHit)
        {
            Debug.LogFormat("Not Hit! - {0}, {1}", note._genTime, _stageBgm.time);
            note._isSucceed = false;
            note._isHit = false;
            _stage.OnFail(note);
        }
        else
        {
            StartCoroutine(AfterHit(note));
        }
    }

    IEnumerator GoEndScene()
    {
        yield return new WaitForSeconds(2.0f);
        var overlay = Instantiate(Resources.Load("Prefab/FadeOut") as GameObject);
        yield return new WaitForSeconds(2.0f);
        overlay.SetActive(false);
        _pauseButton.Close();
        var ui = UIManager.OpenUI<EndStageUI>(Resources.Load("Prefab/EndStageUI") as GameObject);
        _stage.OnEnd(ui);

        _hitNote = _stageNostes.Count((x) => x._type == NoteType.Touch && x._isSucceed);
        _totalNote = _stageNostes.Count((x) => x._type == NoteType.Touch);

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

    public float GetAnimSpeed()
    {
        return _bpm / 60.0f;
    }

    public void ExitStage()
    {
        _stage.OnExit();
    }
}
