using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class FadeOut : UI
{
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(GetComponent<Image>().DOFade(1.0f, 1.0f));
        seq.Play();
    }
}
