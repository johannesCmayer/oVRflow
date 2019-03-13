using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtility
{
    public static class AudioUtil : object
    {
        public static IEnumerator FadeLoopInCR(AudioSource audioSource, float fadeTime = 1)
        {
            float initialVolume = audioSource.volume;

            float t = 0;
            while (audioSource.volume < 1 || t < 2)
            {
                audioSource.volume += fadeTime * Time.deltaTime;

                t += fadeTime * Time.deltaTime;

                yield return null;
            }

            if (audioSource.volume > 0.95f)
                audioSource.volume = 1;
        }

        public static IEnumerator FadeLoopOutCR(AudioSource audioSource, float fadeTime = 1)
        {
            float initialVolume = audioSource.volume;

            float t = 0;
            while (audioSource.volume < 1 || t < initialVolume * 2)
            {
                audioSource.volume -= fadeTime * Time.deltaTime;

                t += fadeTime * Time.deltaTime;

                yield return null;
            }

            if (audioSource.volume < 0.05f)
                audioSource.volume = 0;
        }

        public static void FadeLoopIn(AudioSource audioSource, float fadeTime = 1)
        {
            audioSource.volume += fadeTime * Time.deltaTime;

            if (audioSource.volume > 0.95f)
                audioSource.volume = 1;
        }

        public static void FadeLoopOut(AudioSource audioSource, float fadeTime = 1)
        {
            audioSource.volume -= fadeTime * Time.deltaTime;

            if (audioSource.volume < 0.05f)
                audioSource.volume = 0;
        }
    }
}
