using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioHitProjectile : Projectile
{
    public LayerMask HitLayers;
    public GameObject WeaponShootPrefab;
    public GameObject BulletHitEnvironmentPrefab;

    void Start()
    {
        Debug.DrawLine(transform.position, transform.position + (transform.right * 50f), Color.red, 1f);

        var fx = Instantiate(WeaponShootPrefab, transform.position, transform.rotation);
        GameObject.Destroy(fx, 2f);

        var hitInfo = Physics2D.Raycast(transform.position, transform.right, 50f, HitLayers);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.tag == "Player")
            {
                hitInfo.collider.gameObject.GetComponentInParent<PlayerAttackable>().GetHitByBullet(this);
            }
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                Debug.Log("TODO projectile hit enemy. Make it die");
            }
            else
            {
                var envFx = Instantiate(BulletHitEnvironmentPrefab, hitInfo.point, Quaternion.identity);
                envFx.transform.right = hitInfo.normal;
                GameObject.Destroy(envFx, 2f);
            }
        }
        Destroy(this.gameObject, 1f);
    }
}
