using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    private bool _isTouched = false;
    private bool _isSplash = true;
    private GameObject _info;
    private GameObject _arrow;

    private Rigidbody2D _rigid;
    private Animator _anim;

	// Use this for initialization
	void Start ()
	{
	    _info = GameObject.Find("Info");
	    _arrow = GameObject.Find("화살표");
        
        _rigid = GetComponent<Rigidbody2D>();
	    _anim = GetComponent<Animator>();
	}

    void Update()
    {
        // 타이틀 위로 슬라이드되서 안보일때.
        if (_isSplash && transform.position.y > Camera.main.rect.yMax + 10.0f)
        {
            _isSplash = false;
        }
    }

    void FixedUpdate()
    {
        if (_isSplash && _isTouched)
        {
            _rigid.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * 1000.0f);
        }
    }

    void OnMouseDown()
    {
        _isTouched = true;

        _arrow.SetActive(false);
        _info.GetComponent<Info>().SwapSprite();
    }

    void OnMouseUp()
    {
        if (!_isSplash) { return; }
        
        _isTouched = false;
        
        if (_rigid.velocity.y > 20.0f)
        {
            _rigid.drag = 0.0f;
        }
        else
        {
            _rigid.velocity = new Vector2(0.0f, 0.0f);
            _rigid.position = new Vector2(0.0f, 0.0f);
            _rigid.drag = 20.0f;
            _arrow.SetActive(true);
        }

        _info.GetComponent<Info>().SwapSprite();
    }

    public bool IsEnd()
    {
        return _isSplash;
    }
}
