using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteepGround : MonoBehaviour
{
    public Collider2D EndTrigger;
    public Collider2D DontGoBackCollider;

    public void Start()
    {
        if (DontGoBackCollider == null)
            Debug.LogError(gameObject.name + " - DontGoBackCollider should be set");
        if (EndTrigger == null)
            Debug.LogError(gameObject.name + " - EndTrigger should be set");
    }

    public void ReachedEnd()
    {
        this.GetComponent<Collider2D>().sharedMaterial = null;
        DontGoBackCollider.gameObject.SetActive(true);
        EndTrigger.gameObject.SetActive(false);
    }
}
