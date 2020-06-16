using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackable : MonoBehaviour
{
    public Transform[] PointsToRaycast;
    public GameObject HitFX;
    public PlayerRagdoll PlayerRagdoll;
    public Transform PlayerRig;

    public void GetHitByBullet(Projectile projectile, Vector3 impactPosition)
    {
        //Debug.Log("projectile " + projectile.GetType().Name + " hit player. Game over.");

        var hitFx = Instantiate(HitFX, impactPosition, Quaternion.identity);
        hitFx.transform.right = projectile.transform.right; // rotate it correctly

        Die(projectile.transform.right * projectile.KickbackStrength);
    }

    public void Die(Vector2 forceToAddToRagdoll)
    {
        RagdollHelper.CreateCollidersForRagdoll(transform.position);
        var ragdoll = Instantiate(PlayerRagdoll, transform.position, transform.rotation);
        ragdoll.CopyPose(PlayerRig);
        ragdoll.AddForce(forceToAddToRagdoll);
        GameObject.Destroy(this.gameObject);
    }

}
