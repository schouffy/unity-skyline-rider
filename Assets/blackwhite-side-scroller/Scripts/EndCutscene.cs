using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class EndCutscene : GameTrigger
{
    public UnityEvent OnPlayerEnter;

    public GameObject Player;
    public GameObject ScriptedPlayer;
    public GameObject ScriptedEnemy;
    public AudioClip Gunshots;

    public GameObject EndScreen;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (OnPlayerEnter == null)
            OnPlayerEnter = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            // disable player
            Player.SetActive(false);
            // enable scripted player
            ScriptedPlayer.SetActive(true);
        }
    }

    public void StartEnemyAnimation()
    {
        StartCoroutine(_StartEnemyAnimation());
    }

    IEnumerator _StartEnemyAnimation()
    {
        float currentTime = 0;
        var turnAroundDuration = 0.5f;
        var rotY = ScriptedEnemy.transform.rotation.eulerAngles.y;
        while ((int)ScriptedEnemy.transform.rotation.eulerAngles.y != 270)
        {
            currentTime += Time.deltaTime;
            ScriptedEnemy.transform.rotation = Quaternion.Euler(0, Mathf.Lerp(90, 270, currentTime / turnAroundDuration), 0);
            yield return null;
        }
        
        ScriptedEnemy.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        DisplayEndScreen();
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioSource>().PlayOneShot(Gunshots);
    }

    void DisplayEndScreen()
    {
        EndScreen.SetActive(true);
    }
}
