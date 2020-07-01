using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    public Transform LeftFoot;
    public Transform RightFoot;
    public Transform Feet;

    public GameObject FootTouchGroundWhileRunningParticleEffect;
    public GameObject JumpStartParticleEffect;
    public GameObject LandAfterJumpParticleEffect;

    private AudioSource _audio;
    public AudioClip[] Footsteps;
    public AudioClip Jump;
    public AudioClip Land;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    void LeftFootHitGround()
    {
        var particle = Instantiate(FootTouchGroundWhileRunningParticleEffect, LeftFoot.position, Quaternion.identity);
        _audio.PlayOneShot(Footsteps[0]);
    }
    void RightFootHitGround()
    {
        var particle = Instantiate(FootTouchGroundWhileRunningParticleEffect, RightFoot.position, Quaternion.identity);
        _audio.PlayOneShot(Footsteps[0]);
    }
    void JumpStart()
    {
        var particle = Instantiate(JumpStartParticleEffect, Feet.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        _audio.PlayOneShot(Jump);
    }
    void LandAfterJump()
    {
        var particle = Instantiate(LandAfterJumpParticleEffect, Feet.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        _audio.PlayOneShot(Land);
    }
}
