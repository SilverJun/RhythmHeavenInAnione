using RAS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialStage : BaseStage
{
    int _currentBeat = 0;

    new void Awake()
    {
        _pauseButton = FindObjectOfType<PauseButton>();

        _stageBgm.playOnAwake = false;
        _metronome.playOnAwake = false;

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
                if (_currentBeat + item._totalBeat >= 32)
                {
                    _currentBeat = 0;
                    waitBeat = 0;
                    yield return new WaitWhile(() => _stageBgm.time >= _fourBeatSecond);
                }

                yield return StartCoroutine(DoAction(item));
                waitBeat += GetNextBeat(item._totalBeat);
                Debug.LogFormat("_currentBeat : {0}", _currentBeat);
                Debug.LogFormat("pattern totalBeat : {0}", item._totalBeat);
                Debug.LogFormat("waitBeat : {0}", waitBeat);
                yield return new WaitWhile(() => _stageBgm.time <= _fourBeatSecond * waitBeat);
                _currentBeat = waitBeat;
            }
        }
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
                StartCoroutine(NoticeHit(noteList[i]));
            }

            yield return new WaitWhile(() => _stageBgm.time <= noteList[i]._genTime);
            _stage.OnNote(noteList[i]);
        }
    }
}
