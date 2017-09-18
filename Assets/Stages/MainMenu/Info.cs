using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    private bool _nowTouchPlease = true;
    public Sprite _touch;
    public Sprite _slide;

    public void SwapSprite()
    {
        GetComponent<SpriteRenderer>().sprite = _nowTouchPlease ? _slide : _touch;
        _nowTouchPlease = !_nowTouchPlease;
    }
}
