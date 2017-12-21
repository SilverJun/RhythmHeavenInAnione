using System;
using System.Collections;
using UnityEngine;

public class MenuInitializer : MonoBehaviour
{
    // Start에서 초기 스테이지로 설정할 UI.
    public static bool _isFirstStart = true;
    [SerializeField]
    private string _editOnlyInitStage = "";
    public static string _initStageName = "";
    
    void Start()
    {
        //if (_initStageName == "")
        //{
        //    _initStageName = "StartEffect";
        //    _isFirstStart = true;
        //}
        //else
        //{
        //    _isFirstStart = false;
        //}

        // For Debug, Test With Unity Editer.
        if (_editOnlyInitStage != "")
        {
            _initStageName = _editOnlyInitStage;
            _isFirstStart = false;
        }

        if (_isFirstStart)
        {
            Instantiate(Resources.Load("Prefab/Splash") as GameObject);
            _isFirstStart = false;
        }
        else
        {
            Instantiate(Resources.Load("Prefab/FadeIn") as GameObject);
        }

        UIManager.OpenUI<UI>(Resources.Load("Prefab/" + _initStageName) as GameObject);
        Destroy(gameObject);
    }
}
