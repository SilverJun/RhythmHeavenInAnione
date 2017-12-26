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
    }

    void ShowStageMenu()
    {
        UIManager.OpenUI<PlayGameMenu>("Prefab/PlayGameMenu");
    }
}
