using System;
using System.Collections;
using UnityEngine;

public class MenuInitializer : MonoBehaviour
{
    // Start에서 초기 스테이지로 설정할 UI.
    public static bool _isFirstStart = true;
    [SerializeField]
    private string _editOnlyInitStage = "";
    public static string _initStageName = "StartEffect";
    
    void Start()
    {
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

        UIManager.OpenUI<StartEffect>(Resources.Load("Prefab/" + _initStageName) as GameObject);
        Destroy(gameObject);
    }
}
