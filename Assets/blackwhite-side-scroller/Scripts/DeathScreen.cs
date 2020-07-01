using Assets.blackwhite_side_scroller.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathScreen : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnKeyPress;

    private void Start()
    {
        if (OnKeyPress == null)
            OnKeyPress = new UnityEvent();
    }

    public void Show()
    {
        Constants.GameController.IncrementDeathCount();
        gameObject.SetActive(true);
        StartCoroutine(ListenToKeyPress());
    }

    private IEnumerator ListenToKeyPress()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                OnKeyPress.Invoke();
                break;
            }
            yield return null;
        }
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
