using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameOverWhenDestroyed : StartGameWhenDestroyed
{
    internal override void SetGameState()
    {
        GameStateManagement.instance.gameState.primaryPhase = PrimaryPhase.PreGame;
    }
}
