using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractButton : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private System.Action _callBack;

    void Start()
	{
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	    _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    public void SetClickCallBack(System.Action callBack)
    {
        _callBack = callBack;
        StartCoroutine(SetButtonOn());
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    void OnMouseUp()
    {
        if (_callBack != null)
        {
            _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _callBack();
        }
    }

    IEnumerator SetButtonOn()
    {
        yield return new WaitWhile(()=> _spriteRenderer == null);
        _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
