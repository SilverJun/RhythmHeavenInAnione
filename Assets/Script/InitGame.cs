using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        // 사용자 스테이지 클리어 등 정보 읽기.
	    var stageInfoJsonPath = Path.Combine(Application.persistentDataPath, "StageInfo.json");

	    Debug.Log(stageInfoJsonPath);
	    if (!File.Exists(stageInfoJsonPath))    // 파일이 없으면 (처음 앱을 깔면) 에셋에 넣어둔 초기 json 내용 복사후 생성.
	    {
	        TextAsset initJsonFile = (TextAsset)Resources.Load("StageInfo");
            Debug.Log(initJsonFile.text);
            File.WriteAllText(stageInfoJsonPath, initJsonFile.text);
        }

        MenuInitializer._isFirstStart = true;
        MenuInitializer._initStageName = "StartEffect";
        SceneManager.LoadScene("MainSplash");
	}
}
