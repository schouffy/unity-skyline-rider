using Assets.blackwhite_side_scroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private CharacterController2D _controller;
    private SlidingCharacterController2D _slidingController;
    private float _horizontalMove;
    public float RunSpeed;
    public float[] SpeedSteps;

    

    [Header("Debug info (read only)")]
    public float InputSpeed;
    public float StepSpeed;
    public bool _jump;
    public bool _crouch;

    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _slidingController = GetComponent<SlidingCharacterController2D>();
    }

    void Update()
    {
        InputSpeed = Input.GetAxis("Horizontal");
        StepSpeed = SpeedSteps.ClosestTo(Mathf.Abs(InputSpeed)) * (InputSpeed > 0 ? 1 : -1);
        _horizontalMove = StepSpeed * RunSpeed;

        _jump = Input.GetButtonDown("Jump");
        _crouch = Input.GetButton("Crouch");
    }

    void FixedUpdate()
    {
        if (_controller.enabled)
            _controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
        else if (_slidingController.enabled && _jump)
            _slidingController.Jump();

        _jump = false;
    }
}
