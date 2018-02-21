using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public float speed;
    Vector3 bgPos;

    // Use this for initialization
    void Start()
    {
        bgPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        parallaxBackground();
    }

    void parallaxBackground()
    {
        transform.Translate((new Vector3(-1, 0, 0)) * speed * Time.deltaTime);
        if (transform.position.x < -41.7)
            transform.position = bgPos;
    }
}
