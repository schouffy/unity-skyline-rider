using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Object.DontDestroyOnLoad example.
//
// This script example manages the playing audio. The GameObject with the
// "music" tag is the BackgroundMusic GameObject. The AudioSource has the
// audio attached to the AudioClip.

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        foreach (AudioSource audio in GetComponents<AudioSource>().ToArray())
        {
            audio.pitch = Time.timeScale;
        }
    }
}
