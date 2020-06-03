using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum ObstacleSize
{
    None = 0,
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh
}

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_BoostJumpMultiplier = 0f;                     // Multiplier of current horizontal velocity to boost forward when jumping
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    [SerializeField] private float m_groundApproachingDistance = 1f;                 // A mask determining what is ground to the character

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Climbing")]
    [Space]
    public Transform[] ObstacleRaycastStartPositions;
    public float FrontObstacleRaycastDistance = 0.2f;
    public float ClimbTransitionSpeed = 1f;


    [Header("Events")]
    [Space]
    public UnityEvent OnJumpEvent;
    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;
    public UnityEvent OnGroundApproachingEvent;

    [System.Serializable]
    public class ObstacleClimbEvent : UnityEvent<ObstacleSize> { }
    public ObstacleClimbEvent OnClimbStartEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    //public Animator UpperBodyAnimator;
    //public Animator LowerBodyAnimator;
    private Player _player;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();

        if (OnJumpEvent == null)
            OnJumpEvent = new UnityEvent();

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnGroundApproachingEvent == null)
            OnGroundApproachingEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();

        if (OnClimbStartEvent == null)
            OnClimbStartEvent = new ObstacleClimbEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if (wasGrounded && !m_Grounded && m_Velocity.y < 0)
        {
            Debug.Log("fall");
            OnFallEvent.Invoke();
        }

        Debug.DrawRay(m_GroundCheck.position, Vector2.down * m_groundApproachingDistance, Color.green);
        var ground = Physics2D.CircleCast(m_GroundCheck.position, k_GroundedRadius, Vector2.down, m_groundApproachingDistance, m_WhatIsGround);
        if (ground.collider != null)
        {
            OnGroundApproachingEvent.Invoke();
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            //if (targetVelocity != Vector3.zero)
            //{
            //	LowerBodyAnimator.SetBool("walking", true);
            //}
            //else
            //{
            //	LowerBodyAnimator.SetBool("walking", false);
            //}

            // If the input is moving the player right and the player is facing left and aiming right or not aiming...
            if (move > 0 && !m_FacingRight && (!_player.IsAiming || _player.IsAimingRight))
            {
                // ... flip the player.
                Look(true);
            }
            // Otherwise if the input is moving the player left and the player is facing right and aiming left or not aiming...
            else if (move < 0 && m_FacingRight && (!_player.IsAiming || !_player.IsAimingRight))
            {
                // ... flip the player.
                Look(false);
            }
            else if (move == 0 && _player.IsAiming)
            {
                Look(_player.IsAimingRight);
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            var obstacleSize = ObstacleSize.None;
            var obstacleApproxPosition = Vector2.zero;
            for (var i = 0; i < ObstacleRaycastStartPositions.Length; ++i)
            {
                var startPos = ObstacleRaycastStartPositions[i];
                var hit = Physics2D.Raycast(startPos.position, new Vector2(m_FacingRight ? 1 : -1, 0), FrontObstacleRaycastDistance, m_WhatIsGround);
                if (hit.collider != null)
                {
                    obstacleSize = (ObstacleSize)(i + 1);
                    obstacleApproxPosition = hit.point;
                }
            }
            if (obstacleSize != ObstacleSize.None)
            {
                // if there is an obstacle in front, climb it
                ClimbOverObstacle(obstacleSize, obstacleApproxPosition);
            }
            else
            {
                //else jump

                // Add a vertical force to the player.
                m_Grounded = false;

                // Add vertical updwards force.
                // Also, if already moving in a direction, boost in that direction
                var jumpForce = new Vector2(move * m_BoostJumpMultiplier, m_JumpForce);

                m_Rigidbody2D.AddForce(jumpForce);

                OnJumpEvent.Invoke();
            }
        }
    }

    private void ClimbOverObstacle(ObstacleSize obstacleSize, Vector2 obstacleApproxPosition)
    {
        m_Grounded = false;

        // identify where to land on the obstacle
        var hit = Physics2D.Raycast(obstacleApproxPosition + Vector2.up * 3, Vector3.down, 5, m_WhatIsGround);
        Debug.DrawRay(obstacleApproxPosition + Vector2.up * 3, Vector3.down * 5);
        if (hit.collider != null)
        {
            // animate to this location
            StartCoroutine(MoveToTargetPosition(hit.point));
            OnClimbStartEvent.Invoke(obstacleSize);
        }
    }

    private IEnumerator MoveToTargetPosition(Vector2 targetPosition)
    {
        m_Rigidbody2D.simulated = false;

        // First go up, then go forward
        var firstTargetPos = new Vector2(transform.position.x, targetPosition.y);
        var secondTargetPos = targetPosition;

        while (Vector2.Distance(transform.position, firstTargetPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, firstTargetPos, ClimbTransitionSpeed * Time.deltaTime);
            yield return null;
        }
        while (Vector2.Distance(transform.position, secondTargetPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, secondTargetPos, ClimbTransitionSpeed * Time.deltaTime);
            yield return null;
        }

        m_Rigidbody2D.simulated = true;
    }


    private void Look(bool right)
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = right;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        if (right)
            theScale.x = Mathf.Abs(theScale.x);
        else
            theScale.x = -Mathf.Abs(theScale.x);
        //theScale.x *= -1;
        transform.localScale = theScale;
    }
}
