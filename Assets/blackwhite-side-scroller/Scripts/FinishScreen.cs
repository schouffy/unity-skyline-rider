using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishScreen : MonoBehaviour
{
    public Text StatsText;

    public void Display()
    {
        String displayCompletionTime = "170 seconds";
        StatsText.text = $"Time: {displayCompletionTime}\nDeath count: {5}";
        gameObject.SetActive(true);
    }
}
