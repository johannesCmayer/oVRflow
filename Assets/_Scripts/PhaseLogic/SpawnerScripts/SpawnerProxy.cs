using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnerProxy : MonoBehaviour {
	public bool isHand;
	//public bool thisPhaseUsesMultiBoxObjects;
	//public GameObject[]
	void Awake () {

		foreach (GameObject myTriggerArea in GetComponentInParent<SpawnBoxesInFixedOrder>().spawnedTriggerareas) 
		{
            if (myTriggerArea.GetComponent<DetectEnteringObject>() != null)
            {
                DetectEnteringObject detectScript = myTriggerArea.gameObject.GetComponent<DetectEnteringObject>();
                if (detectScript.colorID == ColorID.Blue || detectScript.colorID == ColorID.Magenta)
                {
                    detectScript.isHand = true;
                }
                else
                {
                    detectScript.isHand = false;
                }

				if ((detectScript.isHand == true) && isHand==true/*&& (myTriggerArea.CompareTag ("RightHandTrigger") || myTriggerArea.CompareTag ("LeftHandTrigger"))*/) 
				{
					detectScript.spawnerHeight = transform.lossyScale.y / 100 * 100;
					detectScript.spawnPosition = transform.position.y - transform.lossyScale.y / 2;
				} 
				if ((detectScript.isHand == false) && (myTriggerArea.CompareTag ("RightFootTrigger") || myTriggerArea.CompareTag ("LeftFootTrigger"))&&isHand==false) 
				{
					detectScript.spawnerHeight = transform.lossyScale.z / 100 * 100;
					detectScript.spawnPosition = transform.position.z - transform.lossyScale.z / 2;
				}
			} 
			else 
			{
				foreach (Transform subTriggerArea in myTriggerArea.transform) 
				{
					DetectEnteringObject detectScript = subTriggerArea.gameObject.GetComponent<DetectEnteringObject> ();
                    if (detectScript.colorID == ColorID.Blue || detectScript.colorID == ColorID.Magenta)
                    {
                        detectScript.isHand = true;
                    }
                    else
                    {
                        detectScript.isHand = false;
                    }
                    if ((detectScript.isHand == true)&&isHand==true /*&& (subTriggerArea.gameObject.CompareTag ("RightHandTrigger") || subTriggerArea.gameObject.CompareTag ("LeftHandTrigger"))*/) 
					{
						detectScript.spawnerHeight = transform.lossyScale.y / 100 * 100;
						detectScript.spawnPosition = transform.position.y - transform.lossyScale.y / 2;
					} 
					if ((detectScript.isHand == false) && (subTriggerArea.gameObject.CompareTag ("RightFootTrigger") || subTriggerArea.gameObject.CompareTag ("LeftFootTrigger"))&&isHand==false) 
					{
						detectScript.spawnerHeight = transform.lossyScale.z / 100 * 100;
						detectScript.spawnPosition = transform.position.z - transform.lossyScale.z / 2;
					}
				}

			}
		}

		Destroy (this.gameObject);
	}
}
