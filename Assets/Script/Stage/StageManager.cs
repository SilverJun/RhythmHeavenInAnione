using System.IO;
using System.Collections;
using JSONForm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageManager : MonoBehaviour
{
    // singleton
    private static StageManager _instance;
    public static StageManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (StageManager)FindObjectOfType(typeof(StageManager));
                if (!_instance)
                {
                    GameObject stageManagerObject = Resources.Load<GameObject>("Prefab/StageManager");
                    _instance = Instantiate(stageManagerObject).GetComponent<StageManager>();
                    _instance.Init();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    //
    public string _stageScript { get; set; }
    public StageInfo _stageInfo { get; set; }
    private JObject _stageInfoJson { get; set; }
    public string _currentStageName { get; set; }
    public AudioClip _stageBgm { get; set; }
    public BaseStage _baseStage { get; set; }
    public UI _pauseUI { get; set; }

    public AudioSource _mainBgSource { get; set; }
    public float _bgVolume {
        get
        {
            return Instance.__bgVolume;
        }
        set
        {
            Instance.__bgVolume = value;

            if (_mainBgSource == null)
            {
                _mainBgSource = GameObject.Find("backgroundSound").GetComponent<AudioSource>();   
            }
            if (_mainBgSource != null)
            {
                _mainBgSource.volume = Instance.__bgVolume;
            }
        }
    }
    private float __bgVolume;
    public float _fxVolume { get; set; }
    //

    void Init()
    {
        // Load json
        var stageInfoJsonPath = Path.Combine(Application.persistentDataPath, "StageInfo.json");

        Debug.Log(stageInfoJsonPath);
        if (!File.Exists(stageInfoJsonPath)) { Debug.LogException(new FileNotFoundException("StageInfo.json 파일이 존재하지 않습니다!")); }

        var jsonText = File.ReadAllText(stageInfoJsonPath);
        Debug.Log(jsonText);
        _instance._stageInfoJson = JObject.Parse(jsonText);

        _bgVolume = 0.8f;
        _fxVolume = 1.0f;
    }

    public void SetStage(string stageName)
    {
        _currentStageName = stageName;
        SceneManager.LoadScene("AbstractStart");
        _stageScript = ((TextAsset)Resources.Load("RAS/" + _currentStageName + "Stage")).text;
        _stageBgm = (AudioClip)Resources.Load("Sound/StageBgm/" + _currentStageName + "Stage");
    }

    public void SetTutorial(string stageName)
    {
        _currentStageName = stageName;
        SceneManager.LoadScene("AbstractTutorial");
        _stageScript = ((TextAsset)Resources.Load("RAS/" + _currentStageName + "Stage")).text;
    }

    public void UpdateStageInfo(string status, float percent)
    {
        _stageInfoJson[_currentStageName]["StageStatus"] = status;
        _stageInfoJson[_currentStageName]["Percent"] = percent;

        // Save json
        var stageInfoJsonPath = Path.Combine(Application.persistentDataPath, "StageInfo.json");
        File.WriteAllText(stageInfoJsonPath, _stageInfoJson.ToString());
    }

    public StageInfo GetStageInfo(string stageName)
    {
        return JsonConvert.DeserializeObject<StageInfo>(Instance._stageInfoJson[stageName].ToString());
    }

    public void OpenPauseUI()
    {
        if (_baseStage == null)
        {
            _baseStage = FindObjectOfType<BaseStage>();
        }

        Time.timeScale = 0.0f;
        _baseStage._stageBgm.Pause();
        _pauseUI = UIManager.OpenUI<PauseUI>(Resources.Load<GameObject>("Prefab/PauseUI"));
    }

    public void ClosePauseUI()
    {
        Time.timeScale = 1.0f;
        _baseStage._stageBgm.UnPause();
        UIManager.CloseUI(_pauseUI);
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1.0f;
        _baseStage.ExitStage();
    }
}
