using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class ExitOnDeltaTime : MonoBehaviour
{
    public float maxDeltaTime = 1;
    public float activeAfter = 2;
    public bool exitPlayMode;
    public bool pauseGame = true;

	void Update ()
	{
        if (Time.time > activeAfter)
        {
            if (Time.deltaTime > maxDeltaTime && exitPlayMode)
                UnityEditor.EditorApplication.isPlaying = false;

            if (Time.deltaTime > maxDeltaTime && pauseGame)
                UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
#endif