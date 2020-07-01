using Assets.blackwhite_side_scroller.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FinishScreen : MonoBehaviour
{
    public Text StatsText;

    [Header("Events")]
    public UnityEvent OnKeyPress;

    public AudioClip Sound;

    private void Start()
    {
        if (OnKeyPress == null)
            OnKeyPress = new UnityEvent();
    }

    public void Show()
    {
        String displayCompletionTime = Math.Round(Constants.GameController.GetElapsedTime(), 2) + " seconds";
        StatsText.text = $"Time: {displayCompletionTime}\nDeath count: {Constants.GameController.GetDeathCount()}";
        gameObject.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(Sound);
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
}
