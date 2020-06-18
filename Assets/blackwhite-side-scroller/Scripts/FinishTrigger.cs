using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishTrigger : GameTrigger
{
    public UnityEvent OnPlayerEnter;
    public Transform EndOfLevelPosition;

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
            // stop camera follow
            Camera.main.GetComponent<SmoothFollow2D>().enabled = false;

            // move character to final destination
            collision.GetComponentInParent<CharacterController2D>().MoveToEndOfLevel(EndOfLevelPosition.position);

            OnPlayerEnter.Invoke();
        }
    }
}
