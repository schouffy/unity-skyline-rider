using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class BottomlessPit : MonoBehaviour
{
    private Collider2D _trigger;
    public UnityEvent OnPlayerEnter;

    // Start is called before the first frame update
    void Start()
    {
        _trigger = GetComponent<Collider2D>();
        if (!_trigger.isTrigger)
            Debug.LogError($"{name} collider should be a trigger");

        if (OnPlayerEnter == null)
            OnPlayerEnter = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // stop camera follow
            Camera.main.GetComponent<SmoothFollow2D>().enabled = false;

            // TODO play fall and crash sounds synchronized with fade to black

            OnPlayerEnter.Invoke();
        }
    }

}
