using RAS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialStage : BaseStage
{
    int _currentBeat = 0;

    public GameObject _timingNotice;

    new void Awake()
    {
        _pauseButton = FindObjectOfType<PauseButton>();

        _stageBgm.playOnAwake = false;
        _metronome.playOnAwake = false;
        _timingNotice.SetActive(false);

        // StageManger에서 스테이지 정보 불러와서 초기화.
        _scriptText = StageManager.Instance._stageScript;
        StageManager.Instance._baseStage = this;
        _stageBgm.volume = StageManager.Instance._bgVolume;
        _metronome.volume = StageManager.Instance._fxVolume;

        // RAS 파싱작업.
        _parser = new Parser(this, _scriptText);
        _parser.ParseSource();
        
        foreach (var item in _parser._initCommands)
        {
            switch (item._commandName)
            {
                case "SetBpm":
                    SetBpm(103.0f);
                    break;
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
        //StartCoroutine(StartStage());
        StartCoroutine(TutorialStart());
    }

    IEnumerator TutorialStart()
    {
        // 총 32 beat.
        _currentBeat = 0;
        int waitBeat = 0; 
        while (true)
        {
            foreach (var item in _parser._patterns)
            {
                if (waitBeat + GetNextBeat(item._totalBeat) >= 32)
                {
                    yield return new WaitWhile(() => _stageBgm.time > _fourBeatSecond);
                    waitBeat = 0;
                    _currentBeat = 0;
                }

                if (waitBeat == 0)
                {
                    waitBeat = GetFirstBeat(item._totalBeat);
                }
                else
                {
                    waitBeat += GetNextBeat(item._totalBeat);
                }

                _currentBeat = waitBeat;
                Debug.LogFormat("_currentBeat : {0}", _currentBeat);
                Debug.LogFormat("pattern totalBeat : {0}", item._totalBeat);
                Debug.LogFormat("waitBeat : {0}", waitBeat);
                yield return new WaitWhile(() => _stageBgm.time < _fourBeatSecond * waitBeat);
                yield return StartCoroutine(DoAction(item));
            }
        }
    }

    int GetFirstBeat(float beat)
    {
        int i = 1;
        while (i < 8)
        {
            if (beat <= 4 * i)
            {
                return 4 * i;
            }
            i++;
        }
        return 32;
    }

    int GetNextBeat(float beat)
    {
        int i = 1;
        while (i < 8)
        {
            // beat = 2, i 1 return 4
            // beat = 4, i 2 return 8
            // beat = 8, i 3 return 12
            // beat = 12, i 4 return 16
            if (beat < 4 * i)
            {
                return 4 * i;
            }
            i++;
        }
        return 32;
    }

    IEnumerator DoAction(Pattern pattern)
    {
        var genTime = BeatToRealSecond(_currentBeat);
        _judgementSecond = BeatToRealSecond(0.25f);

        var noteList = new List<Note>();

        float offsetTime = 0.0f;

        foreach (var note in pattern._noteSetting)
        {
            _stageNostes.Add(new Note(genTime + offsetTime, note._type, note._name, note._beat));
            noteList.Add(_stageNostes[_stageNostes.Count - 1]);
            offsetTime += BeatToRealSecond(note._beat);
        }

        for (int i = 0; i < noteList.Count; i++)
        {
            if (noteList[i]._type != NoteType.Notice)
            {
                StartCoroutine(CheckHit(noteList[i]));
            }
            else if (noteList[i]._type == NoteType.Notice)
            {
                StartCoroutine(BeforeHit(noteList[i]));
            }

            yield return new WaitWhile(() => _stageBgm.time <= noteList[i]._genTime);
            _stage.OnNote(noteList[i]);
        }
    }

    new IEnumerator CheckHit(Note note)
    {
        yield return new WaitWhile(() => note._genTime - _judgementSecond >= _stageBgm.time + _startDelay);

        _timingNotice.SetActive(true);

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
                }
            }
            yield return new WaitForFixedUpdate();
        }

        _timingNotice.SetActive(false);

        if (!note._isHit)
        {
            Debug.LogFormat("Not Hit! - {0}, {1}", note._genTime, _stageBgm.time);
            note._isSucceed = false;
            note._isHit = false;
            _stage.OnFail(note);
        }
    }
}
