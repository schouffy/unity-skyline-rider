using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    public Text TextPresenter;
    public AudioClip ShowSound;
    public AudioClip HideSound;

    public void DisplayText(String text)
    {
        TextPresenter.text = text;
        gameObject.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(ShowSound);
    }

    public void Hide()
    {
        GetComponent<AudioSource>().PlayOneShot(HideSound);
        StartCoroutine(_Hide());
    }

    private IEnumerator _Hide()
    {
        GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
