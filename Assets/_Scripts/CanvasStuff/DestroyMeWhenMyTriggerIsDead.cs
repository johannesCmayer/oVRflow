using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DestroyMeWhenMyTriggerIsDead : MonoBehaviour 
{

	public GameObject myTrigger;
    Color startColor;
	// Use this for initialization
	void Start () 
	{
        startColor = GetComponent<Image>().color;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (myTrigger == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (myTrigger.activeInHierarchy == false)
            {
                Color transparent = new Color(1, 1, 1, 0);
                GetComponent<Image>().color = transparent;
            }
            if (myTrigger.activeInHierarchy == true)
            {
                GetComponent<Image>().color = startColor;
            }
        }
	}
}
