﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttack : MonoBehaviour
{
    AudioSource audio;

    [SerializeField]
    GameObject _feedback;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        _feedback = GameObject.FindGameObjectWithTag("Feedback");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("parableProjectile"))
        {
            _feedback.GetComponent<Feedback>().Create("ParryPass", other.gameObject.transform.localPosition);
            Destroy(other.gameObject);
            audio.Play();

        }
    }
}
