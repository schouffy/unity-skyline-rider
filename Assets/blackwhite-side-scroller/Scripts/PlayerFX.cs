using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    public Transform LeftFoot;
    public Transform RightFoot;
    public Transform Feet;

    public GameObject FootTouchGroundWhileRunningParticleEffect;
    public GameObject JumpStartParticleEffect;
    public GameObject LandAfterJumpParticleEffect;

    void LeftFootHitGround()
    {
        var particle = Instantiate(FootTouchGroundWhileRunningParticleEffect, LeftFoot.position, Quaternion.identity);
        Destroy(particle, 1);
    }
    void RightFootHitGround()
    {
        var particle = Instantiate(FootTouchGroundWhileRunningParticleEffect, RightFoot.position, Quaternion.identity);
        Destroy(particle, 1);
    }
    void JumpStart()
    {
        var particle = Instantiate(JumpStartParticleEffect, Feet.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Destroy(particle, 1);
    }
    void LandAfterJump()
    {
        var particle = Instantiate(LandAfterJumpParticleEffect, Feet.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Destroy(particle, 2);
    }
}
