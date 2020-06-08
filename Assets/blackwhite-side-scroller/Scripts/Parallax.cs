using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject Camera;
    public float ParallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Duplicate the sprite before and after
        DuplicateBackground(new Vector2(startpos - length, transform.position.y));
        DuplicateBackground(new Vector2(startpos + length, transform.position.y));
    }

    void DuplicateBackground(Vector2 position)
    {
        var thisSprite = this.GetComponent<SpriteRenderer>();

        var previousBackground = new GameObject(name, typeof(SpriteRenderer));
        var sprite = previousBackground.GetComponent<SpriteRenderer>();
        sprite.sprite = thisSprite.sprite;
        sprite.sortingLayerID = thisSprite.sortingLayerID;
        sprite.sortingOrder = thisSprite.sortingOrder;
        previousBackground.transform.position = position;
        previousBackground.transform.SetParent(transform);
    }

    void Update()
    {
        float temp = (Camera.transform.position.x * (1 - ParallaxEffect));
        float dist = (Camera.transform.position.x * ParallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}