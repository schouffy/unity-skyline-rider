using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndScreen : MonoBehaviour
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
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (Input.anyKeyDown || time > 10)
            {
                OnKeyPress.Invoke();
                break;
            }
            yield return null;
        }
    }
}
