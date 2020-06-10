using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndSlidingCondition
{
    Fall,
    Jump
}

public class SlidingCharacterController2D : MonoBehaviour
{
    private Vector2 _destination;
    public float SlideSpeed = 1f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _destination, SlideSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _destination) < 0.1f)
        {
            GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Fall);
        }
    }

    public void SetSlideDestination(Vector2 position)
    {
        _destination = position;
    }

    public void Jump()
    {
        Debug.Log("Jump from slide");
        GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Jump);
    }
}
