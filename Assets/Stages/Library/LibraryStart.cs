using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LibraryStart : MonoBehaviour {

    IEnumerator FadeOutEffect()
    {
        Instantiate(Resources.Load("Prefab/FadeOut") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadSceneAsync("LibraryStage");
    }

    public void StartStage()
    {
        StartCoroutine(FadeOutEffect());
    }
}
