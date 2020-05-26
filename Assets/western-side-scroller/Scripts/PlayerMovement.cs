using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D _controller;
    private float _horizontalMove;
    public float RunSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * RunSpeed;
    }

    void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, false, false);
    }
}
