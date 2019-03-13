using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickPhase : MonoBehaviour
{
    GameObject leftFootSpawner;
    GameObject rightFootSpawner;

    public float newBoxAfterXBeats;

    public float boxExistsForXBeats;

    public float riskyModeAtXBeatsBeforeDespawn;

    float boxLastsInTime;

    float counter;

    bool spawnLock;

    SoundManagement soundMan;

    int leftOrRight;

    void Start()
    {

        leftFootSpawner = transform.GetChild(0).gameObject;
        rightFootSpawner = transform.GetChild(1).gameObject;

        soundMan = SoundManagement.instance;

        boxLastsInTime = newBoxAfterXBeats * 60 / soundMan.effectiveBeatsPerMinute;

        leftFootSpawner.GetComponent<SpawnObjectsNoIntervall>().riskyModeAtXBeatsBeforeDespawn = riskyModeAtXBeatsBeforeDespawn;
        rightFootSpawner.GetComponent<SpawnObjectsNoIntervall>().riskyModeAtXBeatsBeforeDespawn = riskyModeAtXBeatsBeforeDespawn;
        leftFootSpawner.GetComponent<SpawnObjectsNoIntervall>().boxDeathAfterXBeats = boxExistsForXBeats;
        rightFootSpawner.GetComponent<SpawnObjectsNoIntervall>().boxDeathAfterXBeats = boxExistsForXBeats;
    }

    void Update()
    {

        if (counter == 0 && spawnLock == false)
        {
            spawnLock = true;
            spawnBox();
        }

        counter += Time.deltaTime;

        if (counter >= boxLastsInTime)
        {
            counter = 0;
            spawnLock = false;
        }
    }

    void spawnBox()
    {
        leftOrRight = Random.Range(0, 3);

        if (leftOrRight <= 1.5f)
        {
            leftFootSpawner.GetComponent<SpawnObjectsNoIntervall>().spawn = true;
        }

        if (leftOrRight >= 1.5)
        {
            rightFootSpawner.GetComponent<SpawnObjectsNoIntervall>().spawn = true;
        }
    }
}
