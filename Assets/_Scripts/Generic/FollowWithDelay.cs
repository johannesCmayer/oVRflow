using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowWithDelay : MonoBehaviour
{
	public event EventHandler FollowerTickCompleted;
    public GameObject master;

    public float delayTime;
	float delayTimeInSeconds;
    public int tickRate;

    private Vector3[] bufferPos;
    private Vector3 followerPosition;
    private Quaternion[] bufferRot;
    private Quaternion followerRotation;

    private int inCounter;
    private int outCounter;

    private int bufferSize;
    private float tickDuration;
	float RingBufferSizefloat;
	SoundManagement soundMan;
	float prevbpm;
    void Start()
    {
		soundMan = SoundManagement.instance;

		delayTimeInSeconds=delayTime  * 60 / soundMan.effectiveBeatsPerMinute;

        RingBufferSizefloat = Mathf.Round(delayTimeInSeconds * tickRate);
        bufferSize = (int)RingBufferSizefloat;

        bufferPos = new Vector3[60000];
        bufferRot = new Quaternion[60000];

        inCounter = bufferSize - 1;
        outCounter = 0;

        tickDuration = (1.0f / tickRate);
        InvokeRepeating("Tick", 0f, tickDuration);
        
        string masterTag = GetComponent<SelfCollision>().objID.ToString();
        master = GameObject.FindGameObjectWithTag(masterTag);
		//prevbpm = soundMan.beatsPerMinute;
    }

    void Tick()
    {
		delayTimeInSeconds=delayTime  * 60 / soundMan.effectiveBeatsPerMinute;
		//if (soundMan.beatsPerMinute != prevbpm) {
		if( inCounter<=bufferSize/2)
		{
			RingBufferSizefloat = Mathf.Round (delayTimeInSeconds * tickRate);
			bufferSize = (int)RingBufferSizefloat;
			//prevbpm = soundMan.beatsPerMinute;
		}
		//}

        string masterTag = GetComponent<SelfCollision>().objID.ToString();
        master = GameObject.FindGameObjectWithTag(masterTag);

        if (master != null)
        {
            if (inCounter >= bufferSize)
            {
                inCounter = 0;
            }

            if (outCounter >= bufferSize)
            {
                outCounter = 0;
            }

            if (inCounter >= bufferPos.Length)
            {
                inCounter = bufferPos.Length;
            }
            if (outCounter >= bufferPos.Length)
            {
                outCounter = bufferPos.Length;
            }

            bufferPos[inCounter] = master.transform.position;
            followerPosition = bufferPos[outCounter];

            bufferRot[inCounter] = master.transform.rotation;
            followerRotation = bufferRot[outCounter];

            outCounter++;
            inCounter++;
                       

            transform.position = followerPosition;
            transform.rotation = followerRotation;
        }

		OnFollowerTickCompleted (EventArgs.Empty);
    }

	void OnFollowerTickCompleted(EventArgs e)
	{
		if (FollowerTickCompleted != null) 
		{
			FollowerTickCompleted (this, e);
		}
	}
}
