using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTriggerToNearDespawnMode : MonoBehaviour {

	SoundManagement soundMan;
	selfDestructAfterTime despawnScript;
	public float switchAtXBeatsBeforeDespawn;
	float switchTime;
	MeshRenderer mRenderer;
	Color currentColor;
	Color startColor;


	public float sinspeed = 1;
	public float sinstark = 1;

	public bool isInNearDespawnMode = false;

	bool soundWasPlayed;

	void Start () 
	{
		soundMan = SoundManagement.instance;

		despawnScript = GetComponent<selfDestructAfterTime>();

		switchAtXBeatsBeforeDespawn = switchAtXBeatsBeforeDespawn * 60 / soundMan.effectiveBeatsPerMinute;
		//sinspeed = Time.time / SoundManagement.instance.beatIntervall;

		switchTime = despawnScript.timeToDestroy - switchAtXBeatsBeforeDespawn;
		mRenderer = GetComponent<MeshRenderer> ();
		startColor = mRenderer.material.GetColor ("_EmissionColor");

	}

	void Update () 
	{
		
		if (despawnScript.counter >= switchTime) 
		{
            //currentColor = startColor * (1f + Mathf.Sin(Time.time*sinspeed)*sinstark);
            currentColor = startColor * (1f + Mathf.Sin(Time.time / SoundManagement.instance.beatIntervall * sinspeed) * sinstark);

            mRenderer.material.SetTextureScale ("_EmissionMap", new Vector2 (0.5f, 0.5f));
			mRenderer.material.SetTextureScale ("_MainTex", new Vector2 (2f, 2f));
			mRenderer.material.SetColor ("_EmissionColor", currentColor);

			isInNearDespawnMode = true;
		}

		if (isInNearDespawnMode == true && soundWasPlayed == false) 
		{
			DetectEnteringObject detectScript = GetComponent <DetectEnteringObject> ();
			GameObject emitter = Instantiate (soundMan.soundEmitterPrefab,transform.position,Quaternion.identity);
			if (detectScript.myInstrument == Instrument.Bass) 
			{
				emitter.GetComponent <AudioSource> ().outputAudioMixerGroup = soundMan.mainMixer.FindMatchingGroups ("Bass/Weak") [0];
				emitter.GetComponent <AudioSource> ().PlayOneShot (soundMan.sounds.weakInstrumentSounds.bass [detectScript.soundPitch]);
			}
			if (detectScript.myInstrument == Instrument.Piano) 
			{
				emitter.GetComponent <AudioSource> ().outputAudioMixerGroup = soundMan.mainMixer.FindMatchingGroups ("Piano/Weak") [0];
				emitter.GetComponent <AudioSource> ().PlayOneShot (soundMan.sounds.weakInstrumentSounds.piano [detectScript.soundPitch]);
			}
			if (detectScript.myInstrument == Instrument.Chime) 
			{
				emitter.GetComponent <AudioSource> ().outputAudioMixerGroup = soundMan.mainMixer.FindMatchingGroups ("Chime/Weak") [0];
				emitter.GetComponent <AudioSource> ().PlayOneShot (soundMan.sounds.weakInstrumentSounds.chime [detectScript.soundPitch]);
			}
			if (detectScript.myInstrument == Instrument.Synth) 
			{
				emitter.GetComponent <AudioSource> ().outputAudioMixerGroup = soundMan.mainMixer.FindMatchingGroups ("Synth/Weak") [0];
				emitter.GetComponent <AudioSource> ().PlayOneShot (soundMan.sounds.weakInstrumentSounds.synth [detectScript.soundPitch]);
			}
			soundWasPlayed = true;
		}
	}
}
