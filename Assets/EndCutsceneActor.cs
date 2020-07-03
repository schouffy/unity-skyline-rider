using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndCutsceneActor : MonoBehaviour
{
    public UnityEvent OnReloadEnd;
    public AudioClip Reload;

    public GameObject WeaponOnGround;
    public GameObject WeaponInHand;

    public void GunAcquired()
    {
        WeaponOnGround?.SetActive(false);
        WeaponInHand?.SetActive(true);
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
