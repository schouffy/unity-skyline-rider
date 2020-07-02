using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndCutsceneActor : MonoBehaviour
{
    public UnityEvent OnReloadEnd;
    public AudioClip Reload;
    
    public void GunAcquired()
    {
        Debug.Log("Gun acquired");
    }

    public void ReloadStart()
    {
        GetComponent<AudioSource>().PlayOneShot(Reload);
    }

    public void ReloadEnd()
    {
        OnReloadEnd.Invoke();
    }
}
