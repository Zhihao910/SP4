﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifeTime;
    public float projectileSpeed ;
    Vector3 dir;
    Vector3 target;

	// Use this for initialization
	void Start () {
        lifeTime = 5;
        projectileSpeed = -5;
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        transform.Translate(projectileSpeed * Time.deltaTime, 0, 0);
        if (lifeTime <= 0)
            Destroy(gameObject);
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player health -10");
            other.GetComponent<PlayerController>().health -= 10;
            Destroy(this.gameObject);
        }
    }
}
