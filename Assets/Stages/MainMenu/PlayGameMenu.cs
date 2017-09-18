using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameMenu : Menu
{
    protected override void Start()
    {
        base.Start();
        UIHelper.AddButtonListener(Vars["menu1"], () => AnimHide(ShowStageMenu));
        UIHelper.AddButtonListener(Vars["back"], () => AnimHide(ShowBackMenu));
    }

    void ShowStageMenu()
    {
        UIManager.OpenUI<SchoolMenu>("Prefab/SchoolMenu");
    }

    void ShowBackMenu()
    {
        UIManager.OpenUI<MainMenu>("Prefab/MainMenu");
    }
}
