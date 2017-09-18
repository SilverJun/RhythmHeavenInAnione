using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartEffect : UI {
    
	void Start ()
	{
        AnimSplash();
	}

    void AnimSplash()
    {
        var anione = DOTween.Sequence();
        anione.Append(Vars["AnioneLogo"].GetComponent<Image>().DOFade(1.0f, 1.5f));
        anione.Append(Vars["AnioneLogo"].GetComponent<Image>().DOFade(0.0f, 0.5f));

        var team = DOTween.Sequence();
        team.Append(Vars["TeamLogo"].GetComponent<Image>().DOFade(1.0f, 1.5f));
        team.Append(Vars["TeamLogo"].GetComponent<Image>().DOFade(0.0f, 0.5f));

        var background = DOTween.Sequence();
        background.Append(Vars["background"].GetComponent<Image>().DOFade(1.0f, 1.5f));
        background.Append(Vars["background"].GetComponent<Image>().DOFade(0.0f, 0.5f));
        background.AppendCallback(() => UIManager.CloseUI(this));

        anione.Play();
        team.Play();
        background.Play();
    }
}
