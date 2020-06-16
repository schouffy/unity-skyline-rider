using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    public Rigidbody RootBone;

    public void CopyPose(Transform rigRoot)
    {
        Debug.Log("copy all transforms according to bone name");
    }

    public void AddForce(Vector3 force)
    {
        RootBone.AddForce(force, ForceMode.Impulse);
    }
}
