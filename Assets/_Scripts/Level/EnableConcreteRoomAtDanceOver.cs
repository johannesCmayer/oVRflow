using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableConcreteRoomAtDanceOver : MonoBehaviour {

	GameStateManagement stateMan;

	bool concreteIsOn = false;

	GameObject[] children;
	public float minFlicker = 0.05f;
	public float maxFlicker = 0.16f;

	float startMaxFlicker;
	// Use this for initialization
	void Start () {
		stateMan = GameStateManagement.instance;

		children = GameObject.FindGameObjectsWithTag("ConcreteChild");
		startMaxFlicker = maxFlicker;
	}


	
	// Update is called once per frame
	void Update () {
		if (stateMan.gameState.primaryPhase == PrimaryPhase.DanceOver) 
		{
			if (concreteIsOn == false) 
			{
				foreach (var item in children) 
				{
					StartCoroutine (transformToReality(item));
				}
				concreteIsOn = true;
			}
		} 
		else 
		{
			foreach (var item in children) 
			{			
				item.gameObject.SetActive (false);
			}
			maxFlicker = startMaxFlicker;
			concreteIsOn = false;
		}
	}

	IEnumerator transformToReality(GameObject child)
	{
		for (int i = 0; i < 4; i++) 
		{	
			maxFlicker += 0.0038f;
			child.gameObject.SetActive (false);

			yield return new WaitForSeconds (Random.Range (minFlicker, maxFlicker));
		
			child.gameObject.SetActive (true);

			yield return new WaitForSeconds (Random.Range (minFlicker, maxFlicker));

			if (stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver) 
			{
				maxFlicker = startMaxFlicker;
				break;
			}
		}
	}
}
