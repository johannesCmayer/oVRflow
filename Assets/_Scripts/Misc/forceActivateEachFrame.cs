using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class forceActivateEachFrame : MonoBehaviour
{
    public GameObject[] activateThese;

    void Update()
    {
        foreach (GameObject a in activateThese)
        {
            if (a != null)
                a.SetActive(true);
        }
    }
}
