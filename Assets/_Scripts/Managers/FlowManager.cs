using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public static FlowManager instance;
    SoundManagement soundMan;
	GameStateManagement stateMan;

    public float flow;

    public float startFlow;

    public float maxFlow = 100f;
    public float maxOverFlow = 200f;
    public float overFlowThreshold = 150;

    public float flowLossAtBoxDespawn = 10f;
    public float flowGainAtSingleBoxHit = 10f;
    public float flowGainAtRiskyBoxHit = 30f;
    public float flowGainAtDualBoxHit = 30f;

	public bool OverFlowMode = false;

	public float flowLossRateDuringOverflow = 1f;
    
    private float loseFlowOverTimeCoef;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stateMan = GameStateManagement.instance;
        soundMan = SoundManagement.instance;
        flow = startFlow;
        soundMan.soundPlayedEvent += AddFlow;
    }

    void Update()
    {
        if (stateMan.gameState.primaryPhase == PrimaryPhase.DanceOver)
            flow = -1000;

        if (flow <= 0 && stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver && stateMan.gameState.primaryPhase != PrimaryPhase.PreGame)        
			stateMan.gameState.primaryPhase = PrimaryPhase.DanceOver;        

        if (flow <= 30 && flow > 0 && stateMan.gameState.primaryPhase != PrimaryPhase.DanceOver)
            stateMan.gameState.primaryPhase = PrimaryPhase.LowFlow;

        if (flow > 30 && stateMan.gameState.primaryPhase == PrimaryPhase.LowFlow)
            stateMan.gameState.primaryPhase = PrimaryPhase.NormalGame;

        if (flow > maxOverFlow)
			flow = maxOverFlow;

		if (flow > overFlowThreshold) 
		{
			OverFlowMode = true;
		} 
		
        if (flow <= maxFlow)
		{
			OverFlowMode = false;
		}

		if (OverFlowMode == true) 
		{
            loseFlowOverTimeCoef += Time.deltaTime*5;

            if (loseFlowOverTimeCoef > 5)
                loseFlowOverTimeCoef = 5;

            flow -= Time.deltaTime * flowLossRateDuringOverflow * loseFlowOverTimeCoef;
			stateMan.gameState.primaryPhase = PrimaryPhase.Overflow;
		} 
		else 
		{
            loseFlowOverTimeCoef = 0;

			if (stateMan.gameState.primaryPhase == PrimaryPhase.Overflow) 
			{
				stateMan.gameState.primaryPhase = PrimaryPhase.NormalGame;
			}
		}
    }

    void AddFlow(InstrumentSoundType soundType, DeathState nearDespawnState)
    {
        if (soundType == InstrumentSoundType.Normal)
        {
			if (flow <= maxFlow) 
			{
				flow += flowGainAtSingleBoxHit;
				if (flow > maxFlow) 
				{
					flow = maxFlow;
				}
			}
        }
        if (soundType == InstrumentSoundType.Strong)
        {
            if (nearDespawnState == DeathState.inNearDespawnMode)
            {
                flow += flowGainAtRiskyBoxHit;
            }
            if (nearDespawnState == DeathState.normal)
            {
                flow += flowGainAtDualBoxHit;
            }
        }
    }
}
