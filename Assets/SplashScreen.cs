using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplashScreen : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnKeyPress;


    private void Start()
    {
        if (OnKeyPress == null)
            OnKeyPress = new UnityEvent();

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
