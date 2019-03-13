using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSS_SpawnStreamOfSingleBoxFarAway : Phase
{
    [Header("Phase Specific")]
    public float waitBetweenStreams = 2;

    protected override IEnumerator PhaseCoroutine()
    {
        while (true)
        {
            SpawnFromRandomSpawnPool();
            yield return new WaitForSeconds(waitBetweenStreams);
        }
    }
}
