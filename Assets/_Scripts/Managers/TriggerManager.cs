using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager instance;

    public delegate void cubeWasDestroyed(ColorID colorID);
    public cubeWasDestroyed cubeDestroyedEvent;

    public delegate void cubeWasCriticallyDestroyed(ColorID colorID);
    public cubeWasCriticallyDestroyed cubeCriticalyDestroyedEvent;

    public GameObject HUDCanvas;

    public List<GameObject> allBoxTriggers = new List<GameObject>();
    public List<GenericTriggerData> allHoldSphereTriggers = new List<GenericTriggerData>();
    public List<GenericTriggerData> allFollowers = new List<GenericTriggerData>();

    public void ClearAllTriggers()
    {
        allBoxTriggers.Clear();
        allHoldSphereTriggers.Clear();
    }

    void Awake()
    {
        TriggerManager.instance = this;
    }    
}
