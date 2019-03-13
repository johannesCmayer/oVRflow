using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowerStartOffset : MonoBehaviour 
{

	public enum OffsetDirection
	{
		X,
		Y,
		Z		
	}

	public OffsetDirection offsetDirection;

	Vector3 oldPos;
	Vector3 newPos;

	public float startOffset;
	public float speedTowardsPlayer;

	bool add = false;

	FollowWithDelay followWithDelayScript;

	void Start () 
	{
		followWithDelayScript = GetComponent<FollowWithDelay>();
		followWithDelayScript.FollowerTickCompleted += CalcFollowerOffset;

		oldPos = transform.position;
		newPos = transform.position;

		if (startOffset < 0) 
		{
			add = true;
		}
	}

	void CalcFollowerOffset(object script, EventArgs e) 
	{
		oldPos = transform.position;

		if (add == true) 
		{
			if (startOffset >= 0) 
			{
				followWithDelayScript.FollowerTickCompleted -= CalcFollowerOffset;
				Destroy (this);
			}
			startOffset += Time.deltaTime * speedTowardsPlayer;
		}

		if (add == false) 
		{
			if (startOffset <= 0) 
			{
				followWithDelayScript.FollowerTickCompleted -= CalcFollowerOffset;
				Destroy (this);
			}
			startOffset -= Time.deltaTime * speedTowardsPlayer;
		}

		if (offsetDirection == OffsetDirection.X) 
		{
			newPos = new Vector3 (oldPos.x + startOffset, oldPos.y, oldPos.z);
		}
		if (offsetDirection == OffsetDirection.Y) 
		{
			newPos = new Vector3 (oldPos.x, oldPos.y + startOffset, oldPos.z);
		}
		if (offsetDirection == OffsetDirection.Z) 
		{
			newPos = new Vector3 (oldPos.x, oldPos.y, oldPos.z + startOffset);
		}

		//print ("Offset"+startOffset);
		//print ("Position" + newPos.z);
		transform.position = newPos;
	}
}
