using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateChildOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetChild (0).gameObject.SetActive (true);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
