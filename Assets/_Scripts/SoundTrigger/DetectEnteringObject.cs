using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwitchTriggerToNearDespawnMode))]
public class DetectEnteringObject : MonoBehaviour
{
    public SoundTriggerData soundTriggerData;

    public string[] tagsToDetect;

	TriggerManager triggerMan;
    public GameObject fracturedCube;

    SwitchTriggerToNearDespawnMode switchTriggerToNearDespawnMode;

    public ColorID colorID;

    public GameObject spawner;

	[HideInInspector]
    public float spawnerHeight;
	[HideInInspector]
    public float spawnPosition;

	public bool isHand;

    float spawnHeight1;
    float spawnHeight2;
    float spawnHeight3;
    float spawnHeight4;

	public int soundPitch;
	public Instrument myInstrument;

    private void Start()
    {
        triggerMan = TriggerManager.instance;
        switchTriggerToNearDespawnMode = GetComponent<SwitchTriggerToNearDespawnMode>();

		myInstrument = soundTriggerData.instrument;

		spawnHeight1 = spawnerHeight / 100 * 20 + spawnPosition;
		spawnHeight2 = spawnerHeight / 100 * 40 + spawnPosition;
		spawnHeight3 = spawnerHeight / 100 * 60 + spawnPosition;
		spawnHeight4 = spawnerHeight / 100 * 80 + spawnPosition;
		//print (spawnHeight1);
		//print (spawnHeight2);
		//print (spawnHeight3);
		//print (spawnHeight4);
    }

	void Update()
	{
		if (isHand == true)
		{
			if (this.gameObject.transform.position.y < spawnHeight1)
			{
				soundTriggerData.soundPitch = 0;
			}

			if (this.gameObject.transform.position.y >= spawnHeight1 && this.gameObject.transform.position.y < spawnHeight2)
			{
				soundTriggerData.soundPitch = 1;
			}

			if (this.gameObject.transform.position.y >= spawnHeight2 && this.gameObject.transform.position.y < spawnHeight3)
			{
				soundTriggerData.soundPitch = 2;
			}

			if (this.gameObject.transform.position.y >= spawnHeight3 && this.gameObject.transform.position.y < spawnHeight4)
			{
				soundTriggerData.soundPitch = 3;
			}

			if (this.gameObject.transform.position.y >= spawnHeight4)
			{
				soundTriggerData.soundPitch = 4;
			}
			soundPitch = soundTriggerData.soundPitch;
		}


		if (isHand == false) 
		{
			if (this.gameObject.transform.position.z < spawnHeight1) 
			{
				soundTriggerData.soundPitch = 0;
			}

			if (this.gameObject.transform.position.z >= spawnHeight1 && this.gameObject.transform.position.z < spawnHeight2) 
			{
				soundTriggerData.soundPitch = 1;
			}

			if (this.gameObject.transform.position.z >= spawnHeight2 && this.gameObject.transform.position.z < spawnHeight3) 
			{
				soundTriggerData.soundPitch = 2;
			}

			if (this.gameObject.transform.position.z >=	spawnHeight3 && this.gameObject.transform.position.z < spawnHeight4) 
			{
				soundTriggerData.soundPitch = 3;
			}

			if (this.gameObject.transform.position.z >= spawnHeight4) 
			{
				soundTriggerData.soundPitch = 4;
			}
			soundPitch = soundTriggerData.soundPitch;
		}
	}

    void OnTriggerEnter(Collider c)
    {
        soundTriggerData.playPosition = transform.position;
        soundTriggerData.soundType = SoundType.InstrumentSound;
        soundTriggerData.instrumentSoundType = InstrumentSoundType.Normal;

        if (isHand == true)
        {
            if (this.gameObject.transform.position.y < spawnHeight1)
            {
                soundTriggerData.soundPitch = 0;
            }

            if (this.gameObject.transform.position.y >= spawnHeight1 && this.gameObject.transform.position.y < spawnHeight2)
            {
                soundTriggerData.soundPitch = 1;
            }

            if (this.gameObject.transform.position.y >= spawnHeight2 && this.gameObject.transform.position.y < spawnHeight3)
            {
                soundTriggerData.soundPitch = 2;
            }

            if (this.gameObject.transform.position.y >= spawnHeight3 && this.gameObject.transform.position.y < spawnHeight4)
            {
                soundTriggerData.soundPitch = 3;
            }

            if (this.gameObject.transform.position.y >= spawnHeight4)
            {
                soundTriggerData.soundPitch = 4;
            }

        }


        if (isHand == false)
        {
            if (this.gameObject.transform.position.z < spawnHeight1)
            {
                soundTriggerData.soundPitch = 0;
            }

            if (this.gameObject.transform.position.z >= spawnHeight1 && this.gameObject.transform.position.z < spawnHeight2)
            {
                soundTriggerData.soundPitch = 1;
            }

            if (this.gameObject.transform.position.z >= spawnHeight2 && this.gameObject.transform.position.z < spawnHeight3)
            {
                soundTriggerData.soundPitch = 2;
            }

            if (this.gameObject.transform.position.z >= spawnHeight3 && this.gameObject.transform.position.z < spawnHeight4)
            {
                soundTriggerData.soundPitch = 3;
            }

            if (this.gameObject.transform.position.z >= spawnHeight4)
            {
                soundTriggerData.soundPitch = 4;
            }

        }
        
        foreach (var item in tagsToDetect)
        {
            if (c.gameObject.CompareTag(item))
            {
                DestroyTrigger();
            }
        }
    }

    public void DestroyTrigger()
    {

        if (switchTriggerToNearDespawnMode.isInNearDespawnMode)
        {
            soundTriggerData.deathState = DeathState.inNearDespawnMode;
            triggerMan.cubeCriticalyDestroyedEvent(colorID);
        }

        else
        {
            triggerMan.cubeDestroyedEvent(colorID);
        }

        SoundManagement.instance.triggerQueue.Add(soundTriggerData);
        Instantiate(fracturedCube, transform.position, Quaternion.identity);
        triggerMan.allBoxTriggers.Remove(gameObject);
        Destroy(gameObject);
    }

    public void DestroyAllTriggers()
    {
        foreach (var item in triggerMan.allBoxTriggers)
        {
            GameObject newFracturedCube = Instantiate(fracturedCube, transform.position, Quaternion.identity);
            Destroy(newFracturedCube.GetComponent<ParticleSystem>());
            Destroy(gameObject);
        }
        triggerMan.allBoxTriggers.Clear();
    }
}

public enum ColorID
{
    None,
    Blue,
    Magenta,
    Green,
    Orange
}
