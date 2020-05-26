using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpriteSize : MonoBehaviour
{
    private float _lastChangeSizeTime;
    public float Rate;
    public float RandomRate;
    public Vector2 Scale;
    private Vector2 _initialScale;
    

    // Start is called before the first frame update
    void Start()
    {
        _initialScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _lastChangeSizeTime + Rate + UnityEngine.Random.Range(0, RandomRate))
        {
            _lastChangeSizeTime = Time.time;
            if (this.transform.localScale.sqrMagnitude == Scale.sqrMagnitude)
                this.transform.localScale = _initialScale;
            else
                this.transform.localScale = Scale;
        }
    }
}
