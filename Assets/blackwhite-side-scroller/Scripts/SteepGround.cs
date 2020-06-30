using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteepGround : MonoBehaviour
{
    public Transform SlideDestination;
    public Collider2D DontGoBackCollider;

    public void Start()
    {
        if (DontGoBackCollider == null)
            Debug.LogError(gameObject.name + " - DontGoBackCollider should be set");
    }

    public void DisableSliding()
    {
        this.GetComponent<Collider2D>().sharedMaterial = null;
    }
}
