using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class GameTrigger : MonoBehaviour
{
    protected Collider2D _trigger;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _trigger = GetComponent<Collider2D>();
        if (!_trigger.isTrigger)
            Debug.LogError($"{name} collider should be a trigger");
    }
}
