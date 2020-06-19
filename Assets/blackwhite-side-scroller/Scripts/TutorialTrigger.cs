using Assets.blackwhite_side_scroller.Scripts;
using System;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : GameTrigger
{
    [TextArea(3, 10)]
    public String TextToDisplay;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<String> { }
    public UnityStringEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    private static Transform _player;

    protected override void Start()
    {
        base.Start();

        if (OnPlayerEnter == null)
            OnPlayerEnter = new UnityStringEvent();
        if (OnPlayerExit == null)
            OnPlayerExit = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            OnPlayerEnter.Invoke(TextToDisplay);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TagPlayer)
        {
            OnPlayerExit.Invoke();
        }
    }
}
