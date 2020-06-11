using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    public Transform LeftFoot;
    public Transform RightFoot;

    public GameObject FootTouchGroundWhileRunningParticleEffect;
    public GameObject JumpStartParticleEffect;
    public GameObject LandAfterJumpParticleEffect;

    void LeftFootHitGround()
    {
        var particle = Instantiate(FootTouchGroundWhileRunningParticleEffect, LeftFoot.position, Quaternion.identity);
        Destroy(particle.gameObject, 1000f);
    }
    void RightFootHitGround()
    {
        var particle = Instantiate(FootTouchGroundWhileRunningParticleEffect, LeftFoot.position, Quaternion.identity);
        Destroy(particle.gameObject, 1000f);
    }
    void JumpStart()
    {
        var particle = Instantiate(JumpStartParticleEffect, LeftFoot.position, Quaternion.identity);
        Destroy(particle.gameObject, 1000f);
    }
    void LandAfterJump()
    {
        var particle = Instantiate(LandAfterJumpParticleEffect, LeftFoot.position, Quaternion.identity);
        Destroy(particle.gameObject, 1000f);
    }
}
