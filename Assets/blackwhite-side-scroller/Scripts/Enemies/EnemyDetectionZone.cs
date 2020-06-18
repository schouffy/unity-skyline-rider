using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    private EnemyAI _enemyAI;

    private void Start()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            _enemyAI.PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            _enemyAI.PlayerInRange = false;
        }
    }
}
