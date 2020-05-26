using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //private SpriteRenderer _spriteRenderer;
    //private float _initialScaleX;

    //public Animator UpperBodyAnimator;
    //public Animator LowerBodyAnimator;

    [HideInInspector]
    public bool IsAiming;
    [HideInInspector]
    public bool IsAimingRight;


    // Start is called before the first frame update
    void Start()
    {
        
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_initialScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var isAiming = Input.GetKey(KeyCode.Mouse1);
        //UpperBodyAnimator.SetBool("aiming", isAiming);
        //UpperBodyAnimator.SetFloat("AimAngle", AngleToBlendingAngle());

        IsAiming = isAiming;
        IsAimingRight = mouseScreenPosition.x >= transform.position.x;
    }

    float AngleToBlendingAngle()
    {
        var mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Vector2.Angle(Vector2.down, mouseScreenPosition - transform.position);

        return angle / 180f;
    }
}
