using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class DebugPhases : MonoBehaviour
{
    public bool enableDebug;

    public bool godMode;
    public float godModeHP;

    public bool activateNormalGame;

    public bool activateLowFlowPhase;

    public bool activateOverflowPhase;

    public bool triggerGameOver;

    FlowManager flowMan;
    GameStateManagement stateMan;

    private void Start()
    {
        flowMan = FlowManager.instance;
        stateMan = GameStateManagement.instance;
    }

    void Update ()
	{
        if (enableDebug)
        {
            if (activateNormalGame)
            {
                activateNormalGame = false;
                stateMan.gameState.primaryPhase = PrimaryPhase.NormalGame;
            }

            if (activateLowFlowPhase)
            {
                activateLowFlowPhase = false;
                flowMan.flow = 25;
                stateMan.gameState.primaryPhase = PrimaryPhase.LowFlow;
            }

            if (activateOverflowPhase)
            {
                activateOverflowPhase = false;
                flowMan.flow = 200;
            }

            if (godMode)
            {                
                flowMan.flow = godModeHP;
            }

            if (triggerGameOver)
            {
                triggerGameOver = false;
                flowMan.flow = -1000;
            }
        }
	}
}
#endif
