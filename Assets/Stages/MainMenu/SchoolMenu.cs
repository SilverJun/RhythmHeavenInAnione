public class SchoolMenu : Menu
{
    protected override void Start()
    {
        base.Start();
        UIHelper.AddButtonListener(Vars["menu1"], () => ShowStageInfo("Library"));
        UIHelper.AddButtonListener(Vars["menu2"], () => ShowStageInfo("Auditorium"));
        UIHelper.AddButtonListener(Vars["menu3"], () => ShowStageInfo("Class"));
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
