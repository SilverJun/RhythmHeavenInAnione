using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

// 1 beat delta = 60.0f / 187.0f(BPM)

public class LibraryStage : AbstractStage {

    GameObject _monitor;
    GameObject _player;
    GameObject _book;

    public AudioSource _AudioSource = new AudioSource();
    public AudioClip _One;
    public AudioClip _Two;
    public AudioClip _TwoNote;
    public AudioClip _tOne;
    public AudioClip _tTwo;
    public AudioClip _tThree;
    public AudioClip _tFour;
    //public AudioClip _Success;

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
                _AudioSource.PlayOneShot(_One);
                _book.GetComponent<Book>().SetOne();
                break;
            case "TwoNotice":
                _AudioSource.PlayOneShot(_Two);
                _book.GetComponent<Book>().SetTwo();
                break;

            case "ThreeOneNotice":
                _AudioSource.PlayOneShot(_tOne);
                _book.GetComponent<Book>().SetThree();
                break;
            case "ThreeTwoNotice":
                _book.GetComponent<Book>().SetThree();
                break;
            case "ThreeThreeNotice":
                _book.GetComponent<Book>().SetThree();
                break;
            case "TwoCheck":
                _AudioSource.PlayOneShot(_TwoNote);
                break;
            case "OneCheck":
                _AudioSource.PlayOneShot(_One);
                break;
            case "ThreeOneCheck":
                _AudioSource.PlayOneShot(_tTwo);
                break;
            case "ThreeTwoCheck":
                _AudioSource.PlayOneShot(_tThree);
                break;
            case "ThreeThreeCheck":
                _AudioSource.PlayOneShot(_tFour);
                break;
        }
    }

    public override void OnSuccess()
    {
        //_AudioSource.PlayOneShot(_Success);
        _monitor.GetComponent<Monitor>().SetO();
    }

    public override void OnFail()
    {
        Debug.Log("실패!!");
        _monitor.GetComponent<Monitor>().SetX();
    }
}
