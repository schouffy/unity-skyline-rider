using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackable : MonoBehaviour
{
    public Transform[] PointsToRaycast;

    public void GetHitByBullet(Projectile projectile)
    {
        Debug.Log("projectile " + projectile.GetType().Name + " hit player. Game over.");
        //GetComponent<CharacterController2D>().Die()
    }

}
