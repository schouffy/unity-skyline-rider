using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndSlidingCondition
{
    Fall,
    Jump
}

public class SlidingCharacterController2D : MonoBehaviour
{
    private Vector2 _destination;
    public float SlideSpeed = 1f;
    public Transform ParticlesSpawnPoint;
    public float ParticlesSpawnInterval;
    private float _lastParticleSpawnTime;
    public GameObject SlidingParticles;
    
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _destination, SlideSpeed * Time.deltaTime);

        if (Time.time > _lastParticleSpawnTime + ParticlesSpawnInterval)
        {
            var particle = Instantiate(SlidingParticles, ParticlesSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            GameObject.Destroy(particle, 1f);
        }

        if (Vector2.Distance(transform.position, _destination) < 0.1f)
        {
            GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Fall);
        }
    }

    public void SetSlideDestination(Vector2 position)
    {
        _destination = position;
    }

    public void Jump()
    {
        GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Jump);
    }
}
