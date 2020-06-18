using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static int _deathCount = 0;

    public float GetElapsedTime()
    {
        return Time.timeSinceLevelLoad;
    }

    public void IncrementDeathCount()
    {
        _deathCount++;
    }
    public void ResetDeathCount()
    {
        _deathCount = 0;
    }

    public int GetDeathCount()
    {
        return _deathCount;
    }
}
