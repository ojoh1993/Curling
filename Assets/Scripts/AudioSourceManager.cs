using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour {

    private static AudioSourceManager audioSourceManager;

    public static AudioSourceManager instance
    {
        get
        {
            if (audioSourceManager == null)
            {
                audioSourceManager = FindObjectOfType(typeof(AudioSourceManager)) as AudioSourceManager;

            }
            return audioSourceManager;
        }
    }


    public AD AudioDict;

    public void Play(string sfx, AudioSource audio = null)
    {
        if (audio == null)
        {
            if (AudioDict[sfx] == null) throw new Exception("Audio is not loaded");            
        }
        else if (!AudioDict.ContainsKey(sfx))
        {
            AudioDict[sfx] = audio;   
        }

        var s = Instantiate(AudioDict[sfx]);
        StartCoroutine(PlaySound(s));
    }

    IEnumerator PlaySound(AudioSource audio)
    {
        audio.Play();
        while(audio.isPlaying) yield return 0;
        Destroy(audio.gameObject);
        yield break;
    }
}

[Serializable]
public class AD : SerializableDictionary<string, AudioSource> { };
