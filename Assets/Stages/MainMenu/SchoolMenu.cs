using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SchoolMenu : Menu
{
    protected override void Start()
    {
        base.Start();
        UIHelper.AddButtonListener(Vars["menu1"], () => ShowStageInfo("Library"));
        UIHelper.AddButtonListener(Vars["back"], () => AnimHide(ShowBackMenu));
    }

    void ShowStageInfo(string stageName)
    {
        StageInfoUI._selectedStage = stageName;
        UIManager.OpenUI<StageInfoUI>("Prefab/StageInfoUI");
    }

    void ShowBackMenu()
    {
        UIManager.OpenUI<PlayGameMenu>("Prefab/PlayGameMenu");
    }
}
