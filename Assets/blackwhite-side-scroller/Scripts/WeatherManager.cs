using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public Camera Camera;
    public GameObject RaindropHitGroundPrefab;
    public float RaindropsSpawnRate;
    public LayerMask RaindropsHitThese;

    public AudioClip Thunder;

    private void Start()
    {
        StartCoroutine(TriggerLightnings());
        StartCoroutine(DisplayRaindrops());
    }

    private void Update()
    {
        transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, transform.position.z);
    }

    IEnumerator TriggerLightnings()
    {
        yield return new WaitForSeconds(Random.Range(10, 20));
        GetComponent<AudioSource>().PlayOneShot(Thunder);
        GetComponent<Animator>().SetTrigger("Lightning");

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(25, 35));
            GetComponent<Animator>().SetTrigger("Lightning");
            GetComponent<AudioSource>().PlayOneShot(Thunder);
        }
    }

    IEnumerator DisplayRaindrops()
    {
        // raycasts, instantiate prefabs
        while (true)
        {
            var skyPosition = new Vector2(
                transform.position.x + Random.Range(-20, 20),
                transform.position.y + 15);

            var hitInfo = Physics2D.Raycast(skyPosition, Vector2.down, 20, RaindropsHitThese.value);
            if (hitInfo.collider != null)
            {
                Instantiate(RaindropHitGroundPrefab, hitInfo.point, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f / RaindropsSpawnRate);
        }
    }
}
