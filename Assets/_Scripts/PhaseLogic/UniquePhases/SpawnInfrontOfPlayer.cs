using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInfrontOfPlayer : Phase
{
    protected override IEnumerator PhaseCoroutine()
    {
        foreach (var item in sequenceSpawnPool)
        {
            GameObject go = SpawnNextInSequence();

            yield return new WaitUntil(() => go == null);
        }
    }
}
