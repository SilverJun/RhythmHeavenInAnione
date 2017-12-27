public class PlayGameMenu : Menu
{
    protected override void Start()
    {
        base.Start();
        UIHelper.AddButtonListener(Vars["menu1"], () => AnimHide(() => ShowStageMenu("SchoolMenu")));
        UIHelper.AddButtonListener(Vars["menu2"], () => AnimHide(() => ShowStageMenu("DormitoryMenu")));
        UIHelper.AddButtonListener(Vars["menu3"], () => AnimHide(() => ShowStageMenu("ExteriorMenu")));
        UIHelper.AddButtonListener(Vars["back"], () => AnimHide(ShowBackMenu));
    }

    void ShowStageMenu(string menu)
    {
        UIManager.OpenUI<Menu>("Prefab/" + menu);
    }

    void ShowBackMenu()
    {
        UIManager.OpenUI<MainMenu>("Prefab/MainMenu");
    }
}
