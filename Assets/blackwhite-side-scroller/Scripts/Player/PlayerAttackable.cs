using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackable : MonoBehaviour
{
    public Transform[] PointsToRaycast;
    public GameObject HitFX;

    public void GetHitByBullet(Projectile projectile, Vector3 impactPosition)
    {
        //Debug.Log("projectile " + projectile.GetType().Name + " hit player. Game over.");

        var hitFx = Instantiate(HitFX, impactPosition, Quaternion.identity);
        hitFx.transform.right = projectile.transform.right; // rotate it correctly

        //GetComponent<CharacterController2D>().Die()
    }

}
