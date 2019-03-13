using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {

    public float spawnIntervallInBeats = 1;
    public GameObject[] objectPrefabPool;
    //public float beatsToDestroyArea = 10f;
        
    public float spawnOffset;
    public bool spawnObjects = true;
    public bool isHand = false;

    public bool useRandomPosition = true;

    float timer;
    float timeStamp;

    TriggerManager triggerMan;
	SoundManagement soundMan;
    
	void Start()
	{
		triggerMan = TriggerManager.instance;
		soundMan = SoundManagement.instance;

        timeStamp = Time.time;
	}

    
	void Update ()
    {
        if (Time.time - timeStamp + spawnOffset * soundMan.beatIntervall > timer)
        {
            if (spawnObjects && this != null)
			    SpawnObject();
            
            timer += soundMan.beatIntervall * spawnIntervallInBeats;
        }
	}

	public void SpawnObject()
    {
        Vector3 randomPosition;

        if (useRandomPosition)
        {
            randomPosition = new Vector3(Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2), Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2), Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)) + transform.position;
        }
        else
            randomPosition = transform.position;


        GameObject area = Instantiate(objectPrefabPool[Random.Range(0, objectPrefabPool.Length)], randomPosition, Quaternion.identity);

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
		//area.GetComponent<selfDestructAfterTime> ().timeToDestroy = beatsToDestroyArea * soundMan.beatIntervall;
		area.GetComponent<DestroyWhenSpawedColliding> ().spawner = this;
		detectScript.spawner = this.gameObject;
        triggerMan.allBoxTriggers.Add(area);

        area.GetComponent<DestroyWhenSpawedColliding>().spawner = gameObject.GetComponent<SpawnObjects>();
    }
}
