using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteepGround : MonoBehaviour
{
    public Transform SlideDestination;

    public void DisableSliding()
    {
        this.GetComponent<Collider2D>().sharedMaterial = null;
    }
}
