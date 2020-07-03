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
    public static MusicPlayer instance = null;
    public float TargetMusicVolume = 0.5f;
    public float AudioFadeSpeed = 1f;
    public AudioSource MusicSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            StartMusic();
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

    public void StartMusic()
    {
        StartCoroutine(StartFade(MusicSource, AudioFadeSpeed, TargetMusicVolume));
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void FinishMusic()
    {
        StartCoroutine(_FinishMusic());
    }

    IEnumerator _FinishMusic()
    {
        yield return StartFade(MusicSource, AudioFadeSpeed, 0);
        Destroy(gameObject);
    }

    static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
