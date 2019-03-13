using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameWhenDestroyed : MonoBehaviour
{
    public SoundTriggerData soundTriggerData;
    public float setBaseAndEffectiveBPMTo = 130;

    public string[] tagsToDetect;

    //TriggerBoxManager triggerMan;
    public GameObject fracturedCube;

    SoundManagement soundMan;
    
    private void Start()
    {
        //triggerMan = TriggerBoxManager.instance;
        //triggerMan.allTriggers.Add(gameObject);

        soundMan = SoundManagement.instance;
    }

    void OnTriggerEnter(Collider c)
    {
        soundTriggerData.playPosition = transform.position;
        soundTriggerData.soundType = SoundType.InstrumentSound;
        soundTriggerData.instrumentSoundType = InstrumentSoundType.Normal;
        
        foreach (var item in tagsToDetect)
        {
            if (c.gameObject.CompareTag(item))
            {
                SoundManagement.instance.triggerQueue.Add(soundTriggerData);
                Instantiate(fracturedCube, transform.position, Quaternion.identity);
                //triggerMan.allTriggers.Remove(gameObject);

                SetGameState();

                soundMan.baseBeatsPerMinute = setBaseAndEffectiveBPMTo;
                soundMan.effectiveBeatsPerMinute = setBaseAndEffectiveBPMTo;

                Destroy(gameObject);
            }
        }
    }

    internal virtual void SetGameState()
    {
        GameStateManagement.instance.gameState.primaryPhase = PrimaryPhase.NormalGame;
    }
}

