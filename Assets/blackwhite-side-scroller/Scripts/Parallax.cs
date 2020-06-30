using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    private float length;
    private Vector2 startPos;
    public float ParallaxEffect;
    //public float ParallaxEffectVertical;

    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        // Duplicate the sprite before and after
        DuplicateBackground(new Vector2(startPos.x - length, transform.position.y));
        DuplicateBackground(new Vector2(startPos.x + length, transform.position.y));
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
        float distX = (Constants.MainCamera.transform.position.x * ParallaxEffect);
        //float distY = (Camera.transform.position.y * ParallaxEffectVertical);
        //(Camera.transform.position.y - cameraOffset.y) * (1 - ParallaxEffectVertical)
        transform.position = new Vector3(startPos.x + distX, transform.position.y, transform.position.z);

        float temp = (Constants.MainCamera.transform.position.x * (1 - ParallaxEffect));
        if (temp > startPos.x + length) startPos.x += length;
        else if (temp < startPos.x - length) startPos.x -= length;
    }
}