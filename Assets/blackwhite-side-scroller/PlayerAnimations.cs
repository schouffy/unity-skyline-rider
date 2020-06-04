using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float RandomIdleAnimationsInterval;
    private float _lastRandomIdleAnimationTime;
    private float _lastInputTime;

    public Animator Animator;
    public Rigidbody2D RigidBody;

    public Collider2D[] PlayerColliders;

    void Update()
    {
        HandleIdleAnimations();
        HandleMovementAnimation();
        HandleCrouchAnimations();
        //HandleJumpAnimations();
    }

    //void OnAnimatorMove()
    //{
    //    Animator animator = GetComponent<Animator>();
    //    if (animator)
    //    {
    //        Vector3 newPosition = RigidBody.transform.position;
    //        newPosition.y += animator.deltaPosition.y;
    //        newPosition.x += animator.deltaPosition.x;
    //        RigidBody.transform.position = newPosition;
    //    }
    //}

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
        if (currentClip.Length == 1 && currentClip[0].clip != null && currentClip[0].clip.name == "JumpMid")
            Animator.SetBool("JumpEnd", true);
    }

    public void StartFall()
    {
        Animator.SetBool("JumpEnd", false);
        Animator.SetTrigger("Fall");
    }

    public void StartClimbing(ObstacleSize obstacleSize)
    {
        Debug.Log("Start climbing animation " + obstacleSize + " value: " + (float)obstacleSize);

        //Animator.applyRootMotion = true;
        //RigidBody.simulated = false;
        //RigidBody.isKinematic = true;
        //for (var i = 0; i < PlayerColliders.Length; ++i)
        //{
        //    PlayerColliders[i].gameObject.SetActive(false);
        //}

        Animator.SetFloat("ObstacleHeight", (float)obstacleSize);
        Animator.SetTrigger("Climb");
    }

    public void EndClimbing()
    {
        Debug.Log("End climbing");
        
        //for (var i = 0; i < PlayerColliders.Length; ++i)
        //{
        //    PlayerColliders[i].gameObject.SetActive(true);
        //}
        //RigidBody.simulated = true;
        //RigidBody.isKinematic = false;
        //Animator.applyRootMotion = false;
    }

}
