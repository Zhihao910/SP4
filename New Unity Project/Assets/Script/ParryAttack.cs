using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttack : MonoBehaviour
{
    AudioSource audio;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("parableProjectile"))
        {
            Destroy(other.gameObject);
            audio.Play();
        }
    }
}
