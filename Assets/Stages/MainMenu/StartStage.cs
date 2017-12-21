using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartStage : MonoBehaviour
{
    IEnumerator FadeOutEffect()
    {
        Instantiate(Resources.Load("Prefab/FadeOut") as GameObject);
        yield return new WaitForSeconds(1.0f);
        //SceneManager.LoadSceneAsync(StageManager.Instance._currentStageName + "Stage");
        SceneManager.LoadScene("AbstractStage");
    }

    public void StartCallback()
    {
        StartCoroutine(FadeOutEffect());
    }
}

