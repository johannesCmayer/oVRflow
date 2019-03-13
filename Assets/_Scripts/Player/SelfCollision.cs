using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfCollision : MonoBehaviour
{
    public PlayerExtremetyIDs objID;

    public float toleranceTime = 2;
    float timestamp;
    SoundManagement soundMan;
    FlowManager flowMan;
    GameStateManagement stateMan;
    TriggerManager triggerMan;

    public AnimationCurve FlowLossRate;

    public bool decreaseFlowNow = false;
    float animationCurvePosition;
    public float rampUpSpeed = 1000;
    ObjectContainer objectCon;

    bool canAddFoot = true;
    bool canSubtractFoot;

	GameObject myParticleEffect;

    void Start()
    {
        soundMan = SoundManagement.instance;
        objectCon = ObjectContainer.instance;
        stateMan = GameStateManagement.instance;
        triggerMan = TriggerManager.instance;

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        flowMan = FlowManager.instance;

        GenericTriggerData gTD = new GenericTriggerData();
        gTD.trigger = this.gameObject;
        gTD.colorID = (ColorID)this.objID + 1;

        triggerMan.allFollowers.Add(gTD);


    }

    void Update()
    {
        if (Time.time > timestamp && timestamp != 0)
        {
            if (decreaseFlowNow)
            {
                animationCurvePosition = animationCurvePosition + Time.deltaTime * rampUpSpeed * 10 / 60 / soundMan.effectiveBeatsPerMinute;

                flowMan.flow -= FlowLossRate.Evaluate(animationCurvePosition);
                if (objID == PlayerExtremetyIDs.LeftFoot || objID == PlayerExtremetyIDs.RightFoot)
                {
                    if (animationCurvePosition >= 0.25f && canAddFoot == true)
                    {
                        canAddFoot = false;
                        canSubtractFoot = true;
                        objectCon.collidingFollowers++;
                    }
                }
            }
        }
        if (objID == PlayerExtremetyIDs.LeftFoot || objID == PlayerExtremetyIDs.RightFoot)
        {
            if (animationCurvePosition < 0.25f && canSubtractFoot == true)
            {
                canSubtractFoot = false;
                canAddFoot = true;
                objectCon.collidingFollowers--;
            }
        }
        if (GameStateManagement.instance.gameState.primaryPhase == PrimaryPhase.DanceOver)
            animationCurvePosition = 0;
    }

    public enum PlayerExtremetyIDs
    {
        LeftHand,
        RightHand,
        LeftFoot,
        RightFoot
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = this.gameObject.tag;
        tag = tag.Replace("Follower", "");
        if (other.tag == tag)
        {
            decreaseFlowNow = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            timestamp = Time.time + toleranceTime;

			myParticleEffect = Instantiate(objectCon.followerParticlePrefab);
			myParticleEffect.transform.position = transform.position;
			myParticleEffect.transform.rotation = transform.rotation;
			myParticleEffect.transform.localScale = transform.localScale;
			myParticleEffect.GetComponent<setFollowerParent>().parent = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string tag = this.gameObject.tag;
        tag = tag.Replace("Follower", "");
        if (other.tag == tag)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            timestamp = 0;
            decreaseFlowNow = false;
            animationCurvePosition = 0;
        }
		Destroy (myParticleEffect);
    }

    void OnDestroy()
    {
		Destroy (myParticleEffect);
        ObjectContainer.instance.collidingFollowers = 0;
    }
}

