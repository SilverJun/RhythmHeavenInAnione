using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameMenu : MonoBehaviour
{

    private MainSceneManager _mainSceneManager;
    private Animator _anim;
    private AbstractButton _back;
    private AbstractButton[] _menuScript = new AbstractButton[3];

    // Use this for initialization
    void Start ()
    {
        _mainSceneManager = GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>();

        _anim = GetComponent<Animator>();

        Transform[] AllData = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < 3; i++)
        {
            foreach (Transform Obj in AllData)
            {
                if (Obj.name == "menu" + (i + 1))
                {
                    _menuScript[i] = Obj.gameObject.GetComponent<AbstractButton>();
                }
                else if (Obj.name == "뒤로가기")
                {
                    _back = Obj.gameObject.GetComponent<AbstractButton>();
                }
            }
        }

        _menuScript[0].SetClickCallBack(() =>
        {
            _anim.SetTrigger("Hide");
            _mainSceneManager.SetStageMenu(MainSceneManager.MenuNumber.SchoolMenu);
        });

        _back.SetClickCallBack(() =>
        {
            _anim.SetTrigger("Hide");
            _mainSceneManager.SetMainMenu();
        });
    }
}
