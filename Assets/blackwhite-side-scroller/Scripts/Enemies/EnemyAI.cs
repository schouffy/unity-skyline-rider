using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyAIStatus
    {
        Idle,
        Suspicious,
        Aiming
    }

    public EnemyWaypoint[] Waypoints;
    public float MovementSpeed = 2f;
    private bool _alive;
    private int _currentWaypointIndex;
    public bool PlayerInRange;
    private bool PlayerCanBeSeen;
    private PlayerAttackable _player;
    public Transform EyePosition;
    public LayerMask RaycastToPlayerLayers;
    public EnemyAIStatus Status;
    public float AimTimeBeforeShoot = 1f;
    public Transform WeaponTip;
    public GameObject BulletPrefab;
    public Animator Animator;
    public int SuspicionLevel; // percent
    private float SuspicionLevelFloat; // percent
    private int SuspicionIncreaseRate = 50; // how much suspicion can change in 0.1 second, if player is at 1 meter distance
    private int SuspicionDecreaseRate = 5;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerAttackable>();
        _alive = true;
        if (Waypoints != null && Waypoints.Length > 0)
        {
            LookAtDestination();
            StartCoroutine(Patrol());
        }
        Status = EnemyAIStatus.Idle;
        SuspicionLevel = 0;
    }

    IEnumerator Patrol()
    {
        while (_alive)
        {
            if (Status == EnemyAIStatus.Idle)
            {
                var currentWaypoint = Waypoints[_currentWaypointIndex];

                Animator.SetFloat("Speed", MovementSpeed);
                transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position, MovementSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, currentWaypoint.transform.position) < 0.2f)
                {
                    Animator.SetFloat("Speed", 0);
                    yield return new WaitForSeconds(currentWaypoint.WaitTime);
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % Waypoints.Length;
                    LookAtDestination();
                }
                yield return 0;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void Update()
    {
        // idle -> suspicious -> aiming
        if (PlayerInRange)
        {
            if (Status == EnemyAIStatus.Idle)
            {
                if (CanPlayerBeSeen(out _))
                {
                    StartCoroutine(GetSuspicious());
                }
            }
        }
    }

    bool CanPlayerBeSeen(out Vector2? whereToAim)
    {
        whereToAim = null;
        foreach (var playerPoint in _player.PointsToRaycast)
        {
            PlayerCanBeSeen = false;
            var hitInfo = Physics2D.Raycast(EyePosition.position, playerPoint.position - EyePosition.position, 50f, RaycastToPlayerLayers);
            if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == Constants.TagPlayer)
            {
                whereToAim = hitInfo.point;
                return true;
            }
        }
        return false;
    }

    float AimAngleToBlendingAimAngle(Vector2 aimPosition)
    {
        var angle = Vector2.Angle(Vector2.down, aimPosition - (Vector2)WeaponTip.position);

        return angle;
    }

    IEnumerator GetSuspicious()
    {
        Status = EnemyAIStatus.Suspicious;

        if (SuspicionLevel <= 0)
            SuspicionLevel = 1;
        else if (SuspicionLevel == 100)
            SuspicionLevel = 99;

        SuspicionLevelFloat = SuspicionLevel;
        while (SuspicionLevel > 0 && SuspicionLevel < 100)
        {
            if (CanPlayerBeSeen(out _))
            {
                var distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);
                SuspicionLevelFloat += SuspicionIncreaseRate / distanceFromPlayer;
                SuspicionLevel = (int)SuspicionLevelFloat;
            }
            else
                SuspicionLevel -= SuspicionDecreaseRate;
            yield return new WaitForSeconds(0.1f);
        }

        if (SuspicionLevel <= 0)
        {
            Debug.Log("Lost sight of enemy. Idle.");
            SuspicionLevel = 0;
            Status = EnemyAIStatus.Idle;
        }
        else if (SuspicionLevel >= 100)
        {
            SuspicionLevel = 100;
            Debug.Log("Enemy in sight. Aim.");
            Status = EnemyAIStatus.Aiming;
            StartCoroutine(AimContinuouslyAtPlayer());
            StartCoroutine(AimAndShootAtPlayer());
        }
    }

    IEnumerator AimContinuouslyAtPlayer()
    {
        // go to aiming status
        Status = EnemyAIStatus.Aiming;

        while (Status == EnemyAIStatus.Aiming)
        {
            if (CanPlayerBeSeen(out var whereToShoot))
            {
                // aim at player
                Animator.SetFloat("Speed", 0);
                Animator.SetBool("IsAiming", true);
                Animator.SetFloat("AimAngle", AimAngleToBlendingAimAngle(whereToShoot.Value));
            }
            yield return new WaitForSeconds(0.1f);
        }
        Animator.SetBool("IsAiming", false);
    }

    IEnumerator AimAndShootAtPlayer()
    {
        // go to aiming status
        Status = EnemyAIStatus.Aiming;

        while (Status == EnemyAIStatus.Aiming)
        {
            // wait for a while
            yield return new WaitForSeconds(AimTimeBeforeShoot);

            if (CanPlayerBeSeen(out var whereToShoot))
            {
                // if player is still castable, shoot it
                ShootAtPlayer(whereToShoot.Value);
            }
            else
            {
                // Can't see player anymore. Revert to suspicious
                StartCoroutine(GetSuspicious());
            }
        }
    }

    void ShootAtPlayer(Vector2 whereToShoot)
    {
        // Instantiate bullet and FX
        bool isLookingRight = transform.localScale.x > 0;
        var bullet = Instantiate(BulletPrefab, WeaponTip.position, Quaternion.identity);
        bullet.transform.right = whereToShoot - (Vector2)WeaponTip.position;

        Animator.SetTrigger("Shoot");
    }

    void LookAtDestination()
    {
        if (Waypoints == null || Waypoints.Length == 0)
            return;

        if (Waypoints[_currentWaypointIndex].transform.position.x < transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

    void OnDrawGizmos()
    {
        Handles.Label(transform.position, "Suspicion :" + SuspicionLevel);
    }

}
