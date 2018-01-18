using System;
using System.Collections;
using UnityEngine;

public class MenuInitializer : MonoBehaviour
{
    // Start에서 초기 스테이지로 설정할 UI.
    public static bool _isFirstStart = true;
    public static string _initStageName = "";
    
    void Start()
    {
        if (_isFirstStart)
        {
            Instantiate(Resources.Load("Prefab/Splash") as GameObject);
            UIManager.OpenUI<UI>(Resources.Load("Prefab/" + _initStageName) as GameObject);
            _isFirstStart = false;
        }
        else
        {
            UIManager.OpenUI<UI>(Resources.Load("Prefab/" + _initStageName) as GameObject);
            Instantiate(Resources.Load("Prefab/FadeIn") as GameObject);
        }

        Destroy(gameObject);
    }
}
