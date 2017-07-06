using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Animator _anim;
    private AbstractButton[] _menuScript = new AbstractButton[3];

    private MainSceneManager _mainSceneManager;

    void Start ()
    {
        _mainSceneManager = GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>();

	    _anim = GetComponent<Animator>();

        Transform[] AllData = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < 3; i++)
	    {
	        foreach (Transform Obj in AllData)
	        {
	            if (Obj.name == "menu" + (i + 1))
	            {
	                _menuScript[i] = Obj.gameObject.GetComponent<AbstractButton>();
                }
	        }
	    }

	    _menuScript[0].SetClickCallBack(() =>
	    {
            _anim.SetTrigger("Hide");
	        _mainSceneManager.SetSubMenu(MainSceneManager.MenuNumber.PlayGame);
        });
    }

    GameObject GetChildObj(string strName)
    {
        Transform[] AllData = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform Obj in AllData)
        {
            if (Obj.name == strName)
            {
                return Obj.gameObject;
            }
        }
        return null;
    }
}
