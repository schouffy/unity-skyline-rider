using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioHitProjectile : Projectile
{
    public LayerMask HitLayers;

    void Start()
    {
        Debug.DrawLine(transform.position, transform.position + (transform.right * 50f), Color.red, 1f);
        var hitInfo = Physics2D.Raycast(transform.position, transform.right, 50f, HitLayers);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.tag == "Player")
            {
                hitInfo.collider.gameObject.GetComponentInParent<CharacterController2D>().GetHitByBullet(this);
            }
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                Debug.Log("TODO projectile hit enemy. Make it die");
            }
            else
            {
                Debug.Log("TODO projectile hit environement. Spawn FX");
            }
        }
        //Destroy(this.gameObject);
    }
}
