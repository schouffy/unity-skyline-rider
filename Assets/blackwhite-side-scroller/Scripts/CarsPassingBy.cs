using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsPassingBy : MonoBehaviour
{
    public AudioClip[] Sounds;

    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(Sounds[Random.Range(0, Sounds.Length - 1)]);
    }
}
