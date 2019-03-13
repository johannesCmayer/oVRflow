using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV_Scroll : MonoBehaviour
{
    public float speed = 0.25f;
    float scroll;
    Vector2 speedV;
    Renderer myMaterial;
    public AnimationCurve groundColorPulse;
    Color startColor;
    float hue;
    float sat;
#pragma warning disable
    float val;
#pragma warning restore

	GameStateManagement stateMan;

    void Start()
    {
		stateMan = GameStateManagement.instance;
        myMaterial = GetComponent<Renderer>();
        startColor = myMaterial.material.GetColor("_EmissionColor");
        Color.RGBToHSV(startColor, out hue, out sat, out val);
    }

    void Update()
    {
		if (stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver) 
		{
			myMaterial.material.SetTextureOffset ("_MainTex", new Vector2 (0, scroll));
		}

        if (Time.time != 0)
        {
            speed = groundColorPulse.Evaluate(Time.time / SoundManagement.instance.beatIntervall / 4) * 4;
            scroll = scroll + speed * Time.deltaTime;

			if (stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver) 
			{
				myMaterial.material.SetColor ("_EmissionColor", Color.HSVToRGB (hue, sat, groundColorPulse.Evaluate (Time.time / SoundManagement.instance.beatIntervall / 4) * 2));
			}
        }
    }
}
