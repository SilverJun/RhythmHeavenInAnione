using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Menu : UI
{
    protected Sequence _menuDown;
    protected Sequence _menuHighlight;
    protected Sequence _menuHide;

    protected virtual void Start()
    {
        AnimMenuDown();
    }

    protected void AnimMenuDown()
    {
        _menuDown = DOTween.Sequence();
        _menuDown.Append(GetComponent<RectTransform>().DOMoveY(GetComponent<RectTransform>().position.y - 10.0f, 1.0f));
        _menuDown.SetEase(Ease.OutExpo);
        _menuDown.AppendCallback(AnimMenuHighlight);
        _menuDown.Play();
    }

    protected void AnimMenuHighlight()
    {
        _menuHighlight = DOTween.Sequence();
        _menuHighlight.Append(Vars["menu1"].GetComponent<RectTransform>().DOScale(new Vector3(1.3f, 1.3f, 1.0f), 1.0f));
        _menuHighlight.Append(Vars["menu1"].GetComponent<RectTransform>().DOScale(Vector3.one, 1.0f));
        _menuHighlight.Append(Vars["menu2"].GetComponent<RectTransform>().DOScale(new Vector3(1.3f, 1.3f, 1.0f), 1.0f));
        _menuHighlight.Append(Vars["menu2"].GetComponent<RectTransform>().DOScale(Vector3.one, 1.0f));
        _menuHighlight.Append(Vars["menu3"].GetComponent<RectTransform>().DOScale(new Vector3(1.3f, 1.3f, 1.0f), 1.0f));
        _menuHighlight.Append(Vars["menu3"].GetComponent<RectTransform>().DOScale(Vector3.one, 1.0f));
        _menuHighlight.SetLoops(-1);
        _menuHighlight.Play();
    }

    protected void AnimHide(Action callback)
    {
        _menuHide = DOTween.Sequence();
        _menuHide.Append(GetComponent<RectTransform>().DOMoveY(GetComponent<RectTransform>().position.y + 10.0f, 1.0f));
        _menuHide.SetEase(Ease.InExpo);
        _menuHide.AppendCallback(() => UIManager.CloseUI(this));
        _menuHide.AppendCallback(() => callback());
        _menuHide.Play();
    }
}
