using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManagement : MonoBehaviour
{
    public static SpawnerManagement instance;

    [SerializeField]
    private GameObject[] normalSpawners;

    private void Awake()
    {
        instance = this;
    }    
}
