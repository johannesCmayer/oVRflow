using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementOnSinusCurve : MonoBehaviour {
    
	public float globalSinusSpeed = 1;
	public float globalSinusStrength = 0.2f;

	public bool moveOnX;
	public bool moveOnY;
	public bool moveOnZ;

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

	Vector3 currentPosition;

    public bool useCreationTimeAsInput = true;

    float timeStamp;

    Vector3 startPos;

    public bool useStartPos;

    void Start()
    {
        if (useCreationTimeAsInput)
        {
            startPos = transform.position;
            timeStamp = Time.time;
        }
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

		if (moveOnX == true) 
		{
            if (useStartPos == true)
            {
                currentPosition.x = startPos.x + Mathf.Sin((Time.time - timeStamp) * sinusSpeedX + sinusOffsetX) * (sinusStrengthX / 100);
            }
            else
            {
                currentPosition.x = transform.position.x + Mathf.Sin((Time.time - timeStamp) * sinusSpeedX + sinusOffsetX) * (sinusStrengthX / 1000);
            }

		}
		else
		{
			currentPosition.x = transform.position.x;
		}
		if (moveOnY == true) 
		{
            if (useStartPos == true)
            {
                currentPosition.y = startPos.y + Mathf.Sin((Time.time - timeStamp) * sinusSpeedY + sinusOffsetY) * (sinusStrengthY / 100);
            }
            else
            {
                currentPosition.y = transform.position.y + Mathf.Sin((Time.time - timeStamp) * sinusSpeedY + sinusOffsetY) * (sinusStrengthY / 1000);
            }
		}
		else
		{
			currentPosition.y = transform.position.y;
		}
		if (moveOnZ == true) 
		{
            if (useStartPos == true)
            {
                currentPosition.z = startPos.z + Mathf.Sin((Time.time - timeStamp) * sinusSpeedZ + sinusOffsetZ) * (sinusStrengthZ / 100);
            }
            else
            {
                currentPosition.z = transform.position.z + Mathf.Sin((Time.time - timeStamp) * sinusSpeedZ + sinusOffsetZ) * (sinusStrengthZ / 1000);
            }
		}
		else
		{
			currentPosition.z = transform.position.z;
		}

		transform.position = currentPosition;

	}
}
