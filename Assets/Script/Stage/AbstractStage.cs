using System.Collections;
using RAS;
using UnityEngine;

/// <summary>
/// 모든 스테이지의 기본이 되는 클래스입니다.
/// </summary>
public class AbstractStage : MonoBehaviour
{
    public BaseStage _baseStage;

    public virtual void OnNote(Note note)
    {
        // name을 switch해서 해당 노트에서 사용할 bgm, 효과, 애니메이션 사용.
    }

    public virtual void OnSuccess()
    {
    }

    public virtual void OnFail()
    {
    }
}
