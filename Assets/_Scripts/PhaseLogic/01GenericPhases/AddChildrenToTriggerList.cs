using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChildrenToTriggerList : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) 
		{
			TriggerManager.instance.allBoxTriggers.Add(child.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
