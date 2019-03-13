using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTotalBeats : MonoBehaviour
{
    SoundManagement soundMan;
    Text beatCount;

    void Start()
    {
        soundMan = SoundManagement.instance;
        beatCount = GetComponent<Text>();
    }

    void Update()
    {
        beatCount.text = soundMan.totalBeats.ToString();
    }
}
