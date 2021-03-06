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
        var speed = Mathf.Abs(RigidBody.velocity.x) / 5.8f;
        if (speed > 0.1f)
            Animator.ResetTrigger("TurnAround");
        Animator.SetFloat("Speed", speed);
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
        Animator.ResetTrigger("JumpStart");
        if (Animator.GetBool("JumpEnd"))
            return;

        var jumpMomentum = RigidBody.velocity;
        if (jumpMomentum.x == 0 && jumpMomentum.y > -6)
            Animator.SetFloat("JumpMomentum", 0);
        else if (Mathf.Abs(jumpMomentum.x) > 8)
            Animator.SetFloat("JumpMomentum", 1);
        else if (jumpMomentum.y < -11)
            Animator.SetFloat("JumpMomentum", 1);
        else
            Animator.SetFloat("JumpMomentum", ((Mathf.Abs(RigidBody.velocity.x) + Mathf.Abs(RigidBody.velocity.y)) / 30f));

        //var currentClip = Animator.GetCurrentAnimatorClipInfo(0);
        //if (currentClip.Length == 1 && currentClip[0].clip != null 
        //    && (currentClip[0].clip.name != "JumpEnd"))//currentClip[0].clip.name == "JumpMid" || currentClip[0].clip.name == "Fall" || currentClip[0].clip.name == "JumpLostControl"))
        Animator.SetBool("JumpEnd", true);
    }

    public void StartFall()
    {
        Animator.SetBool("JumpEnd", false);
        Animator.SetTrigger("Fall");
    }

    public void StartClimbing(ObstacleSize obstacleSize)
    {
        Animator.ResetTrigger("JumpStart");
        Animator.SetFloat("ObstacleHeight", (float)obstacleSize);
        Animator.SetBool("IsClimbing", true);
        Animator.SetTrigger("Climb");
    }

    public void EndClimbing()
    {
        Animator.SetBool("IsClimbing", false);
    }

    public void StartFatalFall()
    {
        Animator.SetBool("FatalFall", true);
    }
    public void CancelFatalFall()
    {
        Animator.SetBool("FatalFall", false);
    }

    public void StartSliding()
    {
        Animator.SetBool("Sliding", true);
    }

    public void EndSliding()
    {
        Animator.SetBool("Sliding", false);
    }

    public void SetSpeed(float speed)
    {
        Animator.SetFloat("Speed", speed);
    }

    public void TurnAround()
    {
        Animator.SetTrigger("TurnAround");
    }

}
