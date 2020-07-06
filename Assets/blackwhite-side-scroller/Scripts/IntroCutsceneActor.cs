using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutsceneActor : MonoBehaviour
{
    public void PauseAnimation()
    {
        GetComponent<Animator>().SetFloat("AnimSpeed", 0);
    }

    public void RotateAfterTurnAroundAnimation()
    {
        transform.rotation = Quaternion.Euler(0, 270, 0);
    }
}
