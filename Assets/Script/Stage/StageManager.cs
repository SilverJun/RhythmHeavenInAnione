using System.Collections.Generic;
using System.Linq;
using JSONForm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


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
                    TextAsset json = (TextAsset)Resources.Load("StageInfo");
                    _instance._stageInfoJson = JObject.Parse(json.text);
                }
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    //
    public TextAsset _stageScript { get; set; }
    public StageInfo _stageInfo { get; set; }
    private JObject _stageInfoJson { get; set; }
    //

    //public void SetStage(TextAsset stageScript)
    //{
    //    SceneManager.LoadScene("AbstractStage");
    //    var baseStage = (BaseStage)FindObjectOfType(typeof(BaseStage));

    //    baseStage.Init();
    //}

    public StageInfo GetStageInfo(string stageName)
    {
        // TODO : 파일 도큐먼트에서 가져오기.

        return JsonConvert.DeserializeObject<StageInfo>(Instance._stageInfoJson[stageName].ToString());
    }

}