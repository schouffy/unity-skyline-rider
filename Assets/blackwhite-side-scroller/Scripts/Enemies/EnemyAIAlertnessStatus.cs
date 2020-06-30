using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIAlertnessStatus : MonoBehaviour
{
    public SpriteMask SpriteMask;
    public EnemyAI EnemyAI;
    public GameObject StatusIcon;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyAI.SuspicionLevel == 0)
        {
            StatusIcon.SetActive(false);
            return;
        }

        StatusIcon.SetActive(true);
        SpriteMask.transform.localScale = new Vector3(
            SpriteMask.transform.localScale.x,
            (float)EnemyAI.SuspicionLevel / 100f,
            SpriteMask.transform.localScale.z);
    }
}
