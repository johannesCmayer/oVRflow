using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnObjectsNoIntervall : MonoBehaviour {

    public float spawnIntervallInBeats = 1;
    public GameObject[] objectPrefabPool;
    //public float beatsToDestroyArea = 10f;
        
    public float spawnOffset;
    public bool spawnObjects = true;
    public bool isHand = false;
    
    float timer;
    float timeStamp;
    TriggerManager triggerMan;
	SoundManagement soundMan;

    public bool spawn;

    public float boxDeathAfterXBeats;
    public float riskyModeAtXBeatsBeforeDespawn;
    void Start()
	{
		triggerMan = TriggerManager.instance;
		soundMan = SoundManagement.instance;
	}

    
	void Update ()
    {
    

        if (Time.time - timeStamp + spawnOffset * soundMan.beatIntervall > timer)
        {
            if(spawn == true)
            {
                if (spawnObjects && this != null)
                {

                    SpawnObject();
                    spawn = false;

                }
            }

            timer += soundMan.beatIntervall;
        }
    }

	public void SpawnObject()
    {
		Vector3 randomPosition = new Vector3(Random.Range(-transform.localScale.x/2, transform.localScale.x/2), Random.Range(-transform.localScale.y/2, transform.localScale.y/2), Random.Range(-transform.localScale.z/2, transform.localScale.z/2)) + transform.position;

        GameObject area = Instantiate(objectPrefabPool[Random.Range(0, objectPrefabPool.Length)], randomPosition, Quaternion.identity);

        area.GetComponent<selfDestructAfterTime>().beatsToDestroy = boxDeathAfterXBeats;
        area.GetComponent<SwitchTriggerToNearDespawnMode>().switchAtXBeatsBeforeDespawn = riskyModeAtXBeatsBeforeDespawn;

		DetectEnteringObject detectScript = area.GetComponent<DetectEnteringObject> ();
		detectScript.isHand = isHand;
		if (isHand == true) 
		{
			detectScript.spawnerHeight = transform.lossyScale.y / 100 * 100;
			detectScript.spawnPosition = transform.position.y - transform.lossyScale.y / 2;
		} 
		else 
		{
			detectScript.spawnerHeight = transform.lossyScale.z / 100 * 100;
			detectScript.spawnPosition = transform.position.z - transform.lossyScale.z / 2;
		}

		area.GetComponent<DestroyWhenSpawedCollidingNoIntervall> ().spawner = this;
		detectScript.spawner = this.gameObject;
        triggerMan.allBoxTriggers.Add(area);

        area.GetComponent<DestroyWhenSpawedCollidingNoIntervall>().spawner = gameObject.GetComponent<SpawnObjectsNoIntervall>();
    }
}
