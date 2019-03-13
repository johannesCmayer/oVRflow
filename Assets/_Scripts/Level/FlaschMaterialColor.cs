using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaschMaterialColor : MonoBehaviour
{
    public AnimationCurve groundColorPulse;
    Material myMaterial;
	GameStateManagement stateMan;
	void Start ()
	{
		stateMan = GameStateManagement.instance;
        myMaterial = GetComponent<Renderer>().material;
    }
    
	void Update ()
	{
		if (Time.time != 0.0f) 
		{
			if (stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver) 
			{
				myMaterial.SetColor ("_EmissionColor", Color.HSVToRGB (0.5f, 0.8f, groundColorPulse.Evaluate (Time.time / SoundManagement.instance.beatIntervall / 4)));
			}
		}
    }
}
