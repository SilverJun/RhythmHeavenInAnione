using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenu : Menu
{
    protected override void Start()
    {
        _menuDown = DOTween.Sequence();
        _menuDown.Append(GetComponent<RectTransform>().DOMoveY(0.0f, 1.0f));
        _menuDown.SetEase(Ease.OutBounce);
        _menuDown.AppendCallback(AnimMenuHighlight);
        _menuDown.Play();
        
        UIHelper.AddButtonListener(Vars["menu1"], () => AnimHide(ShowStageMenu));
        UIHelper.AddButtonListener(Vars["menu2"], () => AnimHide(ShowCredit));
        UIHelper.AddButtonListener(Vars["menu3"], () => AnimHide(ShowSetting));
    }

    void ShowStageMenu()
    {
        UIManager.OpenUI<PlayGameMenu>("Prefab/PlayGameMenu");
    }
    
    void ShowCredit()
    {
        UIManager.OpenUI<CreditMenu>("Prefab/CreditMenu");
    }

    void ShowSetting()
    {
        UIManager.OpenUI<SettingMenu>("Prefab/SettingMenu");
    }
}
