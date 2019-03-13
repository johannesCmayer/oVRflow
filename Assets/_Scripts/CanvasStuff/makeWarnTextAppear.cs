using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class makeWarnTextAppear : MonoBehaviour 
{

	Image myImage;
	Text txt;

	public bool isImage;

	void Start () 
	{

		if (isImage == false) 
		{
			txt = GetComponent<Text> ();

		} 
		else 
		{
			myImage = GetComponent<Image> ();

		}

	}


			

	void Update () 
	{

		if (isImage == false) 
		{


			if (ObjectContainer.instance.collidingFollowers > 0) 
			{
				txt.enabled = true;
			} 
			else 
			{
				txt.enabled = false;
			}

		}

		else 
		{
			if (ObjectContainer.instance.collidingFollowers > 0) 
			{
				myImage.enabled = true;
			} 
			else 
			{
				myImage.enabled = false;
			}



		}
	}
}