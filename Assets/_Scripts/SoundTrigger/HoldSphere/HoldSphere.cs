using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioUtility;

public class HoldSphere : MonoBehaviour
{

    public SoundTriggerData soundTriggerData;

    public string[] tagsToDetect;

    TriggerManager triggerMan;
    public GameObject fracturedSphere;
    
    public ColorID colorID;

    public Material matWhenActive;
    Material originalMat;
    public bool isActivated;
    public bool destroy;
    bool destroyLock = false;
    GameObject particleBurst;
    float particleCounter = 0;
    bool setParticleCounter;

    public float lifeTimeInBeats;
    float lifeTimeInTime;
    float evaluationCounter;
    Keyframe keyOne;
    Keyframe keyTwo;
    float currentlifeTimeleft = 1;

    public AnimationCurve sizeOverLifeTime;

    public float decreaseFlowBy;
    public float increaseFlowBy = 0;

    public float sizeFactorWhenActivated = 1.5f;

    public bool holdForXBeatsToWin = false;

    public float beatsNeededToWin;

    float timeNeededToWin;

    public bool resetCounterWhenSphereWasExited = false;

    float onStayCounter = 0f;

    SoundManagement soundMan;
    FlowManager flowMan;
    GameStateManagement stateMan;

    Vector3 startScale;



    Material chargingMat;

    public float timeToFullCharge;

    float currentChargeInTime;

    float currentCharge = 1.8f;

    float startHue;

    float startSat;

    float startVal;

	GameObject chargePS;

    public float maxEmission = 3;


    public bool chargeSpheres = false;






    void Start()
    {
        soundMan = SoundManagement.instance;
        stateMan = GameStateManagement.instance;
        triggerMan = TriggerManager.instance;
        flowMan = FlowManager.instance;

        startScale = transform.localScale;

        lifeTimeInTime = lifeTimeInBeats * 60 / soundMan.effectiveBeatsPerMinute;
        timeNeededToWin = beatsNeededToWin * 60 / soundMan.effectiveBeatsPerMinute;

        timeToFullCharge = timeNeededToWin;

        originalMat = GetComponent<Renderer>().material;
        particleBurst = transform.GetChild(0).gameObject;

        keyOne = new Keyframe(lifeTimeInTime, 0);

        sizeOverLifeTime.MoveKey(1, keyOne);

        GenericTriggerData gTD = new GenericTriggerData();
        gTD.trigger = this.gameObject;
        gTD.colorID = this.colorID;

        triggerMan.allHoldSphereTriggers.Add(gTD);




        chargingMat = matWhenActive;



    }



    void Update()
    {
        lifeTimeInTime = lifeTimeInBeats * 60 / soundMan.effectiveBeatsPerMinute;

        timeNeededToWin = beatsNeededToWin * 60 / soundMan.effectiveBeatsPerMinute;

        if (setParticleCounter == true)
        {
            particleCounter += Time.deltaTime;
        }

        if (destroy == true && destroyLock == false)
        {
            destroyLock = true;
            destroySphere();
        }

        currentlifeTimeleft = sizeOverLifeTime.Evaluate(evaluationCounter);

        if (isActivated == false)
        {
            transform.localScale = new Vector3(currentlifeTimeleft, currentlifeTimeleft, currentlifeTimeleft);

            flowMan.flow = flowMan.flow - decreaseFlowBy * Time.deltaTime;
        }

        if (isActivated == true)
        {
            flowMan.flow = flowMan.flow + increaseFlowBy * Time.deltaTime;
            transform.localScale = new Vector3(startScale.x * sizeFactorWhenActivated, startScale.y * sizeFactorWhenActivated, startScale.z * sizeFactorWhenActivated);
        }

        if (onStayCounter >= timeNeededToWin && holdForXBeatsToWin == true)
        {
            flowMan.flow += flowMan.flowGainAtSingleBoxHit;
            destroySphere();
        }

        if (currentlifeTimeleft <= 0.1f)
        {
            Destroy(transform.parent.gameObject);
        }

        evaluationCounter += Time.deltaTime;
    }

    void OnTriggerEnter(Collider c)
    {
        foreach (var item in tagsToDetect)
        {
            if (c.gameObject.CompareTag(item))
            {
                GetComponent<Renderer>().material = matWhenActive;
                isActivated = true;
                particleBurst.SetActive(true);
                setParticleCounter = true;
            }
        }

        if (holdForXBeatsToWin == true )
        {
		
        }
    }

    private void OnTriggerStay(Collider c)
    {
        foreach (var item in tagsToDetect)
        {
            if (c.gameObject.CompareTag(item) && holdForXBeatsToWin == true)
            {
                onStayCounter += Time.deltaTime;
            }
        }

        if (holdForXBeatsToWin == true )
        {

            currentChargeInTime += Time.deltaTime;

            currentCharge = ((3/ timeToFullCharge) * currentChargeInTime);

            matWhenActive.SetColor("_EmissionColor", Color.HSVToRGB(startHue, startSat, currentCharge));
        }
    }

    void OnTriggerExit(Collider c)
    {
        foreach (var item in tagsToDetect)
        {
            if (c.gameObject.CompareTag(item))
            {
                GetComponent<Renderer>().material = originalMat;
                isActivated = false;
                if (particleCounter >= 1f)
                {
                    particleBurst.SetActive(false);
                    particleCounter = 0;
                    setParticleCounter = false;
                }

                if (resetCounterWhenSphereWasExited == true)
                {
                    onStayCounter = 0;
                }
            }
        }

        if (holdForXBeatsToWin == true)
        {
			
        }
    }

    void destroySphere()
    {      
        triggerMan.cubeDestroyedEvent(colorID);
        soundTriggerData.playPosition = transform.position;
        soundTriggerData.soundType = SoundType.InstrumentSound;
        soundTriggerData.instrumentSoundType = InstrumentSoundType.Strong;
        soundTriggerData.soundPitch = Random.Range(0, 4);

        SoundManagement.instance.triggerQueue.Add(soundTriggerData);
        Instantiate(fracturedSphere, transform.position, Quaternion.identity);
        triggerMan.allBoxTriggers.Remove(gameObject);
        Destroy(gameObject);
    }
}

public struct GenericTriggerData
{
    public GameObject trigger;
    public ColorID colorID;
}
