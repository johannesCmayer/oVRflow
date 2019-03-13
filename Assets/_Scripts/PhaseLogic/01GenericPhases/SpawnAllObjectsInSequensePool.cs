using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAllObjectsInSequensePool : Phase {

    protected override void PhaseStart()
    {
        waitUntillSequeneSpawnPoolCompletedOnce = true;
    }

    protected override IEnumerator PhaseCoroutine()
    {               
        foreach (var item in sequenceSpawnPool)
        {
            GameObject go = SpawnNextInSequence();
            yield return new WaitUntil(() => go == null);
        }     
    }
}
