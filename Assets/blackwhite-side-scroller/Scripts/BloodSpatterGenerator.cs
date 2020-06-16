using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodSpatterGenerator : MonoBehaviour
{
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    private static Dictionary<GameObject, List<Vector3>> _listOfBloodSpattersCreatedByCollider = new Dictionary<GameObject, List<Vector3>>(); // static because shared by all the particle systems
    public GameObject BloodSpatter;
    private float _spatterRadius;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        _spatterRadius = 0.15f;
    }

    void OnParticleCollision(GameObject colliderParticleFellOn)
    {
        int numCollisionEvents = part.GetCollisionEvents(colliderParticleFellOn, collisionEvents);

        int i = 0;
        while (i < numCollisionEvents)
        {
            if (!_listOfBloodSpattersCreatedByCollider.ContainsKey(colliderParticleFellOn))
                _listOfBloodSpattersCreatedByCollider.Add(colliderParticleFellOn, new List<Vector3>());

            var spriteMask = colliderParticleFellOn.GetComponent<SpriteMask>();
            if (spriteMask == null)
            {
                colliderParticleFellOn.AddComponent(typeof(SpriteMask));
                spriteMask = colliderParticleFellOn.GetComponent<SpriteMask>();
                spriteMask.sprite = colliderParticleFellOn.GetComponent<SpriteRenderer>().sprite;
            }

            // Do not create if another blood spatter is already is in range on this collider according to sprite size
            if (!_listOfBloodSpattersCreatedByCollider[colliderParticleFellOn].Any(spatter => Vector2.Distance(spatter, collisionEvents[i].intersection) < _spatterRadius))
            {
                _listOfBloodSpattersCreatedByCollider[colliderParticleFellOn].Add(collisionEvents[i].intersection);
                Instantiate(BloodSpatter, collisionEvents[i].intersection, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            }
            i++;
        }
    }
}
