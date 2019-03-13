using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : Phase
{
    [Header("Special Phase Parameters")]
    public int numberOfPhasesToSpawn;

    protected override IEnumerator PhaseCoroutine()
    {
        suspendCompletion = true;
        
            for (int i = 0; i < numberOfPhasesToSpawn; i++)
            {
                GameObject go = SpawnFromRandomSpawnPool(Difficulty.Normal);
                yield return new WaitUntil(() => go == null);
            }

        suspendCompletion = false;
    }
}
