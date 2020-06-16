using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

public enum ObstacleSize
{
    None = 0,
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
    Above1,
    Above2,
    Above3,
    Above4,
    Above5,
    Above6
}

public class CharacterController2D : MonoBehaviour
{
    public PlayerAnimations Animator;
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_BoostJumpMultiplier = 0f;                     // Multiplier of current horizontal velocity to boost forward when jumping
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private CircleCollider2D m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    [SerializeField] private float m_groundApproachingDistance = 1f;                 // A mask determining what is ground to the character
    public float MaxVerticalVelocityBeforeFallIsFatal;
    public Vector2 JumpFromSlidingForce;

    private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_Climbing;
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public bool FacingRight => m_FacingRight;
    private Vector3 m_Velocity = Vector3.zero;
    private bool _hasJumped;
    private bool _dieOnLand;
    private bool _isControllable;


    [Header("Climbing")]
    [Space]
    public Transform[] ObstacleRaycastStartPositions;
    public float FrontObstacleRaycastDistance = 0.2f;
    public float ClimbTransitionSpeed = 1f;
    public float MaxClimbDistanceIfObstacleIsTooHigh = 2f;
    public float MaxClimbDistanceAfterJumpIfObstacleIsTooHigh = 1f;
    public float MinIntervalBetweenTwoClimbsOnSameObstacle = 0.8f;
    private bool _hasAlreadyClimbedOnce = false;
    private float _lastClimbTime;


    [Header("Events")]
    [Space]
    public UnityEvent OnJumpEvent;
    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;
    public UnityEvent OnDieEvent;
    public UnityEvent OnGroundApproachingEvent;
    public UnityEvent OnSlideStartEvent;

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
        _isControllable = true;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();

        if (OnJumpEvent == null)
            OnJumpEvent = new UnityEvent();

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnDieEvent == null)
            OnDieEvent = new UnityEvent();

        if (OnGroundApproachingEvent == null)
            OnGroundApproachingEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();

        if (OnClimbStartEvent == null)
            OnClimbStartEvent = new ObstacleClimbEvent();

        if (OnSlideStartEvent == null)
            OnSlideStartEvent = new UnityEvent();
    }

    private void Update()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.transform.position, m_GroundCheck.radius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                _hasAlreadyClimbedOnce = false;
                
                _hasJumped = false;
                if (!wasGrounded)
                    OnLandEvent.Invoke();

                if (colliders[i].sharedMaterial != null && colliders[i].sharedMaterial.name == "SlidingSteep")
                {
                    StartSliding(colliders[i].GetComponent<SteepGround>());
                }
                if (_dieOnLand)
                {
                    Die();
                }
            }
        }

        
        Debug.DrawRay(m_GroundCheck.transform.position, Vector2.down * m_groundApproachingDistance, Color.green);
        var ground = Physics2D.CircleCast(m_GroundCheck.transform.position, m_GroundCheck.radius, Vector2.down, m_groundApproachingDistance, m_WhatIsGround);
        if (ground.collider != null)
        {
            OnGroundApproachingEvent.Invoke();
        }
        else if (wasGrounded && !m_Grounded && !_hasJumped && !m_Climbing)
        {
            Debug.Log("fall");
            OnFallEvent.Invoke();
        }

        if (!m_Grounded && -m_Rigidbody2D.velocity.y > MaxVerticalVelocityBeforeFallIsFatal && !_dieOnLand)
        {
            Debug.Log("fatal fall + " + (-m_Rigidbody2D.velocity.y));
            _isControllable = false;
            _dieOnLand = true;
            Animator.StartFatalFall();
        }
    }

    public void StartSliding(SteepGround steepGroundToSlideOn)
    {
        if (_dieOnLand)
        {
            Debug.Log("Cancelling fatal fall");
            Animator.CancelFatalFall();
            _dieOnLand = false; // Can't die from jumping on steep surfaces
            _isControllable = true;
        }
        m_Rigidbody2D.simulated = false;
        GetComponent<SlidingCharacterController2D>().enabled = true;
        GetComponent<SlidingCharacterController2D>().SetSlideDestination(steepGroundToSlideOn.SlideDestination.position);
        enabled = false;
        Animator.StartSliding();
    }

    public void EndSliding(EndSlidingCondition condition)
    {
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<SlidingCharacterController2D>().enabled = false;
        enabled = true;
        Animator.EndSliding();

        if (condition == EndSlidingCondition.Jump)
        {
            Debug.Log("Adding jump from slide force : " + JumpFromSlidingForce);

            m_Grounded = false;
            m_Rigidbody2D.velocity = JumpFromSlidingForce;
            OnJumpEvent.Invoke();
        }
    }

    public void Die()
    {
        _isControllable = false;
        OnDieEvent.Invoke();
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!_isControllable)
            return;

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
            _hasJumped = true;

            if (!DetectAndClimbObstacle())
            {
                //no obstacle:  jump
                // Add vertical updwards force.
                // Also, if already moving in a direction, boost in that direction
                var jumpForce = new Vector2(move * m_BoostJumpMultiplier, m_JumpForce);
                Jump(jumpForce);
            }
        }
        else if (!m_Grounded && !m_Climbing && Input.GetButton("Jump"))
        {
            // If obstacle is encountered mid-air and jump button is down, jump over it
            DetectAndClimbObstacle();
        }
    }

    private void Jump(Vector2 jumpForce, ForceMode2D forceMode2D = ForceMode2D.Force)
    {
        // Add a vertical force to the player.
        m_Grounded = false;
        m_Rigidbody2D.AddForce(jumpForce, forceMode2D);
        OnJumpEvent.Invoke();
    }

    private bool DetectAndClimbObstacle()
    {
        if (_hasAlreadyClimbedOnce || _lastClimbTime + MinIntervalBetweenTwoClimbsOnSameObstacle > Time.time)
            return true;

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
            return true;
        }
        return false;
    }

    private void ClimbOverObstacle(ObstacleSize obstacleSize, Vector2 obstacleApproxPosition)
    {
        _hasAlreadyClimbedOnce = true;
        _lastClimbTime = Time.time;

        if ((obstacleSize == ObstacleSize.Above6 && m_Grounded)
            || (obstacleSize >= ObstacleSize.Above3 && !m_Grounded))
        {
            // if obstacle is too high, we can't go on top of it but we still try to climb it
            //Debug.Log("Too high to climb.");

            m_Climbing = true;
            // animate to this location
            m_Rigidbody2D.velocity = Vector2.zero;
            var targetPosition = new Vector2(transform.position.x, transform.position.y + (m_Grounded ? MaxClimbDistanceIfObstacleIsTooHigh : MaxClimbDistanceAfterJumpIfObstacleIsTooHigh));
            StartCoroutine(MoveToTargetPosition(targetPosition));
            OnClimbStartEvent.Invoke(obstacleSize);
        }
        else
        {
            // identify where to land on the obstacle
            var hit = Physics2D.Raycast(obstacleApproxPosition + Vector2.up * 3, Vector3.down, 5, m_WhatIsGround);
            Debug.DrawRay(obstacleApproxPosition + Vector2.up * 3, Vector3.down * 5);
            if (hit.collider != null)
            {
                m_Climbing = true;
                // animate to this location
                m_Rigidbody2D.velocity = Vector2.zero;
                StartCoroutine(MoveToTargetPosition(hit.point));
                OnClimbStartEvent.Invoke(obstacleSize);
            }
        }

        m_Grounded = false;
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

        m_Climbing = false;
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
