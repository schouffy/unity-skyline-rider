using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float RandomIdleAnimationsInterval;
    private float _lastRandomIdleAnimationTime;
    private float _lastInputTime;

    public Animator Animator;

    void Update()
    {
        HandleIdleAnimations();
        HandleMovementAnimation();
        HandleCrouchAnimations();
        HandleJumpAnimations();
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
        Animator.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) / 5.8f);
    }

    void HandleCrouchAnimations()
    {
        Animator.SetBool("Crouch", Input.GetButton("Crouch"));
    }

    void HandleJumpAnimations()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Animator.SetBool("JumpEnd", false);
            Animator.SetTrigger("JumpStart");
        }
    }

    public void LandAfterJump()
    {
        var currentClip = Animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip.Length == 1 && currentClip[0].clip != null && currentClip[0].clip.name == "JumpMid")
            Animator.SetBool("JumpEnd", true);
    }

    public void StartFall()
    {
        Animator.SetBool("JumpEnd", false);
        Animator.SetTrigger("Fall");
    }
}
