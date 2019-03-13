using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentChildrenAndDestroy : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.DetachChildren();
        Destroy(gameObject);
    }
}
