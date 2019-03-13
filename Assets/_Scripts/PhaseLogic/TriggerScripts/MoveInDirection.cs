using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInDirection : MonoBehaviour
{
    public Vector3 moveDirection;
    public float outOfBoundsTravelSpeedCoef = 40;
    public Vector3 normalSpeedBoundsZPos = new Vector3(3, 3, 3);

    SoundManagement soundMan;

    private void Start()
    {
        soundMan = SoundManagement.instance;
    }

    void Update()
    {
        if (transform.position.x < normalSpeedBoundsZPos.x && transform.position.x > -normalSpeedBoundsZPos.x  && transform.position.y < normalSpeedBoundsZPos.y && transform.position.y > -normalSpeedBoundsZPos.y && transform.position.z < normalSpeedBoundsZPos.z && transform.position.z > -normalSpeedBoundsZPos.z)
            transform.position += moveDirection * Time.deltaTime * (soundMan.effectiveBeatsPerMinute / 120);
        else
            transform.position += moveDirection * Time.deltaTime * outOfBoundsTravelSpeedCoef / (soundMan.effectiveBeatsPerMinute / 120);
    }
}