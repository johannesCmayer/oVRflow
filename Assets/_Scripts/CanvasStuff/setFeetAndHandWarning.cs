using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setFeetAndHandWarning : MonoBehaviour {

    GameObject warningHand;
    GameObject warningFoot;
	GameObject warningHand2;
	GameObject warningFoot2;


    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftFoot;
    public GameObject rightFoot;


    TriggerManager triggerMan;

	// Use this for initialization
	void Start () {

		warningHand = transform.GetChild(0).transform.GetChild(1).gameObject;
		warningHand2 = transform.GetChild(0).transform.GetChild(3).gameObject;
		warningFoot = transform.GetChild(0).transform.GetChild(2).gameObject;
		warningFoot2 = transform.GetChild(0).transform.GetChild(4).gameObject;


        triggerMan = TriggerManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	/*	if (triggerMan.allBoxTriggers.Contains(leftHand) || triggerMan.allBoxTriggers.Contains(rightHand))

        {

            warningHand.SetActive(true);
			warningHand2.SetActive (true);
        }

        else
        {
            warningHand.SetActive(false);
			warningHand2.SetActive(false);
        }



        if (triggerMan.allBoxTriggers.Contains(leftFoot) || triggerMan.allBoxTriggers.Contains(rightFoot))

        {

            warningFoot.SetActive(true);
			warningFoot2.SetActive(true);
        }

        else
        {
            warningFoot.SetActive(false);
			warningFoot2.SetActive(false);
        }

		*/


	 warningHand.SetActive (false);
		warningHand2.SetActive (false);

        warningFoot.SetActive(false);
        warningFoot2.SetActive(false);

        foreach (GameObject box in triggerMan.allBoxTriggers) {

			if (box.tag == "LeftHandTrigger" || box.tag == "RightHandTrigger") {

				warningHand.SetActive (true);
				warningHand2.SetActive (true);

			}
		}


        foreach (GameObject box in triggerMan.allBoxTriggers)
        {

            if (box.tag == "LeftFootTrigger" || box.tag == "RightFootTrigger")
            {

                warningFoot.SetActive(true);
                warningFoot2.SetActive(true);

            }
        }


       

        foreach (GenericTriggerData sphere in triggerMan.allHoldSphereTriggers) {

            if (sphere.trigger.gameObject != null)

            {

                if (sphere.trigger.gameObject.tag == "LeftHandTrigger" || sphere.trigger.gameObject.tag == "RightHandTrigger")
                {

                    warningHand.SetActive(true);
                    warningHand2.SetActive(true);

                }

            }
		}


        foreach (GenericTriggerData sphere in triggerMan.allHoldSphereTriggers)
        {

            if (sphere.trigger.gameObject != null)

            {

                if (sphere.trigger.gameObject.tag == "LeftFootTrigger" || sphere.trigger.gameObject.tag == "RightFootTrigger")
                {

                    warningFoot.SetActive(true);
                    warningFoot2.SetActive(true);

                }

            }
        }


    }
}
