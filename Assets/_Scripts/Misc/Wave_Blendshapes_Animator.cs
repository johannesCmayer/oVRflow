using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Blendshapes_Animator : MonoBehaviour {

    Animator Wave;

    public int speed;



    TriggerManager triggerMan;
    public ColorID myColorID;

    public bool playStrobe;

    bool strobeLock;

    public float blendtree;

    // Use this for initialization
    void Start () {

        Wave = GetComponent<Animator>();
        triggerMan = TriggerManager.instance;
        triggerMan.cubeCriticalyDestroyedEvent += strobe;

    }

    void Update()
    {
        if (Wave.GetCurrentAnimatorStateInfo(0).IsName("transition"))
        {
            Wave.SetBool("Wave", false);
        }       
    }

    

    // Update is called once per frame

    void strobe(ColorID colorID)
    {

      
        Wave.SetBool("Wave", true) ;


        
    }

   
		
	
}
