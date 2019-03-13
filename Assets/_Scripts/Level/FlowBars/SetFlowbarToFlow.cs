using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFlowbarToFlow : MonoBehaviour {
	Slider bar;
	FlowManager flowMan;
	// Use this for initialization
	void Start () {
		flowMan = FlowManager.instance;
		bar = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		bar.value = flowMan.flow/100/2;
	}
}
