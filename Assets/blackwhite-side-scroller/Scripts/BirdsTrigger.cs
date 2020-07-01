using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BirdsTrigger : GameTrigger
{
    public GameObject BirdsOnGround;
    public ParticleSystem BirdsParticleSystem;
    private bool _alreadyFlownAway;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_alreadyFlownAway)
            return;

        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            BirdsParticleSystem.Play();
            GetComponent<AudioSource>().Play();
            BirdsOnGround.SetActive(false);
            _alreadyFlownAway = true;
        }
    }
}
