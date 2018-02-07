using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStart : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var startAnim = Instantiate(Resources.Load<GameObject>("Prefab/Start/" + StageManager.Instance._currentStageName + "Start"));
	    StartCoroutine(PlayStartAnim(startAnim));
	}

    IEnumerator PlayStartAnim(GameObject startAnim)
    {
        yield return new WaitForSeconds(0.5f);
        startAnim.GetComponent<Animator>().SetTrigger("Play");
    }
}
