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
    public bool _crouch;
    public bool _jumpPressed;
    public bool _jumpProcessed;

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

        if (!_jumpPressed || _jumpProcessed)
        {
            _jumpPressed = Input.GetButtonDown("Jump");
            if (_jumpPressed)
                _jumpProcessed = false;
        }
    }

    void FixedUpdate()
    {
        _jumpProcessed = false;
        if (_controller.enabled)
            _controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jumpPressed);
        else if (_slidingController.enabled && _jumpPressed)
            _slidingController.Jump();

        if (_jumpPressed)
        {
            _jumpPressed = false;
            _jumpProcessed = true;
        }
    }
}
