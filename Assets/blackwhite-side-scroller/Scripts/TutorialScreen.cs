using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    public Text TextPresenter;

    public void DisplayText(String text)
    {
        TextPresenter.text = text;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        StartCoroutine(_Hide());
    }

    private IEnumerator _Hide()
    {
        GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
