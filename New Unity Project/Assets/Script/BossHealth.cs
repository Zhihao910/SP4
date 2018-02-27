﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    public float totalhealth = 100;
    [SerializeField]
    public float health = 100;

    [SerializeField]
    GameObject Healthbar;

    [SerializeField]
    Score playerScore;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //scaling of the healthbar
        Healthbar.transform.localScale = new Vector3(health / totalhealth, 1, 1);

        if (health <= 0)
        {
            playerScore.SaveScore();
            Destroy(gameObject);
        }
    }
}
