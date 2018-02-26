﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttack : MonoBehaviour
{
    private GameObject _feedback;

    // Use this for initialization
    void Start()
    {
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
            print("Parried!");
            _feedback.GetComponent<Feedback>().CreateImage("ParryPass", other.gameObject.transform.position);
            _feedback.GetComponent<Feedback>().CreateAudio("Pass");

            //this.GetComponent<PlayerController>().mana += 10;
            gameObject.GetComponentInParent<PlayerController>().mana += 10;


            //other.gameObject.SetActive(false);
            if (other.transform.parent)
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}
