using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartSplash : MonoBehaviour
{
    public GameObject _logo;
    public GameObject _arrow;
    private Sequence arrowSequence;
    public GameObject _info;
    private Sequence infoSequence;

    // Use this for initialization
    void Start ()
    {
        AnimArrow();
        AnimInfo();
    }

    void AnimArrow()
    {
        arrowSequence = DOTween.Sequence();
        arrowSequence.Append(_arrow.transform.DOMoveY(_arrow.transform.position.y + 0.5f, 1.0f));
        arrowSequence.Append(_arrow.transform.DOMoveY(_arrow.transform.position.y, 1.0f));
        arrowSequence.SetLoops(-1);
        arrowSequence.Play();
    }

    void AnimInfo()
    {
        infoSequence = DOTween.Sequence();
        infoSequence.Append(_info.GetComponent<SpriteRenderer>().DOFade(0.2f, 1.0f));
        infoSequence.Append(_info.GetComponent<SpriteRenderer>().DOFade(1.0f, 1.0f));
        infoSequence.SetEase(Ease.InOutQuart);
        infoSequence.SetLoops(-1);
        infoSequence.Play();
    }

    public void DestroySplash()
    {
        Destroy(_logo);
        Destroy(gameObject);
        UIManager.OpenUI<MainMenu>("Prefab/MainMenu");
    }
}
