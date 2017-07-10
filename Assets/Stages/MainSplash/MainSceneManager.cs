using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public enum MenuNumber
    {
        StartEffect = 0,
        Splash = 1,
        MainMenu = 2,
        PlayGame = 3,
        SchoolMenu = 4,
        FadeOut = 10
    }

    private List<GameObject> _prefab = new List<GameObject>();
    private GameObject _fadeOut;
    private GameObject _startEffect;
    private GameObject _splashSwipe;
    private GameObject _mainMenu;
    private GameObject _subMenu;
    private GameObject _stageMenu;

    // sub Menus
    private GameObject _playGameMenu;
    private GameObject _creditGameMenu;
    private GameObject _settingGameMenu;

    // stage Menus
    private GameObject _schoolMenu;
    
    

    void Awake()
    {
        _prefab.Add(Resources.Load("Prefab/StartEffect") as GameObject);
        _prefab.Add(Resources.Load("Prefab/Splash") as GameObject);
        _prefab.Add(Resources.Load("Prefab/MainMenu") as GameObject);
        _prefab.Add(Resources.Load("Prefab/PlayGame") as GameObject);
        _prefab.Add(Resources.Load("Prefab/SchoolMenu") as GameObject);

        _fadeOut = Resources.Load("Prefab/FadeOut") as GameObject;
    }

    void Start ()
    {
        _startEffect = Instantiate(_prefab[0], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        _splashSwipe = Instantiate(_prefab[1], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        StartCoroutine(StartEffectDestroy());
        StartCoroutine(SwipeToMainMenu());
    }

    public IEnumerator StartEffectDestroy()
    {
        var bg = GameObject.Find("Square");
        yield return new WaitWhile(() => bg.activeSelf);
        Destroy(_startEffect);
    }

    private IEnumerator SwipeToMainMenu()
    {
        var logoScript = GameObject.Find("SwipeLogo").GetComponent<Logo>();

        yield return new WaitWhile(() => logoScript.IsEnd());

        Destroy(_splashSwipe);
        _mainMenu = Instantiate(_prefab[2], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }

    public IEnumerator MainMenuDestroy()
    {
        var title = GameObject.Find("Title");
        yield return new WaitWhile(() => title.activeSelf);
        Destroy(_mainMenu);
    }

    public IEnumerator SubMenuDestroy()
    {
        var title = GameObject.Find("Title");
        yield return new WaitWhile(() => title.activeSelf);
        Destroy(_subMenu);
    }

    public IEnumerator StageMenuDestroy()
    {
        var title = GameObject.Find("Title");
        yield return new WaitWhile(() => title.activeSelf);
        Destroy(_stageMenu);
    }

    public IEnumerator StageChanger()
    {
        var title = GameObject.Find("Title");
        yield return new WaitWhile(() => title.activeSelf == false);
    }

    public void SetMainMenu()
    {
        StartCoroutine(SubMenuDestroy());
        _mainMenu = Instantiate(_prefab[2], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }

    public void SetSubMenu(MenuNumber menu)
    {
        if (_mainMenu != null)
            StartCoroutine(MainMenuDestroy());
        else if (_stageMenu != null)
            StartCoroutine(StageMenuDestroy());

        switch (menu)
        {
            case MenuNumber.PlayGame:
                _playGameMenu = Instantiate(_prefab[3], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                _subMenu = _playGameMenu;
                break;
        }
    }

    /// <summary>
    /// set stage menu ex> SchoolMenu.
    /// </summary>
    public void SetStageMenu(MenuNumber menu)
    {
        StartCoroutine(SubMenuDestroy());
        switch (menu)
        {
            case MenuNumber.SchoolMenu:
                _schoolMenu = Instantiate(_prefab[4], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                _stageMenu = _schoolMenu;
                break;
        }
    }

    public IEnumerator SetGameScene(String stageName)
    {
        Instantiate(_fadeOut, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadSceneAsync(stageName);
    }
}
