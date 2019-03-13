using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phase : MonoBehaviour
{
    public delegate void PhaseCompletedHandler();
    public event PhaseCompletedHandler phaseCompleted;
    
    public Difficulty difficulty;

    [Header("Spawn Pools")]
    public GameObject[] randomSpawnPool;
    public GameObject[] sequenceSpawnPool;
    
    [Header("Time")]
    [SerializeField]
    private float idleBeatsBeforSpawnig;
    [SerializeField]
    private float activeBeats = 10;
    [SerializeField]
    private float idleBeatsAfterSpawning;

    [Header("FlowSettings")]
    public bool useCustomFlowSettings;
    public float normalBoxHitFlowGain;
    public float riskyBoxHitFlowGain;
    public float doubleBoxHitFlowGain;
    public float flowLossAtBoxDespawn;
    public float flowLossDuringOverflow;

    [Header("Misc")]
    public bool destroyWhenPhaseCompleted = true;
    public bool waitUntillMyChildsAreCompleted = true;
    public bool waitUntillSequeneSpawnPoolCompletedOnce = false;
    public bool hasFollowers = false;

    protected FlowManager flowMan;
    protected SoundManagement soundMan;
    
    public bool suspendCompletion;

    protected List<GameObject> mySpawns = new List<GameObject>();
    private List<Phase> myPhaseSpawns = new List<Phase>();

    private Coroutine phaseTimingLogic;
    private Coroutine phaseLogic;
    private Coroutine phaseCoroutine;

    #region Awake
    private void Awake()
    {
        PhaseAwake();
    }

    protected virtual void PhaseAwake() { }
    #endregion

    #region Start
    private float[] initialValues = new float[5];
    private void Start()
    {
        flowMan = FlowManager.instance;
        soundMan = SoundManagement.instance;

        initialValues[0] = flowMan.flowGainAtSingleBoxHit;
        initialValues[1] = flowMan.flowGainAtRiskyBoxHit;
        initialValues[2] = flowMan.flowGainAtDualBoxHit;
        initialValues[3] = flowMan.flowLossAtBoxDespawn;
        initialValues[4] = flowMan.flowLossRateDuringOverflow;

        if (useCustomFlowSettings)
        {    
            flowMan.flowGainAtSingleBoxHit = normalBoxHitFlowGain;
            flowMan.flowGainAtRiskyBoxHit = riskyBoxHitFlowGain;
            flowMan.flowGainAtDualBoxHit = doubleBoxHitFlowGain;

            flowMan.flowLossAtBoxDespawn = flowLossAtBoxDespawn;
            flowMan.flowLossRateDuringOverflow = flowLossDuringOverflow;    
        }

        soundMan.nextBeatBeginsEvent += StartPhaseTimingLogicCroutine;

        PhaseStart();
    }

    protected void SetFlowValues()
    {
        flowMan.flowGainAtSingleBoxHit = normalBoxHitFlowGain;
        flowMan.flowGainAtRiskyBoxHit = riskyBoxHitFlowGain;
        flowMan.flowGainAtDualBoxHit = doubleBoxHitFlowGain;

        flowMan.flowLossAtBoxDespawn = flowLossAtBoxDespawn;
        flowMan.flowLossRateDuringOverflow = flowLossDuringOverflow;
    }

    protected virtual void PhaseStart() { }
    #endregion

    void StartPhaseTimingLogicCroutine()
    {
        if (this != null)
            phaseTimingLogic = StartCoroutine(PhaseTimingLogic());
        soundMan.nextBeatBeginsEvent -= StartPhaseTimingLogicCroutine;
    }

    #region Phase timing Logic
    private IEnumerator PhaseTimingLogic()
    {
        float timeStamp1 = Time.time;
        yield return new WaitUntil(() => waitForBeats(idleBeatsBeforSpawnig, timeStamp1));
        if (hasFollowers)
            PlayAnouncerFollower();

        phaseLogic = StartCoroutine(PhaseLogic());
        phaseCoroutine = StartCoroutine(PhaseCoroutine());
        float timeStamp2 = Time.time;
        yield return new WaitUntil(() => waitForBeats(activeBeats, timeStamp2));
        yield return new WaitWhile(() => suspendCompletion);

        if (waitUntillMyChildsAreCompleted)        
            yield return new WaitUntil(() => ReturnTrueWhenListIsEmty());
        

        if (waitUntillSequeneSpawnPoolCompletedOnce)
            yield return new WaitUntil(() => sequeneIsCompletedOnce);

        if (phaseLogic != null) StopCoroutine(phaseLogic);
        if (phaseCoroutine != null) StopCoroutine(phaseCoroutine);

        soundMan.nextBeatBeginsEvent -= StartPhaseTimingLogicCroutine;
        
        float timeStamp3 = Time.time;
        yield return new WaitUntil(() => waitForBeats(idleBeatsAfterSpawning, timeStamp3));

        if (phaseCompleted != null)
            phaseCompleted();

        ResetFlowValues();
        OnPhaseIsOver();

        if (destroyWhenPhaseCompleted)
            Destroy(gameObject);
    }

    private void PlayAnouncerFollower()
    {
        GameObject go = Instantiate(soundMan.soundEmitterPrefab);
        go.GetComponent<AudioSource>().PlayOneShot(soundMan.sounds.anouncerClips[1]);
    }

    #region Force phase completion
    protected void ForcePhaseCompletion()
    {
        StopCoroutine(phaseTimingLogic);
        StopCoroutine(phaseLogic);
        StopCoroutine(phaseCoroutine);

        soundMan.nextBeatBeginsEvent += initialiseFinalWait;
    }

    private void initialiseFinalWait()
    {
        StartCoroutine(WaitAndDestroy());
        soundMan.nextBeatBeginsEvent -= initialiseFinalWait;
    }

    private IEnumerator WaitAndDestroy()
    {
        float timeStamp = Time.time;
        yield return new WaitUntil(() => waitForBeats(idleBeatsAfterSpawning, timeStamp));

        Destroy(gameObject);
    }
    #endregion

    private bool ReturnTrueWhenListIsEmty()
    {
        myPhaseSpawns.RemoveAll(Phase => Phase == null);

        if (myPhaseSpawns.Count == 0)
            return true;
        else
            return false;
    }

    void ResetFlowValues()
    {
        flowMan.flowGainAtSingleBoxHit = initialValues[0];
        flowMan.flowGainAtRiskyBoxHit = initialValues[1];
        flowMan.flowGainAtDualBoxHit = initialValues[2];
        flowMan.flowLossAtBoxDespawn = initialValues[3];
        flowMan.flowLossRateDuringOverflow = initialValues[4];
    }

    protected virtual void OnPhaseIsOver() { }

    private float timer1;
    private int beatsWaited1;
    private bool waitForBeats(float beatsToWait, float timeStamp)
    {
        if (timer1 <= Time.time - timeStamp)
        {
            timer1 += soundMan.beatIntervall;
            beatsWaited1++;
        }

        if (beatsWaited1 >= beatsToWait)
        {
            timer1 = 0;
            beatsWaited1 = 0;
            return true;
        }
        else
            return false;
    }
    #endregion

    protected virtual IEnumerator PhaseCoroutine() { yield return null; }

    private IEnumerator PhaseLogic()
    {
        while (true)
        {
            PhaseUpdate();
            yield return null;
        }
    }

    protected virtual void PhaseUpdate() { }

    #region Spawn Methods
    protected GameObject SpawnFromRandomSpawnPool()
    {
        if (randomSpawnPool.Length != 0)
        {
            GameObject newObj = Instantiate(randomSpawnPool[Random.Range(0, randomSpawnPool.Length)]);

            newObj.transform.SetParent(transform);
            mySpawns.Add(newObj);

            if (newObj.GetComponent<Phase>() != null)
                myPhaseSpawns.Add(newObj.GetComponent<Phase>());

            return newObj;
        }
        else
            throw new System.Exception("Random spawn pool is empty and you are trying to Instantiate from that pool");
    }

    protected GameObject SpawnFromRandomSpawnPool(Difficulty desiredDifficulty, int searchIterations = 100)
    {
        if (randomSpawnPool.Length != 0)
        {
            GameObject newObj = null;
            for (int i = 0; i < searchIterations; i++)
            {
                newObj = Instantiate(randomSpawnPool[Random.Range(0, randomSpawnPool.Length)]);
                if (newObj.GetComponent<Phase>() != null)
                {
                    if (newObj.GetComponent<Phase>().difficulty == desiredDifficulty)
                        goto FoundMatch;
                }
                else
                {
                    goto FoundMatch;
                }

                Destroy(newObj);
            }
            Debug.LogWarning("No Matching Difficulty found, selecting rondom one.");

            FoundMatch:
            newObj.transform.SetParent(transform);
            mySpawns.Add(newObj);

            if (newObj.GetComponent<Phase>() != null)
                myPhaseSpawns.Add(newObj.GetComponent<Phase>());

            return newObj;
        }
        else
            throw new System.Exception("Random spawn pool is empty and you are trying to Instantiate from that pool");
    }
        
    protected bool sequeneIsCompletedOnce;
    int ij;
    ///<summary> Spwan the next entry in the sequense pool starting with the first </summary>
    protected GameObject SpawnNextInSequence()
    {
        if (sequenceSpawnPool.Length != 0)
        {
            GameObject newObj = Instantiate(sequenceSpawnPool[ij++]);
            newObj.transform.SetParent(transform);
            mySpawns.Add(newObj);

            if (newObj.GetComponent<Phase>() != null)
                myPhaseSpawns.Add(newObj.GetComponent<Phase>());

            if (ij > sequenceSpawnPool.Length - 1)
            {
                ij = 0;
                sequeneIsCompletedOnce = true;
            }

            return newObj;
        }
        else
            throw new System.Exception("Sequenece spawn pool is empty and you are trying to Instantiate from that pool");
    }

    ///<summary> Spwan the next entry in the sequense pool starting with the first </summary>
    protected GameObject SpawnNextInSequence(Difficulty desiredDifficulty, int searchIterations = 100)
    {
        if (sequenceSpawnPool.Length != 0)
        {
            GameObject newObj = null;
            for (int i = 0; i < searchIterations; i++)
            {
                newObj = Instantiate(randomSpawnPool[ij]);
                if (newObj.GetComponent<Phase>() != null)
                {
                    if (newObj.GetComponent<Phase>().difficulty == desiredDifficulty)
                        goto FoundMatch;
                }
                else
                {
                    goto FoundMatch;
                }

                Destroy(newObj);
            }
            Debug.LogWarning("No Matching Difficulty found, selecting rondom one.");

            FoundMatch:
            newObj.transform.SetParent(transform);
            mySpawns.Add(newObj);

            if (newObj.GetComponent<Phase>() != null)
                myPhaseSpawns.Add(newObj.GetComponent<Phase>());

            if (ij > sequenceSpawnPool.Length - 1)
            {
                ij = 0;
                sequeneIsCompletedOnce = true;
            }

            ij++;
            return newObj;
        }
        else
            throw new System.Exception("Sequenece spawn pool is empty and you are trying to Instantiate from that pool");
    }
    #endregion

    protected void SetTrueOnCmpletion(bool bl)
    {
        bl = true;
    }

    private void Reset()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
            case 1:
            case 2:
            case 3:
                print("Congratulations, you added a Phase Script.");
                break;
            case 4:
                print("YAYAAYAA more Phase Scripts!");
                break;
        }
    }
}
