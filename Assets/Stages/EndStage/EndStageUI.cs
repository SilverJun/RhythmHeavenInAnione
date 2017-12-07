using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndStageUI : UI
{
    public int MaxScore;
    public int HitScore;
    public string CommentString;

    private GameObject Hit;
    private GameObject Max;
    private GameObject Comment;
    private GameObject Persent;
    private GameObject Button;

    private string[,] _commentStrings =
    {
        {
            "오! 리듬에 소질이 있군요!", "당신이 애니원 리듬스타!", "당신의 리듬에 감탄하고 갑니다!"
        },
        {
            "음.. 조금 아쉽네요", "봐줄만 하네요...", "간발의 차이!"
        },
        {
            "... 혹시 음소거는 아니죠?", "다시합시다...", "... 화이팅"
        }
    };

    void Start()
    {
        Hit = GameObject.Find("HitScore");
        Max = GameObject.Find("MaxScore");
        Comment = GameObject.Find("Comment");
        Persent = GameObject.Find("Persent");
        Button = GameObject.Find("BackButton");

        Hit.SetActive(false);
        Max.SetActive(true);
        Comment.SetActive(false);
        Persent.SetActive(false);
        Button.SetActive(false);

        StartCoroutine(ShowScore());
    }

    IEnumerator ShowScore()
    {
        Max.GetComponent<Text>().text += MaxScore.ToString();
        yield return new WaitForSeconds(1.0f);
        Hit.SetActive(true);
        Hit.GetComponent<Text>().text += HitScore.ToString();
        yield return new WaitForSeconds(1.0f);
        double per = Math.Round((double)HitScore / MaxScore * 100.0f, 1);
        Persent.GetComponent<Text>().text += per + "%";
        Persent.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Comment.SetActive(true);

        int n = UnityEngine.Random.Range(0, 2);
        if (per >= 80.0f)
        {
            CommentString = _commentStrings[0,n];
        }
        else if (per >= 60.0f)
        {
            CommentString = _commentStrings[1, n];
        }
        else
        {
            CommentString = _commentStrings[2, n];
        }

        Comment.GetComponent<Text>().text = CommentString;

        Button.SetActive(true);
    }

    public void SetScore(int hit, int max)
    {
        HitScore = hit;
        MaxScore = max;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainSplash");
    }
}
