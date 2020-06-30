using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static DateTime _timeSinceLevelLoad = DateTime.UtcNow;
    private static int _deathCount = 0;
    private static Vector3? RespawnLocation = null;

    public double GetElapsedTime()
    {
        return (DateTime.UtcNow - _timeSinceLevelLoad).TotalMilliseconds / 1000f;
    }

    public void ResetElapsedTime()
    {
        _timeSinceLevelLoad = DateTime.UtcNow;
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

    public void ActivateCheckpoint(CheckpointTrigger checkpoint)
    {
        RespawnLocation = checkpoint.RespawnLocation.transform.position;
    }

    public void ClearCheckpoint()
    {
        RespawnLocation = null;
    }

    public Vector3? GetRespawnLocation()
    {
        return RespawnLocation;
    }
}
