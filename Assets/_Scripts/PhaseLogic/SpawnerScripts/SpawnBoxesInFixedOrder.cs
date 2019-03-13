using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnBoxesInFixedOrder : Phase
{
	TriggerManager triggerMan;
	public GameObject[] spawnedTriggerareas;

	protected override void PhaseStart()
	{
		triggerMan = TriggerManager.instance;
		foreach (GameObject triggerarea in spawnedTriggerareas) 
		{
			if (triggerarea.transform.childCount == 0) 
			{
				triggerMan.allBoxTriggers.Add (triggerarea);
			}
			triggerarea.gameObject.SetActive (false);

		}
	}

	protected override void PhaseUpdate()
	{
		for (int i = 0; i < spawnedTriggerareas.Length; i++) 
		{
			if (spawnedTriggerareas [i] != null) 
			{
				spawnedTriggerareas [i].SetActive (true);
				break;
			}
		}

		if (transform.childCount == 0) 
		{
			ForcePhaseCompletion ();
		}
	}
}
