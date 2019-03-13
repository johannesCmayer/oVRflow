using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Objectpointer : MonoBehaviour
{
    Vector3 viewPortPos;

	TriggerManager triggerMan;

    private Vector3 positionOnScreenEdge;
    private Vector3 screenSpacePos;

    float angleBetweenScreenVecAndDirVec;
    //float rightAngle = 90f;
    float calculatedAngle;

    Vector2 vecToObject;
    Vector2 vecToScreen;

    float distToScreen;
    float distToObject;
    float finalDist;

    public Vector2 offSet;
    public float cricleRadius = 1000;
    public float circleRadiusYCoef = 0.7f;

	public GameObject[] Images;
	GameObject myImage;
    void Start()
    {
		triggerMan = TriggerManager.instance;
    }


    int i;
    private void Update()
    {
		if (triggerMan.allBoxTriggers.Count != 0) 
		{
			foreach (GameObject triggerArea in triggerMan.allBoxTriggers) 
			{
				if (triggerArea.activeInHierarchy && triggerArea.GetComponent <ImageReference> ().hasImage == false) 
				{
                    //print(i++);
					myImage = Instantiate (Images [(int)triggerArea.GetComponent <DetectEnteringObject> ().colorID - 1], this.gameObject.transform);
					triggerArea.GetComponent <ImageReference> ().hasImage = true;
					myImage.GetComponent <DestroyMeWhenMyTriggerIsDead> ().myTrigger = triggerArea;
                    //print(triggerArea);
                   
                }

				viewPortPos = Camera.main.WorldToViewportPoint (triggerArea.transform.position);
                if ((viewPortPos[0] > 0.8 || viewPortPos[0] < 0.2 || viewPortPos[1] > 0.8 || viewPortPos[1] < 0.2) || viewPortPos.z < 0)
                {
                    if (myImage != null)
                    {
                        myImage.GetComponent<Image>().enabled = true;

                        if (Vector3.Angle(Camera.main.transform.forward, Camera.main.transform.position - triggerArea.transform.position) < 90)
                            viewPortPos *= -1;

                        screenSpacePos = Camera.main.ViewportToScreenPoint(viewPortPos);

                        Vector3 screenSpaceVector = (new Vector3(offSet.x, offSet.y) - screenSpacePos).normalized * -cricleRadius;
                        screenSpaceVector = new Vector3(screenSpaceVector.x, screenSpaceVector.y * circleRadiusYCoef, screenSpaceVector.z);

                        screenSpaceVector = new Vector3(screenSpaceVector.x, screenSpaceVector.y, 0);

                        myImage.GetComponent<RectTransform>().localPosition = screenSpaceVector;
                    }
				}

                if (myImage != null && viewPortPos[0] < 0.8 && viewPortPos[0] > 0.2 && viewPortPos[1] < 0.8 && viewPortPos[1] > 0.2)
                {
					myImage.GetComponent <Image> ().enabled = false;
				}
			}
		}
    }
}