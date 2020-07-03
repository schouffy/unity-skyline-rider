using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioHitProjectile : Projectile
{
    public LayerMask HitLayers;
    public GameObject WeaponShootPrefab;
    public GameObject BulletHitEnvironmentPrefab;
    public float BulletTraceFadeOutSpeed = 1f;
    public AnimationCurve BulletTraceFadeOutCurve;


    void Start()
    {
        var fx = Instantiate(WeaponShootPrefab, transform.position, transform.rotation);
        GameObject.Destroy(fx, 2f);

        var hitInfo = Physics2D.Raycast(transform.position, transform.right, 50f, HitLayers);
        if (hitInfo.collider != null)
        {
            StartCoroutine(RenderBulletPath(hitInfo.point));

            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(Constants.LayerPlayer))
            {
                hitInfo.collider.gameObject.GetComponentInParent<PlayerAttackable>().GetHitByBullet(this, hitInfo.point);
            }
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(Constants.LayerPlayer))
            {
                //Debug.Log("TODO projectile hit enemy. Make it die");
            }
            else
            {
                var envFx = Instantiate(BulletHitEnvironmentPrefab, hitInfo.point, Quaternion.identity);
                envFx.transform.right = hitInfo.normal;
                GameObject.Destroy(envFx, 2f);
            }
        }
        Destroy(this.gameObject, BulletTraceFadeOutSpeed);
    }

    IEnumerator RenderBulletPath(Vector2 destination)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] {
            transform.position,
            destination
        });
        var beginTime = Time.time;

        while (true)
        {
            float timeElapsed = Time.time - beginTime;
            float alpha = BulletTraceFadeOutCurve.Evaluate(timeElapsed);

            if (alpha < 0)
                alpha = 0;
            var currentColor = lineRenderer.startColor;
            var targetColor = currentColor;
            targetColor.a = alpha;
            lineRenderer.startColor = targetColor;
            lineRenderer.endColor = targetColor;
            //lineRenderer.materials[0].SetColor("_Color", targetColor);
            yield return null;
        }
    }
}
