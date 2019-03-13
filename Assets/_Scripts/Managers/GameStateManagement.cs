using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioUtility;

public class GameStateManagement : MonoBehaviour
{
    public static GameStateManagement instance;

    public float gameStartBoxSpawnTime = 1;
    public float gameOverCubeSpawnTime = 1;

    public GameObject gameStartBoxPrefab;
    public GameObject gameOverResetBoxPrefab;

    public GameState gameState;
    GameState oldGameState;

    public GameObject masterPhasePrefab;
    GameObject masterPhase;

    SoundManagement soundMan;
    TriggerManager triggerMan;

    AudioSource[] normalLoopAudioSources;
    AudioSource[] overflowLoopAudioSources;

    float restartTimer;
    float timeAtNextBeat;

    int i;
    bool looseEffectRunning;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        normalLoopAudioSources = SoundManagement.instance.normalLoopAudioSources.ToArray();
        overflowLoopAudioSources = SoundManagement.instance.overflowLoopAudioSources.ToArray();

        soundMan = SoundManagement.instance;
        triggerMan = TriggerManager.instance;

        StateUpdate();
    }

    private void Update()
    {
        if (looseEffectRunning)
            LooseEffectOnLoops();

        if (!gameState.Equals(oldGameState) && Time.time <= timeAtNextBeat)
        {
            StateUpdate();
            oldGameState = gameState;
        }

        recalculateTimeAtNextBeat();

        if (gameState.primaryPhase != PrimaryPhase.LowFlow)
        {
            AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.LowFlow]);
        }
    }

    void recalculateTimeAtNextBeat()
    {
        if (Time.time >= timeAtNextBeat)
        {
            timeAtNextBeat += soundMan.beatIntervall * 10;
        }
    }

    private void StateUpdate()
    {
        UpdateLoops();
        UpdateOther();
    }

    void UpdateOther()
    {
        if (gameState.primaryPhase == PrimaryPhase.PreGame)
        {
            StartCoroutine(SpwanStartGameBox());

            FlowManager.instance.flow = FlowManager.instance.startFlow;
        }

        if (gameState.primaryPhase == PrimaryPhase.NormalGame)
        {
            if (masterPhase == null)
                masterPhase = Instantiate(masterPhasePrefab);
        }

        if (gameState.primaryPhase == PrimaryPhase.Overflow)
        {
        }

        if (gameState.primaryPhase == PrimaryPhase.LowFlow)
        {
        }

        if (gameState.primaryPhase == PrimaryPhase.DanceOver)
        {
            //triggerMan.allBoxTriggers[0].GetComponent<DetectEnteringObject>().DestroyAllTriggers();

            if (masterPhase != null)
            {
                Destroy(masterPhase);
            }

            StartCoroutine(SpawnGameOverBox());
        }
    }
    

    IEnumerator SpwanStartGameBox()
    {
        yield return new WaitForSeconds(gameStartBoxSpawnTime);

        Instantiate(gameStartBoxPrefab);

        yield break;
    }

    IEnumerator SpawnGameOverBox()
    {
        yield return new WaitForSeconds(gameOverCubeSpawnTime);

        Instantiate(gameOverResetBoxPrefab);

        yield break;
    }

    Coroutine fadeInWarnsoud;
    Coroutine fadeInOverflow;

    void UpdateLoops()
    {
        if (gameState.primaryPhase == PrimaryPhase.PreGame)
        {
            looseEffectRunning = false;

            ResetAllLops(normalLoopAudioSources);

            normalLoopAudioSources[(int)AudiosourceLoopID.BassLoop].volume = 1;
        }

        if (gameState.primaryPhase == PrimaryPhase.NormalGame)
        {
            StartCoroutine(SwitchToNormalfromOverflow());
        }

        if (gameState.primaryPhase == PrimaryPhase.Overflow)
        {
            StartCoroutine(SwitchToOverflowOnBeat());
        }

        if (gameState.primaryPhase == PrimaryPhase.LowFlow)
        {

            fadeInWarnsoud = StartCoroutine(FadeLoopIn(normalLoopAudioSources[(int)AudiosourceLoopID.LowFlow]));

            normalLoopAudioSources[(int)AudiosourceLoopID.BassLoop].volume = 1;
            normalLoopAudioSources[(int)AudiosourceLoopID.DrumLoop].volume = 1;
        }

        if (gameState.primaryPhase == PrimaryPhase.DanceOver)
        {
            if(fadeInWarnsoud != null)
                StopCoroutine(fadeInWarnsoud);
            if (fadeInOverflow != null)
                StopCoroutine(fadeInOverflow);

            looseEffectRunning = true;
        }
    }
    
    IEnumerator SwitchToOverflowOnBeat()
    {
        soundMan.nextBarBeginsEvent += SetTrue;

        yield return new WaitUntil(() => nextXBegins);
        nextXBegins = false;
        soundMan.nextBarBeginsEvent -= SetTrue;

        GameObject go = Instantiate(soundMan.soundEmitterPrefab);
        go.GetComponent<AudioSource>().PlayOneShot(soundMan.sounds.anouncerClips[0]);

        ResetAllLops(normalLoopAudioSources);
        soundMan.effectiveBeatsPerMinute = soundMan.baseBeatsPerMinute * soundMan.overflowSpeedCoef;

        fadeInOverflow = StartCoroutine(FadeLoopIn(overflowLoopAudioSources[(int)AudiosourceLoopID.Overflow]));
        overflowLoopAudioSources[(int)AudiosourceLoopID.BassLoop].volume = 1;
        overflowLoopAudioSources[(int)AudiosourceLoopID.DrumLoop].volume = 1;
    }

    IEnumerator SwitchToNormalfromOverflow()
    {
        soundMan.nextBarBeginsEvent += SetTrue;

        yield return new WaitUntil(() => nextXBegins);
        nextXBegins = false;
        soundMan.nextBarBeginsEvent -= SetTrue;

        if (fadeInOverflow != null)
            StopCoroutine(fadeInOverflow);

        float overflowLoopVol = overflowLoopAudioSources[(int)AudiosourceLoopID.Overflow].volume;

        ResetAllLops(overflowLoopAudioSources);
        soundMan.effectiveBeatsPerMinute = soundMan.baseBeatsPerMinute;

        overflowLoopAudioSources[(int)AudiosourceLoopID.Overflow].volume = overflowLoopVol;

        StartCoroutine(FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.Overflow]));
        StartCoroutine(FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.LowFlow]));
        StartCoroutine(FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.Overflow]));

        normalLoopAudioSources[(int)AudiosourceLoopID.BassLoop].volume = 1;
        normalLoopAudioSources[(int)AudiosourceLoopID.DrumLoop].volume = 1;
    }

    bool nextXBegins;
    void SetTrue()
    {
        nextXBegins = true;
    }

    private void ResetAllLops(AudioSource[] loopAudioSources)
    {
        foreach (var item in loopAudioSources)
        {
            item.volume = 0;
            item.pitch = 1;
        }
    }

    IEnumerator FadeLoopIn(AudioSource audioSource)
    {
        while (audioSource.volume < 1)
        {
            audioSource.volume += 1 * Time.deltaTime;

            yield return null;
        }

        audioSource.volume = 1;
    }

    IEnumerator FadeLoopOut(AudioSource audioSource)
    {
        float initialVolume = audioSource.volume;

        while (audioSource.volume > 0.05f)
        {
            audioSource.volume -= 0.4f * Time.deltaTime;

            yield return null;
        }

        audioSource.volume = 0;
    }

    void LooseEffectOnLoops()
    {

        for (int j = 0; j < normalLoopAudioSources.Length; j++)
        {
            if (j != (int)AudiosourceLoopID.DanceOverLoop)
            {
                normalLoopAudioSources[j].volume = Mathf.Lerp(normalLoopAudioSources[j].volume, 0, 0.5f * Time.deltaTime);
                normalLoopAudioSources[j].pitch = Mathf.Lerp(normalLoopAudioSources[j].pitch, 0, 0.5f * Time.deltaTime);
            }
            else
            {
                normalLoopAudioSources[j].volume = Mathf.Lerp(normalLoopAudioSources[j].volume, 1, 0.2f * Time.deltaTime);
            }
        }
    }
}

#region GameState
[System.Serializable]
public struct GameState
{
    public PrimaryPhase primaryPhase;
}

public enum PrimaryPhase
{
    PreGame,
    NormalGame,
    Overflow,
    LowFlow,
    DanceOver
}
#endregion
