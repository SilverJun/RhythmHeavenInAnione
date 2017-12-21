using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStart : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    Instantiate(Resources.Load("Prefab/Start/" + StageManager.Instance._currentStageName + "Start"));
	}
}
