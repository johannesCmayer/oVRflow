using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterPhase : Phase
{
    [Header("Phase Specific")]    
    public DifficultySection[] difficultySections;

    [Header("Debug")]
    public bool useOnlyTestingPhases;
    public GameObject[] testigPhases = new GameObject[1];
    
    [Header("Info")]
    public string activeDifficultySection = "No Section active";

    protected override IEnumerator PhaseCoroutine()
    {
        GameObject go = null;

        if (useOnlyTestingPhases)
        {
            int i = 0;
            while (true)
            {
                go = Instantiate(testigPhases[i++]);
                go.transform.SetParent(this.transform);

                if (i > testigPhases.Length - 1)
                    i = 0;

                yield return new WaitUntil(() => go == null);
            }
        }
        else
        {
            foreach (var item in sequenceSpawnPool)
            {
                go = SpawnNextInSequence();

                activeDifficultySection = "Spwaning sequence pool";

                yield return new WaitUntil(() => go == null);
            }

            while (true)
            {
                for (int i = difficultySections.Length - 1; i >= 0; i--)
                {
                    if (soundMan.totalBeats >= difficultySections[i].activeAtBeat)
                    {
                        go = Spawn(difficultySections[i].spawnDifficultyProbabilitys);

                        activeDifficultySection = difficultySections[i].name;

                        yield return new WaitUntil(() => go == null);
                        break;
                    }                    
                }
                if (go == null) Destroy(go);
                yield return null;
            }
        }
    }
    
    GameObject Spawn(SpwanDifficultyProbabilitys sDP)
    {
        GameObject go = null;
        float randomValue = Random.value;
        float totalChance = sDP.easyPhase + sDP.normalPhase + sDP.hardPhase;

        sDP.easyPhase /= totalChance;
        sDP.normalPhase /= totalChance;
        sDP.hardPhase /= totalChance;

        if (randomValue < sDP.easyPhase)
            go = SpawnFromRandomSpawnPool(Difficulty.Easy);

        else if (randomValue >= sDP.easyPhase && randomValue < sDP.normalPhase)
            go = SpawnFromRandomSpawnPool(Difficulty.Normal);

        else if (randomValue >= sDP.easyPhase + sDP.normalPhase)
            go = SpawnFromRandomSpawnPool(Difficulty.Hard);

        return go;
    }

}

[System.Serializable]
public class DifficultySection
{
    public string name = "No Name";
    public int activeAtBeat;
    public SpwanDifficultyProbabilitys spawnDifficultyProbabilitys;
}

[System.Serializable]
public class SpwanDifficultyProbabilitys
{
    public float easyPhase = 1;
    public float normalPhase;
    public float hardPhase;
}

