using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterTime : MonoBehaviour
{
    public float timeToDestroy = 5;

	void Start ()
	{
        Destroy(this.gameObject, timeToDestroy);
	}

	void Update ()
	{
		
	}
}
