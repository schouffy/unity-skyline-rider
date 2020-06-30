using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float Delay;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, Delay);
    }

}
