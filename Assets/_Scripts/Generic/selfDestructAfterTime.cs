using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestructAfterTime : MonoBehaviour {
    
    public float beatsToDestroy = 10;

	[HideInInspector]
	public float timeToDestroy;

	[HideInInspector]
	public float counter;

	public bool isBox=true;

	TriggerManager triggerMan;

	FlowManager flowMan;
    
	SoundManagement soundMan;

	void Start()
	{
		triggerMan = TriggerManager.instance;

		soundMan = SoundManagement.instance;
		timeToDestroy = beatsToDestroy * 60 / soundMan.effectiveBeatsPerMinute;

		flowMan = FlowManager.instance;
	}


	void Update ()
    {
		timeToDestroy = beatsToDestroy * 60 / soundMan.effectiveBeatsPerMinute;
		counter += Time.deltaTime;
		if (counter >= timeToDestroy) 
		{
			if (isBox==true) 
			{
				triggerMan.allBoxTriggers.Remove (this.gameObject);
				flowMan.flow -= flowMan.flowLossAtBoxDespawn;
			}
			Destroy (this.gameObject);
		}
		
	}
}
