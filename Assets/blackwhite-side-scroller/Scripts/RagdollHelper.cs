using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHelper : MonoBehaviour
{
    public static void CreateCollidersForRagdoll(Vector3 position)
    {
        var nearbyColliders2d = Physics2D.OverlapCircleAll(position, 5f, LayerMask.GetMask("Default"));
        foreach (BoxCollider2D nearbyCollider2d in nearbyColliders2d)
        {
            var collider3d = new GameObject(nearbyCollider2d.name + "-3d-collider", typeof(BoxCollider));
            collider3d.transform.parent = nearbyCollider2d.transform;
            collider3d.GetComponent<BoxCollider>().center = new Vector3(nearbyCollider2d.offset.x, nearbyCollider2d.offset.y, 0);
            collider3d.GetComponent<BoxCollider>().size = new Vector3(nearbyCollider2d.size.x, nearbyCollider2d.size.y, 10);
            collider3d.transform.localPosition = Vector3.zero;
            collider3d.transform.localScale = Vector3.one;
        }
    }
}
