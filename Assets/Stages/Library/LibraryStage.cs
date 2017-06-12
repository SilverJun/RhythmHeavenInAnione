using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

// 1 beat delta = 60.0f / 187.0f(BPM)

public class LibraryStage : AbstractStage {

    GameObject _monitor;
    GameObject _player;
    GameObject _book;

    // Use this for initialization
    void Start()
    {
        //_bpm = 190.77989f;
        _bpm = 189;
        //_startDelay = 0.33f;
        _startDelay = 0.0f;
        _fourBeatSecond = 1.0f / (_bpm / 60.0f);
        Debug.Log(_fourBeatSecond);
        //script read.
        ParseSource();

        _monitor = GameObject.Find("모니터");
        _player = GameObject.Find("캐릭터");
        _book = GameObject.Find("책");

        _stageBgm.Play();
        //StartCoroutine(PlayMetronome());
        StartCoroutine(StartStage());
    }

    void FixedUpdate()
    {
        // 눌렀을 시, 노트 체크.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckNoteTouch();
        }
    }

    IEnumerator PlayMetronome()
    {
        yield return new WaitForSeconds(_startDelay);
        while (true)
        {
            _metronome.Play();
            yield return new WaitForSeconds(_fourBeatSecond);
        }
    }

    /// Interface Implement.
    //public override void OnAction(float nBeat, Pattern pattern)
    //{
    //}

    public override void OnNote(string name, float beat, string type)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
        switch (name)
        {
            case "OneNotice":
                _book.GetComponent<Book>().SetOne();
                break;
            case "TwoNotice":
                _book.GetComponent<Book>().SetTwo();
                break;

            case "ThreeOneNotice":
                Debug.Log("여기!!! three 1");
                _book.GetComponent<Book>().SetThree();
                break;
            case "ThreeTwoNotice":
            case "ThreeThreeNotice":
                Debug.Log("여기!!! three 23");
                _book.GetComponent<Book>().SetThree();
                break;

            case "OneCheck":
            case "TwoCheck":
            case "ThreeOneCheck":
            case "ThreeTwoCheck":
            case "ThreeThreeCheck":

                break;
        }
    }

    public override void OnSuccess()
    {
        _monitor.GetComponent<Monitor>().SetO();
    }

    public override void OnFail()
    {
        _monitor.GetComponent<Monitor>().SetX();
    }
}
