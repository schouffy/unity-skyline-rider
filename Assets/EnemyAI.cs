using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyWaypoint[] Waypoints;
    public float MovementSpeed = 2f;
    private bool _alive;
    private int _currentWaypointIndex;
    public bool PlayerInRange;
    private PlayerAttackable _player;
    public Transform EyePosition;
    public LayerMask RaycastToPlayerLayers;

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
    }

    IEnumerator Patrol()
    {
        while (_alive)
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
    }

    void Update()
    {
        if (PlayerInRange)
        {
            // raycast to player
            foreach (var playerPoint in _player.PointsToRaycast)
            {
                var hitInfo = Physics2D.Raycast(EyePosition.position, playerPoint.position - EyePosition.position, 50f, RaycastToPlayerLayers);
                if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Player is in range");
                    break;
                }
            }
            
        }
    }

    void LookAtDestination()
    {
        if (Waypoints[_currentWaypointIndex].transform.position.x < transform.position.x)
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        else
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    
}
