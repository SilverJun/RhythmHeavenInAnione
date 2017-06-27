using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    private bool _isTouched = false;
    private Vector3 _touchPosition;
    private float swipeResistance = 100.0f;
    private GameObject _info;

    private Rigidbody2D _rigid;

	// Use this for initialization
	void Start ()
	{
	    _info = GameObject.Find("Info");
	    _rigid = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        // 타이틀 위로 슬라이드되서 안보일때.
        if (transform.position.y > Camera.main.rect.yMax + 10.0f)
        {
            SceneManager.LoadScene("LibraryStage");
        }
    }

    void FixedUpdate()
    {
        if (_isTouched)
        {
            _rigid.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * 1000.0f);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("마우스 누름");
        _isTouched = true;
        _touchPosition = Input.mousePosition;

        _info.GetComponent<Info>().SwapSprite();
    }

    void OnMouseUp()
    {
        Debug.Log("마우스 땜");
        _isTouched = false;

        Vector3 lastPosition = Input.mousePosition;
        Vector3 deltaSwipe = _touchPosition - lastPosition;
        _touchPosition = Input.mousePosition;

        if (_rigid.velocity.y > 10.0f)
        {
            Debug.Log("다음 스테이지 이동!");
            _rigid.drag = 0.0f;
        }
        else
        {
            Debug.Log("힘이 이것밖에 안됩니까??");
            _rigid.drag = 20.0f;
            _rigid.position = new Vector2(0.0f, 0.0f);
        }

        _info.GetComponent<Info>().SwapSprite();
    }
}
