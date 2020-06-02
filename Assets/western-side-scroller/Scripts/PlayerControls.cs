using Assets.blackwhite_side_scroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private CharacterController2D _controller;
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
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
    }
}
