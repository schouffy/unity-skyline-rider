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
    private Vector2 _destination;
    public float SlideSpeed = 1f;
    public Transform ParticlesSpawnPoint;
    public float ParticlesSpawnInterval;
    private float _lastParticleSpawnTime;
    public GameObject SlidingParticles;
    private SteepGround _groundCurrentlySlidingOn;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, ((Vector2)transform.position + new Vector2(10, -10)), SlideSpeed * Time.deltaTime);

        if (Time.time > _lastParticleSpawnTime + ParticlesSpawnInterval)
        {
            var particle = Instantiate(SlidingParticles, ParticlesSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        }

        if (Vector2.Distance(transform.position, _destination) < 1f)
        {
            GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Fall);
            _groundCurrentlySlidingOn.DontGoBackCollider.gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        Constants.MainCamera.GetComponent<SmoothFollow2D>().SetCameraConfiguration(CameraConfiguration.Default);
    }

    void OnEnable()
    {
        Constants.MainCamera.GetComponent<SmoothFollow2D>().SetCameraConfiguration(CameraConfiguration.SlideSteep);
    }

    public void SlideOn(SteepGround steepGround)
    {
        _groundCurrentlySlidingOn = steepGround;
        _destination = steepGround.SlideDestination.position;

    }

    public void Jump()
    {
        GetComponent<CharacterController2D>().EndSliding(EndSlidingCondition.Jump);
    }
}
