using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHoldSphere : Phase
{
	public GameObject leftFootFollower;
	public GameObject rightFootFollower;

	//public GameObject leftFootHoldSphere;
	//public GameObject rightFootHoldSphere;

	protected override void PhaseStart()
	{
		SpawnFromRandomSpawnPool ();
	}

	protected override void PhaseUpdate()
	{
		if (mySpawns[0].name.Contains("left")) 
		{
			leftFootFollower.SetActive (false);
		}
		if (mySpawns[0].name.Contains("right")) 
		{
			rightFootFollower.SetActive (false);
		}
	}
}

