using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationOnSiunsCurve : MonoBehaviour {

	public float globalSinusSpeed = 1;
	public float globalSinusStrength = 0.2f;

	public bool rotateOnX;
	public bool rotateOnY;
	public bool rotateOnZ;

	public bool separateSpeed;
	public bool separateStrength;

	public float sinusSpeedX;
	public float sinusSpeedY;
	public float sinusSpeedZ;

	public float sinusStrengthX;
	public float sinusStrengthY;
	public float sinusStrengthZ;

	public float sinusOffsetX;
	public float sinusOffsetY;
	public float sinusOffsetZ;

	Vector3 startRotation;
	Vector3 currentRotation;

	void Start () {
		startRotation = transform.rotation.eulerAngles;
	}

	void Update () {
		if (separateSpeed == false) 
		{
			sinusSpeedX = globalSinusSpeed;
			sinusSpeedY = globalSinusSpeed;
			sinusSpeedZ = globalSinusSpeed;
		}
		if (separateStrength == false) 
		{
			sinusStrengthX = globalSinusStrength;
			sinusStrengthY = globalSinusStrength;
			sinusStrengthZ = globalSinusStrength;
		}

		if (rotateOnX == true) 
		{
			currentRotation.x = startRotation.x + Mathf.Sin (Time.time * sinusSpeedX+sinusOffsetX) * sinusStrengthX;
		}
		if (rotateOnY == true) 
		{
			currentRotation.y = startRotation.y + Mathf.Sin (Time.time * sinusSpeedY+sinusOffsetY) * sinusStrengthY;
		}
		if (rotateOnZ == true) 
		{
			currentRotation.z = startRotation.z + Mathf.Sin (Time.time * sinusSpeedZ+sinusOffsetZ) * sinusStrengthZ;
		}

		transform.Rotate (currentRotation);
		//transform.localRotation.eulerAngles = currentRotation;
	}
}