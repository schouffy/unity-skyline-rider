﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float RandomIdleAnimationsInterval;

    // static to keep them when restarting level
    private static float _lastRandomIdleAnimationTime;
    private static float _lastInputTime; 

    public Animator Animator;
    public Rigidbody2D RigidBody;

    public Collider2D[] PlayerColliders;

    void Update()
    {
        HandleIdleAnimations();
        HandleMovementAnimation();
        HandleCrouchAnimations();
    }

    void HandleIdleAnimations()
    {
        if (HasAnyInput())
            _lastInputTime = Time.time;

        if (Time.time > Mathf.Max(_lastInputTime, _lastRandomIdleAnimationTime) + RandomIdleAnimationsInterval)
        {
            Animator.SetTrigger("Idle_random");
            _lastRandomIdleAnimationTime = Time.time;
        }
    }

    bool HasAnyInput()
    {
        if (Input.anyKey || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            return true;
        return false;
    }

    void HandleMovementAnimation()
    {
        Animator.SetFloat("Speed", Mathf.Abs(RigidBody.velocity.x) / 5.8f);
    }

    void HandleCrouchAnimations()
    {
        Animator.SetBool("Crouch", Input.GetButton("Crouch"));
    }

    public void StartJump()
    {
        Animator.SetBool("JumpEnd", false);
        Animator.SetTrigger("JumpStart");
    }

    public void LandAfterJump()
    {
        var currentClip = Animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip.Length == 1 && currentClip[0].clip != null 
            && (currentClip[0].clip.name == "JumpMid" || currentClip[0].clip.name == "Fall" || currentClip[0].clip.name == "JumpLostControl"))
            Animator.SetBool("JumpEnd", true);
    }

    public void StartFall()
    {
        Animator.SetBool("JumpEnd", false);
        Animator.SetTrigger("Fall");
    }

    public void StartClimbing(ObstacleSize obstacleSize)
    {
        Animator.SetFloat("ObstacleHeight", (float)obstacleSize);
        Animator.SetTrigger("Climb");
    }

    public void EndClimbing()
    {
    }

    public void StartFatalFall()
    {
        Animator.SetBool("FatalFall", true);
    }

}
