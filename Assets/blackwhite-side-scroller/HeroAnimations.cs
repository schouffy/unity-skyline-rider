using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimations : MonoBehaviour
{
    public float RandomIdleAnimationsInterval;
    private float _lastRandomIdleAnimationTime;
    private float _lastInputTime;

    public Animator Animator;

    void Update()
    {
        HandleIdleAnimations();
        HandleMovementAnimation();
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
}
