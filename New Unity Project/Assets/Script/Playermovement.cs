﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [SerializeField]
    float totalHealth = 100;
    float health = 100;
    float totalMana = 100;
    float mana = 100;
    [SerializeField]
    GameObject healthBar, manaBar;

    [SerializeField]
    float movementspeed = 10;
    float test = 0;
    bool jump2 = false;
    bool jumping = false;
    int dashcountdown;
    float regaindash = 0;
    // Use this for initialization
    void Start()
    {
        jump2 = false;
        jumping = false;
        dashcountdown = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashcountdown == 0)
        {
            regaindash += Time.deltaTime;
        }
        if (regaindash >= 5)
        {
            dashcountdown = 5;
            regaindash = 0;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * movementspeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && dashcountdown > 0)
        {
            transform.position += transform.right * 20 * Time.deltaTime;
            dashcountdown--;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift) && dashcountdown > 0)
        {
            transform.position -= transform.right * 20 * Time.deltaTime;
            dashcountdown--;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * movementspeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space) && transform.position.y <= 0.5f)
        {
            jumping = true;
            jump2 = false;
        }
        if (jumping == true)
        {
            transform.position += transform.up * 10 * Time.deltaTime;
        }
        if (transform.position.y >= 3)
        {
            jumping = false;
            jump2 = true;
        }
        if (jump2 == true)
        {
            if (transform.position.y >= 0.5)
                transform.position -= transform.up * 10 * Time.deltaTime;
            if (transform.position.y < 0.5)
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        healthBar.transform.localScale = new Vector3(health / totalHealth, 1, 1);
        manaBar.transform.localScale = new Vector3(mana / totalMana, 1, 1);

        health -= 1.0f;
        if (health <= 0.0f)
            health = 0.0f;

        if (Input.GetKeyDown(KeyCode.X))
        {
            mana -= 10.0f;
            if (mana <= 0.0f)
                mana = 0.0f;
        }
        mana += Time.deltaTime * 3.0f;

    }
}
