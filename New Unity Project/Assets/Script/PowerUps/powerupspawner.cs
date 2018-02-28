﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupspawner : MonoBehaviour {
    [SerializeField]
    GameObject Powerup;
    [SerializeField]
    GameObject powerup1;
    [SerializeField]
    GameObject blankPowerup;

    float spawntime;
    float spawntime2;
    float spawntime3;
    // Use this for initialization
    void Start()
    {
        spawntime = 0;
        spawntime2 = 0;
        spawntime3 = 0;
    }
    // Update is called once per frame
    void Update()
    {
        spawntime += Time.deltaTime;
        spawntime2 += Time.deltaTime;
        spawntime3 += Time.deltaTime;
        Vector2 randomPosition = new Vector2(Random.Range(-5, 5), 0);
        if (spawntime >= 10)
        {
            Instantiate(Powerup, randomPosition, Quaternion.identity);
            spawntime = 0;
        }
        if(spawntime2>=15)
        {
            Instantiate(powerup1, randomPosition, Quaternion.identity);
            spawntime2 = 0;
        }
        if (spawntime3 >= 5)
        {
            Instantiate(blankPowerup, new Vector3(5, -3, 0), Quaternion.identity);
            spawntime3 = 0;
        }
    }
}