using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    UIManager.OpenUI<StartEffect>(Resources.Load("Prefab/StartEffect") as GameObject);
	    Instantiate(Resources.Load("Prefab/Splash") as GameObject);
    }
}
