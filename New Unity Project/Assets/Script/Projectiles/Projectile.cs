﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifeTime = 5;
    float projectileSpeed = 5;
    Vector3 dir;
    Vector3 target = new Vector3(999, 999, 999);
    protected bool hittarget = false;
    GameObject Indicator;
    //int _waveHeight = 0;
    //float _sinAngle = 1.0f;

    //[SerializeField]
    //private GameObject _feedback;

    // Use this for initialization
    protected void Start()
    {
        //lifeTime = 5;
        //projectileSpeed = 5;
        //_waveHeight = 0;
        //_feedback = GameObject.FindGameObjectWithTag("Feedback");
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!hittarget)
        {
            lifeTime -= Time.deltaTime;
            transform.Translate(dir.x * projectileSpeed * Time.deltaTime, dir.y * projectileSpeed * Time.deltaTime, 0);

            //if (_waveHeight != 0)
            //{
            //    transform.localScale += new Vector3(0, _waveHeight, 0);
            //}

            //if (_waveHeight == 0)
            //{
            //    transform.Translate(dir.x * projectileSpeed * Time.deltaTime, dir.y * projectileSpeed * Time.deltaTime, 0);
            //}
            //else
            //{
            //    //_sinAngle += (0.025f * _waveHeight);
            //    //transform.Translate(dir.x * projectileSpeed * Time.deltaTime, Mathf.Sin(_sinAngle) / 90, 0);
            //    transform.Translate(dir.x * projectileSpeed * Time.deltaTime, dir.y * projectileSpeed * Time.deltaTime, 0);

            //    if (_sinAngle < 1.5)
            //    {
            //        print("spawn");
            //        transform.localScale += new Vector3(0,20,0);
            //        _sinAngle += 1.0f;
            //    }
            //}

            if (lifeTime <= 0)
            {
                if (null != transform.parent)
                    Destroy((transform.parent).gameObject);
                else
                    Destroy(gameObject);
            }

            if (null != target)
            {
                if ((target - transform.position).magnitude < 0.1f)
                    hittarget = true;
            }
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        yield return new WaitForSeconds(0.05f);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player health -10");
            //Debug.Log("ADD MANA");

            //GetComponent<particles>().ApplyParticle(gameObject, "hitParticle", 0.5f, 1, false);
            
            if (other.GetComponentInParent<PlayerController>().invincible == false && other.GetComponentInParent<PlayerController>().invincible2 == false)
                other.GetComponentInParent<PlayerController>().takeDamage(1);

            if (null != transform.parent)
                Destroy((transform.parent).gameObject);
            else
                Destroy(gameObject);

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void SetDir(Vector3 _Dir)
    {
        dir = _Dir;
    }

    public void SetTarget(Vector3 _Target)
    {
        target = _Target;
        Destroy(Indicator);
        Indicator = Instantiate(Resources.Load("Prefabs/Indicator") as GameObject);
        Indicator.transform.parent = gameObject.transform.parent;
        Indicator.transform.position = _Target;
        Indicator.SetActive(true);
    }

    public void SetSpeed(float _projSpeed)
    {
        projectileSpeed = _projSpeed;
    }

    //public void SetHeight(int _height)
    //{
    //    _waveHeight = _height;
    //}
}
