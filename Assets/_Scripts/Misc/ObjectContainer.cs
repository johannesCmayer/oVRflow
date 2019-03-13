using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    public GameObject followerParticlePrefab;

    public static ObjectContainer instance;

    [HideInInspector]
	public int collidingFollowers;
    public GameObject[] playerExtremities;
    
	GameStateManagement stateMan;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
		stateMan = GameStateManagement.instance;
    }

    void Update()
    {
        if (stateMan.gameState.primaryPhase == PrimaryPhase.DanceOver || stateMan.gameState.primaryPhase == PrimaryPhase.PreGame)
        {
            collidingFollowers = 0;
        }


    }
}