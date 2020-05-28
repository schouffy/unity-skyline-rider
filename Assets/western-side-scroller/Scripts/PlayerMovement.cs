using Assets.blackwhite_side_scroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D _controller;
    private float _horizontalMove;
    public float RunSpeed;
    public float[] SpeedSteps;

    private bool _jump;

    [Header("Debug info (read only)")]
    public float InputSpeed;
    public float StepSpeed;

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
    }

    void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
    }
}
