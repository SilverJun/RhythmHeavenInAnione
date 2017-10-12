using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    private bool _isTouched = false;
    private bool _isSplash = true;
    private GameObject _info;
    private GameObject _arrow;
    private Rigidbody2D _rigid;

	// Use this for initialization
	void Start ()
	{
	    _info = GameObject.Find("Info"); 
	    _arrow = GameObject.Find("Arrow");

        PlayArrowAnimation();

	    _rigid = GetComponent<Rigidbody2D>();
	}

    private void PlayArrowAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_arrow.transform.DOMoveY(_arrow.transform.position.y + 0.5f, 1.0f));
        seq.Append(_arrow.transform.DOMoveY(_arrow.transform.position.y, 1.0f));
        seq.SetLoops(-1);
        seq.Play();
    }

    void Update()
    {
        // 타이틀 위로 슬라이드되서 안보일때.
        if (_isSplash && transform.position.y > Camera.main.rect.yMax + 10.0f)
        {
            GameObject.Find("Splash(Clone)").GetComponent<StartSplash>().DestroySplash();
        }

        if (IsHitByMouse())
        {
            _isTouched = true;

            _arrow.SetActive(false);
            _info.GetComponent<Info>().SwapSprite();
        }

        if (Input.GetMouseButtonUp(0) && _isTouched)
        {
            MouseUpEvent();
        }
    }

    void FixedUpdate()
    {
        if (_isSplash && _isTouched)
        {
            _rigid.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * 1000.0f);
        }
    }
    
    public bool IsEnd()
    {
        return _isSplash;
    }

    bool IsHitByMouse()
    {
        if (!Input.GetMouseButtonDown(0))
            return false;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        return hit.collider != null && hit.collider.gameObject == this.gameObject;
    }

    void MouseUpEvent()
    {
        if (!_isSplash && !_isTouched) { return; }

        _isTouched = false;

        if (_rigid.velocity.y > 20.0f)
        {
            _rigid.drag = 0.0f;
            _arrow.SetActive(false);
            _info.SetActive(false);
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
}
