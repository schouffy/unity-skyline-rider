using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SteepGroundEndTrigger : GameTrigger
{
    public UnityEvent OnPlayerEnter;

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
            OnPlayerEnter.Invoke();
        }
    }
}
