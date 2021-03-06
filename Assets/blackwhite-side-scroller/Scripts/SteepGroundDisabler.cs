﻿using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SteepGroundDisabler : GameTrigger
{
    public UnityEvent OnPlayerEnterEvent;

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            OnPlayerEnterEvent.Invoke();
        }
    }
}
