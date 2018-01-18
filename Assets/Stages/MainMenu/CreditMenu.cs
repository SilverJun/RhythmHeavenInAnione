using DG.Tweening;
using System;
using UnityEngine;

public class CreditMenu : UI
{
    protected Sequence _menuDown;
    protected Sequence _menuHighlight;
    protected Sequence _menuHide;

    void Start()
    {
        AnimMenuDown();
        UIHelper.AddButtonListener(Vars["back"], () => AnimHide(ShowBackMenu));
    }

    void AnimMenuDown()
    {
        _menuDown = DOTween.Sequence();
        _menuDown.Append(GetComponent<RectTransform>().DOMoveY(0.0f, 1.0f));
        _menuDown.SetEase(Ease.OutExpo);
        _menuDown.Play();
    }
    
    void AnimHide(Action callback)
    {
        _menuHide = DOTween.Sequence();
        _menuHide.Append(GetComponent<RectTransform>().DOMoveY(GetComponent<RectTransform>().position.y + 10.0f, 1.0f));
        _menuHide.SetEase(Ease.InExpo);
        _menuHide.AppendCallback(() => UIManager.CloseUI(this));
        _menuHide.AppendCallback(() => callback());
        _menuHide.Play();
    }

    void ShowBackMenu()
    {
        UIManager.OpenUI<MainMenu>("Prefab/MainMenu");
    }
}
