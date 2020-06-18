using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameUIManager : MonoBehaviour
{
    public GameObject DeathScreen;

    [Header("Events")]
    public UnityEvent OnKeyPressAfterDeath;

    private void Start()
    {
        if (OnKeyPressAfterDeath == null)
            OnKeyPressAfterDeath = new UnityEvent();
    }

    public void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
        StartCoroutine(ListenToKeyPress());
    }

    private IEnumerator ListenToKeyPress()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                OnKeyPressAfterDeath.Invoke();
                break;
            }
            yield return null;
        }
    }

    public void ShowTutorial(String text)
    {
        Debug.Log("ShowTutorial " + text);
    }
}
