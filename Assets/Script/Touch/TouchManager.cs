using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// 게임에서 사용되는 터치 관련 판정을 하는 클래스입니다.
/// </summary>
public class TouchManager : MonoBehaviour
{
    public static bool IsTouch { get; set; }
    public static bool IsSwipe { get; set; }
    public static Vector2 TouchedPos { get; set; }
    public static float SwipeLength = 5.0f;

    //public enum SwipeDir
    //{
    //    None,
    //    Up,
    //    Down,
    //    Left,
    //    Right
    //}

    //public static SwipeDir SwipeDirection = SwipeDir.None;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch tempTouchs;
            //터치가 1개 이상이면.
            for (int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if (tempTouchs.phase == TouchPhase.Began)
                {
                    //해당 터치가 시작됐다면.
                    TouchedPos = tempTouchs.position;
                    IsTouch = true;
                    StartCoroutine(TouchEvent());
                }
                else if (tempTouchs.phase == TouchPhase.Ended)
                {
                    Vector2 touchLength = TouchedPos - tempTouchs.position;
                    if (touchLength.magnitude >= SwipeLength)
                    {
                        IsSwipe = true;

                        //float swipeAngle = Mathf.Atan2(touchLength.y, touchLength.x) * Mathf.Rad2Deg;

                        //if (-45.0f >= swipeAngle  && swipeAngle >= -135.0f)
                        //{
                        //    SwipeDirection = SwipeDir.Up;
                        //}
                        //else if (45.0f <= swipeAngle && swipeAngle <= 135.0f)
                        //{
                        //    SwipeDirection = SwipeDir.Down;
                        //}
                        //else if (-45.0f < swipeAngle && swipeAngle < 45.0f)
                        //{
                        //    SwipeDirection = SwipeDir.Left;
                        //}
                        //else if (-45.0f > swipeAngle && swipeAngle > 135.0f)
                        //{
                        //    SwipeDirection = SwipeDir.Right;
                        //}

                        StartCoroutine(SwipeEvent());
                    }
                }
            }
        }
        // For Debugging
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            IsTouch = true;
            StartCoroutine(TouchEvent());
        }
    }
    
    IEnumerator TouchEvent()
    {
        yield return new WaitForFixedUpdate();
        IsTouch = false;
    }

    IEnumerator SwipeEvent()
    {
        yield return new WaitForFixedUpdate();
        IsSwipe = false;
    }
}
