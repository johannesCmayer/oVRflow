using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImageSinusFlicker : MonoBehaviour {

	Text txt;
	Color startColor;
	Color currentColor;

    Image image;

	public float sinusSpeed = 1;
	float currentSinusSpeed;
	public float sinusStrength = 5f;

    public bool isImage;


	void Start () 
	{
		currentSinusSpeed = sinusSpeed * SoundManagement.instance.effectiveBeatsPerMinute / 60;

        if(isImage == true)
        {
            image = GetComponent<Image>();
            startColor = image.color;
        }

        if (isImage == false)
        {
            txt = GetComponent<Text>();
            startColor = txt.color;
        }

    }

	// Update is called once per frame
	void Update () 
	{
        if (isImage == false)
        {

            currentSinusSpeed = sinusSpeed * SoundManagement.instance.effectiveBeatsPerMinute / 60;
            currentColor = new Color(startColor.r, startColor.g, startColor.b, startColor.a + Mathf.Sin(((Time.time * currentSinusSpeed)) + 0.5f) * (sinusStrength));
            txt.color = currentColor;

        }


        if (isImage == true)
        {

            currentSinusSpeed = sinusSpeed * SoundManagement.instance.effectiveBeatsPerMinute / 60;
            currentColor = new Color(startColor.r, startColor.g, startColor.b, startColor.a + Mathf.Sin(((Time.time * currentSinusSpeed)) + 0.5f) * (sinusStrength));
            image.color = currentColor;

        }


    }
}
