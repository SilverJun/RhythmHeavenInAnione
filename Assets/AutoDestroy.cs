using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void EndAnimDestroy()
    {
        Debug.Log("오토 디스트로이!!");
        Destroy(gameObject);
    }
}
