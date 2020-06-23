using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointTrigger : GameTrigger
{
    public UnityEvent OnPlayerEnter;
    public Transform RespawnLocation;

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
            GetComponent<Animator>().SetTrigger("activate");
            OnPlayerEnter.Invoke();
        }
    }
}
