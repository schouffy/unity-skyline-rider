using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator UIAnimator;
    public float TransitionTime = 1f;

    public void Awake()
    {
        var playerSpawnLocation = Constants.GameController.GetRespawnLocation();
        if (playerSpawnLocation != null)
            Constants.Player.transform.position = playerSpawnLocation.Value;
    }

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1;
        StartCoroutine(Reload());
    }

    public void LoadNextLevel()
    {
        Constants.GameController.ClearCheckpoint();
        Time.timeScale = 1;
        StartCoroutine(_LoadNextLevel());
    }

    private IEnumerator _LoadNextLevel()
    {        
        UIAnimator.SetTrigger("Start");

        // Leave time for fade to black to happen
        yield return new WaitForSeconds(TransitionTime);

        Constants.GameController.ResetDeathCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
