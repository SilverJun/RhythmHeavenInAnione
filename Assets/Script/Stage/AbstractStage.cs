using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

using RAS;

/// <summary>
/// 모든 스테이지의 기본이 되는 클래스입니다.
/// </summary>
public class AbstractStage : MonoBehaviour
{
    // RAS
    [SerializeField]
    private TextAsset _script;
    private Parser _parser;
    //

    // Stage, Judgement
    //private bool _isTiming;
    private Note _currentNote;

    private List<Note> _stageNostes = new List<Note>();
    //

    public AudioSource _stageBgm;
    public AudioSource _metronome;
    
    protected float _startDelay;
    protected float _bpm;
    protected float _fourBeatSecond;
    protected int _totalNote;
    protected int _hitNote;

    private void Awake()
    {
        _stageBgm.playOnAwake = false;
        _metronome.playOnAwake = false;

        _parser = new Parser(_script);
        _parser.ParseSource();

        _totalNote = _parser._actions.Values.Sum((x) => x._noteSetting.Count((y) => y._type != "Notice"));
    }

    public static float GetBeat(string str)
    {
        string beatString = str;

        // 기본 박자 => 4 / 비트 => 4분음표는 1, 8분음표는 0.5
        float beat = 4 / float.Parse(beatString[0].ToString());

        if (beatString.Length == 3)
        {
            // 셋잇단음표 구분
            // 4.3 => 4분음표의 셋잇단음표 한개. 3개가 나와야 함
            // 4분음표가 1박이면 0.6666박.
            if (beatString[0] == '4' && beatString[2] != '3')
            {
                beat += 0.666666f;
            }
            // 점 구분
            // 4.5 => 점 4분음표.
            else if (beatString[2] != '5')
            {
                beat += beat / 2.0f;
            }
        }

        return beat;
    }

    protected float BeatToRealMillisecond(float beat)
    {
        return beat *_fourBeatSecond * 1000.0f;
    }

    private void InitNotes()
    {
        foreach (var action in _parser._actions)
        {
            var genTime = BeatToRealMillisecond(action.Key);
            float offsetTime = 0.0f;
            foreach (var note in action.Value._noteSetting)
            {
                _stageNostes.Add(new Note(genTime + offsetTime, note._type, note._name));
                offsetTime += BeatToRealMillisecond(note._beat);
            }
        }

        Debug.Log("InitNote End!");
    }
    
    /// <summary>
    /// 1. 스테이지 RAS에서 읽은 Action 메소드들을 준비.
    /// 2. Action리스트를 순회하면서 다음 실행할 노트를 미리 current Note에 저장
    /// 3. 해당 액션의 시간이 되면 액션 실행.
    /// 4. 2~3 반복.
    /// </summary>
    public IEnumerator StartStage()
    {
        InitNotes();
        yield return new WaitForSeconds(_startDelay);
        StartCoroutine(CheckHit());
        
        foreach (var note in _stageNostes)
        {
            _currentNote = note;
            // 패턴 시작 전까지 대기.
            while (note._genTime > _stageBgm.time * 1000.0f)
            {
                yield return new WaitForEndOfFrame();
            }

            OnNote(note._noteName, note._genTime, note._type.ToString());

            yield return new WaitForSeconds(0.06f);

            if (_currentNote._type != NoteType.Notice && _currentNote._isHit == false)
                OnFail();
        }

        yield return StartCoroutine(GoEndScene());
    }

    IEnumerator CheckHit()
    {
        while (_stageBgm.isPlaying)
        {
            yield return new WaitUntil(() => TouchManager.IsTouch || TouchManager.IsSwipe);

            var currentSecond = _stageBgm.time * 1000.0f;
            
            if (_currentNote._genTime - 330.0f <= currentSecond && currentSecond <= _currentNote._genTime + 330.0f)
            {
                Debug.Log("Hit Success!");
                Debug.LogFormat("{0}, {1}", _currentNote._genTime, currentSecond);
                OnSuccess();
                _currentNote._isHit = true;
            }
            else
            {
                Debug.Log("Hit Error Fail!");
                Debug.LogFormat("{0}, {1}", _currentNote._genTime, currentSecond);
                OnFail();
                _currentNote._isHit = false;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public virtual void OnAction(float nBeat, Pattern pattern)
    {
        // Debug.LogFormat("{0}:{1}패턴 실행", nBeat, pattern._name);
    }
    
    public virtual void OnNote(string name, float beat, string type)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
    }

    public virtual void OnSuccess()
    {
    }

    public virtual void OnFail()
    {
    }

    IEnumerator GoEndScene()
    {
        StageScoreInfo.SetScore(_hitNote, _totalNote);
        yield return new WaitForSeconds(2.0f);
        Instantiate(Resources.Load("Prefab/FadeOut") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadSceneAsync("EndStage");
    }
}
