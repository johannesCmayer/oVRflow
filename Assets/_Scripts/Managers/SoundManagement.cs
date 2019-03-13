using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
using UnityEngine.Audio;
using AudioUtility;

#region SoundTriggerData
[System.Serializable]
public class SoundTriggerData
{
    [HideInInspector]
    public Vector3 playPosition;

    [HideInInspector]
    public SoundType soundType;

    [HideInInspector]
    public int soundPitch;

    public Instrument instrument;

    [HideInInspector]
    public InstrumentSoundType instrumentSoundType;

    [HideInInspector]
    public DeathState deathState;
}

public enum DeathState
{
    normal,
    inNearDespawnMode
}

public enum SoundType
{
    InstrumentSound
}

public enum Instrument
{
    None = 0,
    Bass,
    Chime,
    Piano,
    Synth
}

public enum InstrumentSoundType
{
    None = 0,
    Weak,
    Normal,
    Strong,
}
#endregion

public enum AudiosourceLoopID
{
    BassLoopSlow = 0,
    BassLoop = 1,
    DrumLoop = 2,
    LowFlow = 3,
    Overflow = 4,
    TopWarnToneLeft = 5,
    TopWarnToneRight = 6,
    BottomWarnToneLeft = 7,
    BottomWarnToneRight = 8,
    DanceOverLoop = 9,
    LeftHandHold = 10,
    RightHandHold = 11,
    LeftFootHold = 12,
    RightFootHold = 13
}

#region Sounds
[System.Serializable]
public class Sounds
{
    public string normalLoopsDirectory;
    internal AudioClip[] normalLoops;
    public string overflowLoopsDirectory;
    internal AudioClip[] overflowLoops;
    public InstrumentSounds weakInstrumentSounds;
    public InstrumentSounds normalInstrumentSounds;
    public InstrumentSounds strongInstrumentSounds;
    public string anouncerDirectory;
    internal AudioClip[] anouncerClips;
}

[System.Serializable]
public struct InstrumentSounds
{
    public string bassDirectory;
    internal AudioClip[] bass;
    public string chimeDirectory;
    internal AudioClip[] chime;
    public string pianoDirecory;
    internal AudioClip[] piano;
    public string synthDirectory;
    internal AudioClip[] synth;
}
#endregion

[System.Serializable]
public class SoundManagement : MonoBehaviour
{
    public static SoundManagement instance;

    public AudioMixer mainMixer;

    public delegate void SoundPlayedEvent(InstrumentSoundType instrumentSoundType, DeathState deathState);
    public event SoundPlayedEvent soundPlayedEvent;

    public delegate void NextBeatBeginsHandler();
    public event NextBeatBeginsHandler nextBeatBeginsEvent;

    public delegate void NextBarBeginsHandler();
    public event NextBeatBeginsHandler nextBarBeginsEvent;

    public float effectiveBeatsPerMinute = 130;
    [HideInInspector]public float baseBeatsPerMinute = 130;
    public float overflowSpeedCoef = 1.25f;
    public float globalNoteSnapCoef = 1;

    public Sounds sounds = new Sounds();

    [Header("Setup")]
    public GameObject soundEmitterPrefab;

    [HideInInspector]
    public List<SoundTriggerData> triggerQueue = new List<SoundTriggerData>();

    [HideInInspector]
    public List<AudioSource> normalLoopAudioSources = new List<AudioSource>();
    [HideInInspector]
    public List<AudioSource> overflowLoopAudioSources = new List<AudioSource>();

    GameStateManagement stateMan;
    private TriggerManager triggerMan;

    [HideInInspector]
    public float beatIntervall;
    float noteIntervall;
    float timeAtNextBeat;

    [HideInInspector]
    public int totalBeats;

    private void Awake()
    {
        totalBeats = 0;
        instance = this;

        GetSounds();
        FillAudioSourceArrays();
        SetPannig();
    }

    private void GetSounds()
    {
        sounds.normalLoops = Array.ConvertAll(Resources.LoadAll(sounds.normalLoopsDirectory), item => (AudioClip)item);
        sounds.overflowLoops = Array.ConvertAll(Resources.LoadAll(sounds.overflowLoopsDirectory), item => (AudioClip)item);

        sounds.weakInstrumentSounds.chime = Array.ConvertAll(Resources.LoadAll(sounds.weakInstrumentSounds.chimeDirectory), item => (AudioClip)item);
        sounds.weakInstrumentSounds.piano = Array.ConvertAll(Resources.LoadAll(sounds.weakInstrumentSounds.pianoDirecory), item => (AudioClip)item);
        sounds.weakInstrumentSounds.synth = Array.ConvertAll(Resources.LoadAll(sounds.weakInstrumentSounds.synthDirectory), item => (AudioClip)item);
        sounds.weakInstrumentSounds.bass = Array.ConvertAll(Resources.LoadAll(sounds.weakInstrumentSounds.bassDirectory), item => (AudioClip)item);

        sounds.normalInstrumentSounds.chime = Array.ConvertAll(Resources.LoadAll(sounds.normalInstrumentSounds.chimeDirectory), item => (AudioClip)item);
        sounds.normalInstrumentSounds.piano = Array.ConvertAll(Resources.LoadAll(sounds.normalInstrumentSounds.pianoDirecory), item => (AudioClip)item);
        sounds.normalInstrumentSounds.synth = Array.ConvertAll(Resources.LoadAll(sounds.normalInstrumentSounds.synthDirectory), item => (AudioClip)item);
        sounds.normalInstrumentSounds.bass = Array.ConvertAll(Resources.LoadAll(sounds.normalInstrumentSounds.bassDirectory), item => (AudioClip)item);

        sounds.strongInstrumentSounds.chime = Array.ConvertAll(Resources.LoadAll(sounds.strongInstrumentSounds.chimeDirectory), item => (AudioClip)item);
        sounds.strongInstrumentSounds.piano = Array.ConvertAll(Resources.LoadAll(sounds.strongInstrumentSounds.pianoDirecory), item => (AudioClip)item);
        sounds.strongInstrumentSounds.synth = Array.ConvertAll(Resources.LoadAll(sounds.strongInstrumentSounds.synthDirectory), item => (AudioClip)item);
        sounds.strongInstrumentSounds.bass = Array.ConvertAll(Resources.LoadAll(sounds.strongInstrumentSounds.bassDirectory), item => (AudioClip)item);

        sounds.anouncerClips = Array.ConvertAll(Resources.LoadAll(sounds.anouncerDirectory), item => (AudioClip)item);
    }

    private void FillAudioSourceArrays()
    {
        for (int i = 0; i < sounds.normalLoops.Length; i++)
        {
            normalLoopAudioSources.Add(StartLoopOnGameObject(transform.GetChild(0).gameObject, sounds.normalLoops[i], 0, i));
        }

        for (int i = 0; i < sounds.overflowLoops.Length; i++)
        {
            overflowLoopAudioSources.Add(StartLoopOnGameObject(transform.GetChild(0).gameObject, sounds.overflowLoops[i], 0, i));
        }
    }

    void SetPannig()
    {
        normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft].panStereo = -1;
        normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneRight].panStereo = 1;
        normalLoopAudioSources[(int)AudiosourceLoopID.BottomWarnToneLeft].panStereo = -1;
        normalLoopAudioSources[(int)AudiosourceLoopID.BottomWarnToneRight].panStereo = 1;

        overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft].panStereo = -1;
        overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneRight].panStereo = 1;
        overflowLoopAudioSources[(int)AudiosourceLoopID.BottomWarnToneLeft].panStereo = -1;
        overflowLoopAudioSources[(int)AudiosourceLoopID.BottomWarnToneRight].panStereo = 1;
    }

    void Start()
    {
        stateMan = GameStateManagement.instance;
        triggerMan = TriggerManager.instance;
    }

    float oldBeatsPerMinute;

    int i = 3;
    void Update()
    {
        if (effectiveBeatsPerMinute != oldBeatsPerMinute)
        {
            UpdateBeatIntervall();
            oldBeatsPerMinute = effectiveBeatsPerMinute;
        }

        if (Time.time >= timeAtNextBeat)
        {
            TriggerAllQueuedSounds();
            noteIntervall = beatIntervall * globalNoteSnapCoef;

            if (nextBeatBeginsEvent != null)
                nextBeatBeginsEvent();

            if (++i > 3)
            {
                i = 0;
                if (nextBarBeginsEvent != null)
                    nextBarBeginsEvent();
            }

            timeAtNextBeat += noteIntervall;

            SetBeatCounter();
        }

        PlayMoveYourFeet();

        SetHoldSphereLoops();
        SetFollowerLoops();
    }

    private float anouncerMoveYourFeetHasPlayedCooldown;

    void PlayMoveYourFeet()
    {
        if (ObjectContainer.instance.collidingFollowers > 0 && anouncerMoveYourFeetHasPlayedCooldown <= 0)
        {
            GameObject go = Instantiate(soundEmitterPrefab);
            go.GetComponent<AudioSource>().PlayOneShot(sounds.anouncerClips[(int)AnouncerClip.MoveYourFeet]);
            anouncerMoveYourFeetHasPlayedCooldown = 5;
        }
        if (anouncerMoveYourFeetHasPlayedCooldown >= 0)
        {
            anouncerMoveYourFeetHasPlayedCooldown -= Time.deltaTime;
        }
    }

    void SetBeatCounter()
    {
        if ((stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver) && (stateMan.gameState.primaryPhase != PrimaryPhase.PreGame))
        {
            totalBeats++;
        }
        if (stateMan.gameState.primaryPhase == PrimaryPhase.PreGame)
        {
            totalBeats = 0;
        }
    }

    void UpdateBeatIntervall()
    {
        beatIntervall = 60f / effectiveBeatsPerMinute;
    }

    void SetHoldSphereLoops()
    {
        List<GenericTriggerData> toDelete = new List<GenericTriggerData>();

        foreach (var item in triggerMan.allHoldSphereTriggers)
        {
            if (item.trigger != null)
            {
                if (item.trigger.GetComponent<HoldSphere>().isActivated)
                {

                    if (stateMan.gameState.primaryPhase == PrimaryPhase.LowFlow || stateMan.gameState.primaryPhase == PrimaryPhase.NormalGame || stateMan.gameState.primaryPhase == PrimaryPhase.PreGame)
                    {
                        AudioUtil.FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                        AudioUtil.FadeLoopIn(normalLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                    }

                    if (stateMan.gameState.primaryPhase == PrimaryPhase.Overflow)
                    {

                        AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                        AudioUtil.FadeLoopIn(overflowLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                    }
                }
                else
                {
                    AudioUtil.FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                    AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                }
            }
            else
            {

                AudioUtil.FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);
                AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1]);

                if (overflowLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1].volume <= 0.05f && normalLoopAudioSources[(int)AudiosourceLoopID.LeftHandHold + (int)item.colorID - 1].volume <= 0.05f)
                {
                    toDelete.Add(item);
                }
            }
        }

        foreach (var item in toDelete)
        {
            triggerMan.allHoldSphereTriggers.Remove(item);
        }
    }

    void SetFollowerLoops()
    {
        List<GenericTriggerData> toDelete = new List<GenericTriggerData>();

        foreach (var item in triggerMan.allFollowers)
        {
            if (item.trigger != null)
            {
                if (item.trigger.GetComponent<SelfCollision>().decreaseFlowNow)
                {

                    if (stateMan.gameState.primaryPhase == PrimaryPhase.LowFlow || stateMan.gameState.primaryPhase == PrimaryPhase.NormalGame || stateMan.gameState.primaryPhase == PrimaryPhase.PreGame)
                    {
                        AudioUtil.FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                        AudioUtil.FadeLoopIn(normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                    }

                    if (stateMan.gameState.primaryPhase == PrimaryPhase.Overflow)
                    {
                        print("overflow fade in");
                        AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                        AudioUtil.FadeLoopIn(overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                    }
                }
                else
                {
                    AudioUtil.FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                    AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                }
            }
            else
            {

                AudioUtil.FadeLoopOut(overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);
                AudioUtil.FadeLoopOut(normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1]);

                if (overflowLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1].volume <= 0.05f && normalLoopAudioSources[(int)AudiosourceLoopID.TopWarnToneLeft + (int)item.colorID - 1].volume <= 0.05f)
                {
                    toDelete.Add(item);
                }
            }
        }

        foreach (var item in toDelete)
        {
            triggerMan.allFollowers.Remove(item);
        }
    }

    void TriggerAllQueuedSounds()
    {
        int bassCount = 0;
        int chimeCount = 0;
        int pianoCount = 0;
        int synthCount = 0;

        RemoveDoubleSounds(ref bassCount, ref chimeCount, ref pianoCount, ref synthCount);
        SetDoubleSoundsToStrong(ref bassCount, ref chimeCount, ref pianoCount, ref synthCount);

        foreach (var triggerQueueItem in triggerQueue)
        {
            AudioSource newAudioSource = Instantiate(soundEmitterPrefab, triggerQueueItem.playPosition, Quaternion.identity).GetComponent<AudioSource>();

            if (triggerQueueItem.deathState == DeathState.normal)
            {
                if (triggerQueueItem.soundType == SoundType.InstrumentSound)
                {
                    if (triggerQueueItem.instrumentSoundType == InstrumentSoundType.Weak)
                    {
                        if (triggerQueueItem.instrument == Instrument.Bass)
                        {
                            newAudioSource.PlayOneShot(sounds.weakInstrumentSounds.bass[triggerQueueItem.soundPitch], 0.1f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Bass/Weak")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Chime)
                        {
                            newAudioSource.PlayOneShot(sounds.weakInstrumentSounds.chime[triggerQueueItem.soundPitch], 0.4f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Chime/Weak")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Piano)
                        {
                            newAudioSource.PlayOneShot(sounds.weakInstrumentSounds.piano[triggerQueueItem.soundPitch], 0.6f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Piano/Weak")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Synth)
                        {
                            newAudioSource.PlayOneShot(sounds.weakInstrumentSounds.synth[triggerQueueItem.soundPitch], 0.4f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Synth/Weak")[0];
                        }

                        soundPlayedEvent(InstrumentSoundType.Weak, triggerQueueItem.deathState);
                    }

                    if (triggerQueueItem.instrumentSoundType == InstrumentSoundType.Normal)
                    {
                        if (triggerQueueItem.instrument == Instrument.Bass)
                        {
                            newAudioSource.PlayOneShot(sounds.normalInstrumentSounds.bass[triggerQueueItem.soundPitch], 1f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Bass/Normal")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Chime)
                        {
                            newAudioSource.PlayOneShot(sounds.normalInstrumentSounds.chime[triggerQueueItem.soundPitch], 0.4f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Chime/Normal")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Piano)
                        {
                            newAudioSource.PlayOneShot(sounds.normalInstrumentSounds.piano[triggerQueueItem.soundPitch], 0.6f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Piano/Normal")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Synth)
                        {
                            newAudioSource.PlayOneShot(sounds.normalInstrumentSounds.synth[triggerQueueItem.soundPitch], 0.4f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Synth/Normal")[0];
                        }

                        soundPlayedEvent(InstrumentSoundType.Normal, triggerQueueItem.deathState);
                    }

                    if (triggerQueueItem.instrumentSoundType == InstrumentSoundType.Strong)
                    {
                        if (triggerQueueItem.instrument == Instrument.Bass)
                        {
                            newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.bass[triggerQueueItem.soundPitch], 1f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Bass/Strong")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Chime)
                        {
                            newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.chime[triggerQueueItem.soundPitch], 0.8f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Chime/Strong")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Piano)
                        {
                            newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.piano[triggerQueueItem.soundPitch], 1f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Piano/Strong")[0];
                        }

                        if (triggerQueueItem.instrument == Instrument.Synth)
                        {
                            newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.synth[triggerQueueItem.soundPitch], 0.8f);
                            newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Synth/Strong")[0];
                        }

                        soundPlayedEvent(InstrumentSoundType.Strong, triggerQueueItem.deathState);
                    }
                }
            }
            if (triggerQueueItem.deathState == DeathState.inNearDespawnMode)
            {
                if (triggerQueueItem.instrument == Instrument.Bass)
                {
                    newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.bass[triggerQueueItem.soundPitch], 1f);
                    newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Bass/Strong")[0];
                }

                if (triggerQueueItem.instrument == Instrument.Chime)
                {
                    newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.chime[triggerQueueItem.soundPitch], 0.8f);
                    newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Chime/Strong")[0];
                }

                if (triggerQueueItem.instrument == Instrument.Piano)
                {
                    newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.piano[triggerQueueItem.soundPitch], 1f);
                    newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Piano/Strong")[0];
                }

                if (triggerQueueItem.instrument == Instrument.Synth)
                {
                    newAudioSource.PlayOneShot(sounds.strongInstrumentSounds.synth[triggerQueueItem.soundPitch], 0.8f);
                    newAudioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Synth/Strong")[0];
                }

                soundPlayedEvent(InstrumentSoundType.Strong, triggerQueueItem.deathState);
            }
        }
        triggerQueue.Clear();
    }

    void RemoveDoubleSounds(ref int bassCount, ref int chimeCount, ref int pianoCount, ref int synthCount)
    {
        int iterateFor = triggerQueue.Count;
        if (iterateFor > 0)
        {

            SoundTriggerData[] dataToRemove = new SoundTriggerData[iterateFor - 1];
            int dataPositon = 0;

            for (int i = 0; i < iterateFor; i++)
            {

                if (triggerQueue[i].instrument == Instrument.Bass)
                {
                    bassCount++;
                    if (bassCount > 1)
                    {
                        dataToRemove[dataPositon++] = triggerQueue[i];
                        continue;
                    }
                }

                if (triggerQueue[i].instrument == Instrument.Chime)
                {
                    chimeCount++;
                    if (chimeCount > 1)
                    {
                        dataToRemove[dataPositon++] = triggerQueue[i];
                        continue;
                    }
                }

                if (triggerQueue[i].instrument == Instrument.Piano)
                {
                    pianoCount++;
                    if (pianoCount > 1)
                    {
                        dataToRemove[dataPositon++] = triggerQueue[i];
                        continue;
                    }
                }

                if (triggerQueue[i].instrument == Instrument.Synth)
                {
                    synthCount++;
                    if (synthCount > 1)
                    {
                        dataToRemove[dataPositon++] = triggerQueue[i];
                        continue;
                    }
                }
            }

            foreach (var item in dataToRemove)
            {
                triggerQueue.Remove(item);
            }
        }
    }

    void SetDoubleSoundsToStrong(ref int bassCount, ref int chimeCount, ref int pianoCount, ref int synthCount)
    {
        for (int i = 0; i < triggerQueue.Count; i++)
        {
            if (triggerQueue[i].instrument == Instrument.Bass && bassCount > 1)
                triggerQueue[i].instrumentSoundType = InstrumentSoundType.Strong;

            if (triggerQueue[i].instrument == Instrument.Chime && chimeCount > 1)
                triggerQueue[i].instrumentSoundType = InstrumentSoundType.Strong;

            if (triggerQueue[i].instrument == Instrument.Piano && pianoCount > 1)
                triggerQueue[i].instrumentSoundType = InstrumentSoundType.Strong;

            if (triggerQueue[i].instrument == Instrument.Synth && synthCount > 1)
                triggerQueue[i].instrumentSoundType = InstrumentSoundType.Strong;
        }
    }

    AudioSource StartLoopOnGameObject(GameObject target, AudioClip audioClip, float volume, int iterator)
    {
        AudioSource newAudioSource = target.AddComponent<AudioSource>();
        newAudioSource.clip = audioClip;
        newAudioSource.loop = true;
        newAudioSource.Play();
        newAudioSource.volume = volume;

        AssignLoopMixerGroup(newAudioSource, iterator);

        return newAudioSource;
    }

    public enum AnouncerClip
    {
        Overflow,
        Follower,
        MoveYourFeet
    }

    void AssignLoopMixerGroup(AudioSource source, int iterator)
    {

        if (iterator == 0)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("BassLoop")[0];
        }
        if (iterator == 1)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("BassLoop")[0];
        }
        if (iterator == 2)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("DrumLoop")[0];
        }
        if (iterator == 3)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("LowFlowLoop")[0];
        }
        if (iterator == 4)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("OverflowLoop")[0];
        }
        if (iterator == 5)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("WarnLoopHigh")[0];
        }
        if (iterator == 6)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("WarnLoopHigh")[0];
        }
        if (iterator == 7)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("WarnLoopLow")[0];
        }
        if (iterator == 8)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("WarnLoopLow")[0];
        }
        if (iterator == 9)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("DanceOver")[0];
        }
        if (iterator == 10)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("HoldLoopChime")[0];
        }
        if (iterator == 11)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("HoldLoopSynth")[0];
        }
        if (iterator == 12)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("HoldLoopBass")[0];
        }
        if (iterator == 13)
        {
            source.outputAudioMixerGroup = mainMixer.FindMatchingGroups("HoldLoopPiano")[0];
        }
    }
}

