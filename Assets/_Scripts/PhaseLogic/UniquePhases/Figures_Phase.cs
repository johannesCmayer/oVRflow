using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figures_Phase : MonoBehaviour {

    public GameObject[] figures;

    GameObject[] figureChilds;
    GameObject poseLeftHand;
    GameObject poseRightHand;
    GameObject poseLeftFoot;
    GameObject poseRightFoot;

    public float flowDecrease;

    public float flowGainOnSuccess;

    public bool penalityWhenLost = false;

    public float flowLooseOnPenality;

    public int lengthOfOnePoseInBeats;

    float phasecounter;

    int phaseChoosen;

    bool phaseActive;

    GameObject currentFigure;

    int spheresNeededToSucced;

    SoundManagement soundMan;

    FlowManager flowMan;

    float lifeTimeInTime;

    public float timeBetweenPhases = 1f;

    bool safetyFirst=true;

	// Use this for initialization
	void Start () {

        soundMan = SoundManagement.instance;
        flowMan = FlowManager.instance;

        //currentFigure = Instantiate(figures[phaseChoosen], new Vector3(0, 0, 0), Quaternion.identity);
        //currentFigure.transform.parent = this.transform;

        //figureChilds = new GameObject[currentFigure.transform.childCount];

        //for (int i = 0; i < currentFigure.transform.childCount; i++)
        //{
        //    figureChilds[i] = currentFigure.transform.GetChild(i).gameObject;

        //}
    }

    // Update is called once per frame
    void Update () {


        if (phasecounter == 0 && phaseActive == false)
        {
            if (safetyFirst == true)
            {
                StartCoroutine(waitBetweenFigures());
                safetyFirst = false;
            }
        }
        phasecounter += Time.deltaTime;
        if (phaseActive == true)
        {
            checkForSuccess();
        }

        
		
	}

    IEnumerator waitBetweenFigures()
    {
        yield return new WaitForSeconds(timeBetweenPhases);
        initiatePhase();
    }

    void initiatePhase()
    {
        phaseActive = true;
        lifeTimeInTime = lengthOfOnePoseInBeats * 60 / soundMan.effectiveBeatsPerMinute;
        phaseChoosen = (Random.Range(0, figures.Length));

        currentFigure = Instantiate(figures[phaseChoosen], new Vector3 (0,0,0), Quaternion.identity);
        currentFigure.transform.parent = this.transform;

        figureChilds = new GameObject[currentFigure.transform.childCount];

        for (int i = 0; i < currentFigure.transform.childCount; i++)
        {
            figureChilds[i] = currentFigure.transform.GetChild(i).gameObject;
            
        }

        
        foreach (GameObject holdSphere in figureChilds)
        {
            holdSphere.GetComponent<HoldSphere>().decreaseFlowBy = flowDecrease;
            holdSphere.GetComponent<HoldSphere>().lifeTimeInBeats = lengthOfOnePoseInBeats;


        }
    }

    void checkForSuccess()
    {       
        foreach (GameObject holdSphere in figureChilds)
        {
            if (holdSphere != null && holdSphere.GetComponent<HoldSphere>() != null)
            {
                if (holdSphere.GetComponent<HoldSphere>().isActivated == true)
                {
                    spheresNeededToSucced += 1;
                }

            }
        }

        

        if (spheresNeededToSucced == figureChilds.Length)
        {
            phaseActive = false;
            phasecounter = 0;
            gainFlow();
            
        }

        if (spheresNeededToSucced <= figureChilds.Length)
        {
            spheresNeededToSucced = 0;
        }


        if (phasecounter >= lifeTimeInTime)
        {
            looseFlow();
            phaseActive = false;
            phasecounter = 0;
            
        }
    }

    void gainFlow()
    {
        flowMan.flow += flowGainOnSuccess;
        foreach (GameObject holdSphere in figureChilds)
        {
            if (holdSphere != null)
            {
                holdSphere.GetComponent<HoldSphere>().destroy=true;
        
            }
        }
        safetyFirst = true;
  
    }

    void looseFlow()
    {
        if(penalityWhenLost == true)
        {
            flowMan.flow -= flowLooseOnPenality;
        }
        safetyFirst = true;
    }
}
