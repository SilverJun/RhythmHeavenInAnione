using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

using RAS;

/// <summary>
/// 모든 스테이지의 기본이 되는 클래스입니다.
/// </summary>
public class AbstractStage : MonoBehaviour
{
    [SerializeField]
    private TextAsset _script;

    private Parser _parser;

    private bool _isTiming;

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
        _isTiming = false;

        _parser = new Parser(_script);
        _parser.ParseSource();
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

    
    /// <summary>
    /// 1. 스테이지 RAS에서 읽은 Action 메소드들을 준비.
    /// 2. Action리스트를 순회하면서 다음 실행할 노트를 미리 current Note에 저장
    /// 3. 해당 액션의 시간이 되면 액션 실행.
    /// 4. 2~3 반복.
    /// </summary>
    /// <returns>코루틴에서 사용할 열거자를 리턴</returns>
    public IEnumerator StartStage()
    {
        yield return new WaitForSeconds(_startDelay);
        //_timer.Start();

        foreach (var item in _parser._actions)
        {
            var time = BeatToRealMillisecond(item.Key);

            // 액션 실행.

            // 패턴 시작 전까지 대기.
            while (time > _stageBgm.time * 1000.0f)
            {
                yield return new WaitForEndOfFrame();
            }

            // OnAction 실행.
            OnAction(item.Key, item.Value);

            var noteGenTime = 0.0f;
            // 패턴이 시작되면 해당하는 노트를 실행.
            foreach (var note in item.Value._noteSetting)
            {
                OnNote(note._name, note._beat, note._type);

                _isTiming = (note._type != "Notice");
                if (_isTiming)
                    _totalNote++;
                noteGenTime += BeatToRealMillisecond(note._beat);
                while (time + noteGenTime > _stageBgm.time * 1000.0f)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (_isTiming)
                    OnFail();
            }
        }

        yield return StartCoroutine(GoEndScene());
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

    /// <summary>
    /// 판정 체크 함수.
    /// </summary>
    /// <returns>판정에 맞으면 true를 반환</returns>
    public void CheckNoteTouch()
    {
        if (_isTiming)
        {
            _isTiming = false;
            OnSuccess();
        }
        else
        {
            OnFail();
        }
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
