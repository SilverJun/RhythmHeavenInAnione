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
        UIHelper.AddButtonListener(Vars["menu1"], () => StartCoroutine(ShowMenu()));
        UIHelper.AddButtonListener(Vars["back"], () => AnimHide(ShowBackMenu));
    }

    IEnumerator ShowMenu()
    {
        UIManager.OpenUI<UI>("Prefab/FadeOut");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LibraryStart");
    }

    void ShowBackMenu()
    {
        UIManager.OpenUI<PlayGameMenu>("Prefab/PlayGameMenu");
    }
}
