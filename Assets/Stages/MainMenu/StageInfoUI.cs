using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JSONForm;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageInfoUI : UI
{
    public static string _selectedStage;
    private StageInfo _stageInfo;

    void Start()
    {
        SetupUI();
        ShowAnim();
    }

    void SetupUI()
    {
        _stageInfo = StageManager.Instance.GetStageInfo(_selectedStage);
        //string status = _stageInfo.GetStageStatusString();
        Vars["stageImage"].GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/UI/" + _stageInfo.StageName);
        Vars["stageImage"].GetComponent<Image>().preserveAspect = true;
        Vars["statusText"].GetComponent<Text>().text = _stageInfo.StageStatus;
        Vars["InfoText"].GetComponent<Text>().text = _stageInfo.StageIntro;
    }

    void ShowAnim()
    {
        Sequence seq = DOTween.Sequence();
        var rectTransform = GetComponent<RectTransform>();
        seq.SetEase(Ease.OutBounce);
        seq.Append(rectTransform.DOMoveX(0.0f, 1.0f));
        seq.Play();
    }

    public void BackAnim()
    {
        Sequence seq = DOTween.Sequence();
        var rectTransform = GetComponent<RectTransform>();
        seq.SetEase(Ease.InExpo);
        seq.Append(rectTransform.DOMoveX(-700.0f, 1.0f));
        seq.AppendCallback(() =>
        {
            UIManager.CloseUI(this);
        });
        seq.Play();
    }

    public void StartStage()
    {
        StartCoroutine(_StartStage());
    }

    IEnumerator _StartStage()
    {
        Instantiate(Resources.Load("Prefab/FadeOut") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LibraryStart");
    }
}
