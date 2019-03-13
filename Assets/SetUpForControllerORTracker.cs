using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpForControllerORTracker : MonoBehaviour
{
    public ControllerToUse inputDevices = ControllerToUse.two_Trackers_two_Controllers;

    public GameObject[] objectsToTrack;

    private SteamVR_ControllerManager controllerMan;

    void Awake()
    {
        controllerMan = GetComponent<SteamVR_ControllerManager>();

        //if (inputDevices == ControllerToUse.two_Trackers_two_Controllers)
        //{
        //    controllerMan.objects = new GameObject[objectsToTrack.Length - 2];

        //    controllerMan.right = objectsToTrack[0];
        //    controllerMan.left = objectsToTrack[1];

        //    objectsToTrack[0].transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
        //    objectsToTrack[1].transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
        //    objectsToTrack[0].transform.GetChild(0).transform.localPosition = new Vector3(0, -0.06f, -0.09f);
        //    objectsToTrack[1].transform.GetChild(0).transform.localPosition = new Vector3(0, -0.06f, -0.09f);

        //    for (int i = 0; i < objectsToTrack.Length - 2; i++)
        //    {
        //        controllerMan.objects[i] = objectsToTrack[i + 2];
        //    }

        //    Destroy(this.transform.GetChild(0).gameObject);
        //    Destroy(this.transform.GetChild(1).gameObject);
        //}

        if (inputDevices == ControllerToUse.two_Trackers_two_Controllers)
        {
            controllerMan.objects = new GameObject[objectsToTrack.Length];

            controllerMan.right = objectsToTrack[0];
            controllerMan.left = objectsToTrack[1];

            objectsToTrack[0].transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
            objectsToTrack[1].transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
            objectsToTrack[0].transform.GetChild(0).transform.localPosition = new Vector3(0, -0.06f, -0.09f);
            objectsToTrack[1].transform.GetChild(0).transform.localPosition = new Vector3(0, -0.06f, -0.09f);

            for (int i = 0; i < objectsToTrack.Length; i++)
            {
                controllerMan.objects[i] = objectsToTrack[i];
            }

            Destroy(this.transform.GetChild(0).gameObject);
            Destroy(this.transform.GetChild(1).gameObject);
        }
        if (inputDevices == ControllerToUse.four_Trackers)
        {
            controllerMan.objects = new GameObject[objectsToTrack.Length];

            controllerMan.left = this.transform.GetChild(0).gameObject;
            controllerMan.right = this.transform.GetChild(1).gameObject;

            for (int i = 0; i < objectsToTrack.Length; i++)
            {
                controllerMan.objects[i] = objectsToTrack[i];
            }
        }
    }
}

public enum ControllerToUse
{
    four_Trackers,
    two_Trackers_two_Controllers
}
