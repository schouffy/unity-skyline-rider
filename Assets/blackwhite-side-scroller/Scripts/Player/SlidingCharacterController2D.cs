using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EndSlidingCondition
{
    Fall,
    Jump
}

public class SlidingCharacterController2D : MonoBehaviour
{
    public float SlideSpeed = 1f;
    public Transform ParticlesSpawnPoint;
    public float ParticlesSpawnInterval;
    private float _lastParticleSpawnTime;
    public GameObject SlidingParticles;
    private AudioSource _audio;
    public AudioClip Sliding;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, ((Vector2)transform.position + new Vector2(10, -10)), SlideSpeed * Time.deltaTime);

        if (Time.time > _lastParticleSpawnTime + ParticlesSpawnInterval)
        {
            var particle = Instantiate(SlidingParticles, ParticlesSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        }

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void SlideReachedEnd()
    {
        GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Fall);
    }

    void OnDisable()
    {
        Constants.MainCamera.GetComponent<SmoothFollow2D>().SetCameraConfiguration(CameraConfiguration.Default);
        _audio.loop = false;
        _audio.Stop();
    }

    void OnEnable()
    {
        Constants.MainCamera.GetComponent<SmoothFollow2D>().SetCameraConfiguration(CameraConfiguration.SlideSteep);
        _audio.loop = true;
        _audio.clip = Sliding;
        _audio.Play();
    }

    public void Jump()
    {
        GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Jump);
    }
}
