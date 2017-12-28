using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public string StageName;

	void Start () {
		StageManager.Instance.SetStage(StageName);
	}
	
}
