using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight_Control : MonoBehaviour
{
    GameObject ledPanel;
    GameObject spotlight;
    public float strobe_speed;

    public AnimationCurve flashCurve;
    public float curvePosition;
    TriggerManager triggerMan;
    public ColorID myColorID;

    void Start()
    {
        ledPanel = transform.GetChild(1).gameObject;
        spotlight = ledPanel.transform.GetChild(0).gameObject;

        triggerMan = TriggerManager.instance;
        triggerMan.cubeDestroyedEvent += strobe;
    }

    void Update()
    {

        curvePosition = curvePosition + Time.deltaTime * strobe_speed;
        spotlight.GetComponent<Light>().intensity = flashCurve.Evaluate(curvePosition);
        if (curvePosition >= 1)
        {
            ledPanel.SetActive(false);
            spotlight.SetActive(false);
        }
    }

    public void strobe(ColorID colorID)
    {
        if (myColorID == colorID)
        {
            ledPanel.SetActive(true);
            spotlight.SetActive(true);
            curvePosition = 0f;
        }
    }
}
