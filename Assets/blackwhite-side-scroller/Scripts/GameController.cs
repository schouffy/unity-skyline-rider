using UnityEngine;

public class GameController : MonoBehaviour
{
    private static int _deathCount = 0;
    private static Vector3? RespawnLocation = null;

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
