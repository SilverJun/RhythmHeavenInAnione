public class DormitoryMenu : Menu
{
    protected override void Start()
    {
        base.Start();
        UIHelper.AddButtonListener(Vars["menu1"], () => ShowStageInfo("Room"));
        UIHelper.AddButtonListener(Vars["menu2"], () => ShowStageInfo("Store"));
        UIHelper.AddButtonListener(Vars["menu3"], () => ShowStageInfo("PERoom"));
        UIHelper.AddButtonListener(Vars["back"], () => AnimHide(ShowBackMenu));
    }

    void ShowStageInfo(string stageName)
    {
        StageManager.Instance._currentStageName = stageName;
        UIManager.OpenUI<StageInfoUI>("Prefab/StageInfoUI");
    }

    void ShowBackMenu()
    {
        UIManager.OpenUI<PlayGameMenu>("Prefab/PlayGameMenu");
    }
}
