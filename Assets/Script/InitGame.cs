using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        // 사용자 스테이지 클리어 등 정보 읽기.
	    MenuInitializer._isFirstStart = true;
        MenuInitializer._initStageName = "StartEffect";
        SceneManager.LoadScene("MainSplash");
	}
}
