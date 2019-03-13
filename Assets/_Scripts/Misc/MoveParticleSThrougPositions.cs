using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticleSThrougPositions : MonoBehaviour
{
    public float moveSpeed;
    public GameObject[] gOPositions;

    private float timeStamp;
    Vector3 positionStamp;
    
    int i = 0;

    void Start ()
	{
        timeStamp = Time.time;
        positionStamp = transform.position;
	}

    void Update()
    {
        transform.position = Vector3.Lerp(positionStamp, gOPositions[i].transform.position, moveSpeed * (Time.time - timeStamp));

        if ((gOPositions[i].transform.position - transform.position).magnitude < 0.1f)
        {
            timeStamp = Time.time;
            positionStamp = transform.position;
            i++;
        }
            
        

        if (i >= gOPositions.Length)
        {
            i = 0;
        }
	}
}
