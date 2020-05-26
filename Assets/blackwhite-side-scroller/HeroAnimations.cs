using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimations : MonoBehaviour
{
    [Range(0, 100)]
    public float PlayRandomIdleAnimationEverySeconds;
    private float _lastRandomIdleAnimationTime;

    public Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time > _lastRandomIdleAnimationTime + PlayRandomIdleAnimationEverySeconds)
        //{
        //    Animator.SetTrigger("Idle_random");
        //    _lastRandomIdleAnimationTime = Time.time;
        //}

        //Debug.Log("velocity: " + GetComponent<Rigidbody2D>().velocity);

        Animator.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) / 100);
    }
}
