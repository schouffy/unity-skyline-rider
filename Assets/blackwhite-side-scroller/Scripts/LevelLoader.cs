using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator UIAnimator;
    public float TransitionTime = 1f;

    public void RestartCurrentLevel()
    {
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        UIAnimator.SetTrigger("Start");

        // Leave time for fade to black to happen
        yield return new WaitForSeconds(TransitionTime);

        // reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
