using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerMaterialToConcrete : MonoBehaviour 
{
	public Material concrete;

	Material myMaterial;

	MeshRenderer matContainer;

	GameStateManagement stateMan;

	public float minFlicker = 0.05f;
	public float maxFlicker = 0.16f;

	bool concreteIsOn = false;

	float startMaxFlicker;

	// Use this for initialization
	void Start () 
	{
		stateMan = GameStateManagement.instance;
		matContainer = GetComponent <MeshRenderer> ();
		myMaterial = matContainer.material;
		startMaxFlicker = maxFlicker;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (stateMan.gameState.primaryPhase == PrimaryPhase.DanceOver) 
		{
			if (concreteIsOn == false) 
			{
				StartCoroutine (transformToConcrete());
				concreteIsOn = true;
			}
		} 
		else 
		{
			maxFlicker = startMaxFlicker;
			concreteIsOn = false;
			matContainer.material = myMaterial;
			matContainer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
	}

	IEnumerator transformToConcrete()
	{
		for (int i = 0; i < 6; i++) 
		{	
			maxFlicker += 0.01f;
			matContainer.material = myMaterial;
			matContainer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			yield return new WaitForSeconds (Random.Range (minFlicker, maxFlicker));

			matContainer.material = concrete;
			matContainer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

			yield return new WaitForSeconds (Random.Range (minFlicker, maxFlicker));

			if (stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver) 
			{
				maxFlicker = startMaxFlicker;
				break;
			}
		}
	}

}
