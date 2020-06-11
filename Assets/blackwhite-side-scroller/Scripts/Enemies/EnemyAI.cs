using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyAIStatus
    {
        Idle,
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
    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackable>();
        _alive = true;
        if (Waypoints != null && Waypoints.Length > 0)
        {
            LookAtDestination();
            StartCoroutine(Patrol());
        }
        Status = EnemyAIStatus.Idle;
    }

    IEnumerator Patrol()
    {
        while (_alive)
        {
            if (Status == EnemyAIStatus.Idle)
            {
                var currentWaypoint = Waypoints[_currentWaypointIndex];

                transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position, MovementSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, currentWaypoint.transform.position) < 0.2f)
                {
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
        if (PlayerInRange)
        {
            if (Status == EnemyAIStatus.Idle)
            {
                if (CanPlayerBeSeen(out _))
                {
                    StartCoroutine(AimAtPlayer());
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
            if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Player")
            {
                whereToAim = hitInfo.point;
                return true;
            }
        }
        return false;
    }

    IEnumerator AimAtPlayer()
    {
        // go to aiming status
        Status = EnemyAIStatus.Aiming;

        while (Status == EnemyAIStatus.Aiming)
        {
            // aim at player
            Debug.Log("Aim at player");

            // wait for a while
            yield return new WaitForSeconds(AimTimeBeforeShoot);

            if (CanPlayerBeSeen(out var whereToShoot))
            {
                // if player is still castable, shoot it
                ShootAtPlayer(whereToShoot.Value);
            }
            else
            {
                Status = EnemyAIStatus.Idle;
            }
        }
    }

    void ShootAtPlayer(Vector2 whereToShoot)
    {
        // shoot at player
        Debug.Log("Shoot at player");

        // Instantiate bullet and FX
        bool isLookingRight = transform.localScale.x > 0;
        Instantiate(BulletPrefab, WeaponTip.position, Quaternion.LookRotation(WeaponTip.right, Vector2.up));
    }

    void LookAtDestination()
    {
        if (Waypoints[_currentWaypointIndex].transform.position.x < transform.position.x)
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        else
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }


}
