using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenSpawedColliding : MonoBehaviour
{
    public float framesToCheck = 1;

    int i = 0;
    bool triggered;

	TriggerManager triggerMan;
    
    [HideInInspector]
    public SpawnObjects spawner;


    private void Start()
    {
		triggerMan = TriggerManager.instance;
        GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        if (spawner != null)
        {
            if (triggered)
            {
                spawner.SpawnObject();
                triggerMan.allBoxTriggers.Remove(this.gameObject);
                Destroy(this.gameObject);
            }

            if (i >= framesToCheck)
            {
                GetComponent<Renderer>().enabled = true;
                gameObject.layer = 0;
                Destroy(this);
            }

            i++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(gameObject.tag) || other.CompareTag("NoSpawnArea"))
        {
            triggered = true;
        }
    }
}
