using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifeTime;
    public float projectileSpeed;
    Vector3 dir;
    Vector3 target = new Vector3(999, 999, 999);
    protected bool hittarget = false;
    GameObject Indicator;

    // Use this for initialization
    protected void Start()
    {
        lifeTime = 5;
        projectileSpeed = 5;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!hittarget)
        {
            lifeTime -= Time.deltaTime;
            transform.Translate(dir.x * projectileSpeed * Time.deltaTime, dir.y * projectileSpeed * Time.deltaTime, 0);
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player health -10");
            Debug.Log("ADD MANA");
            other.GetComponent<PlayerController>().mana += 20;
            if (other.GetComponent<PlayerController>().mana >= 100)
                other.GetComponent<PlayerController>().mana = 100;
            if (other.GetComponent<PlayerController>().invincible == false&&other.GetComponent<PlayerController>().invincible2==false)
                other.GetComponent<PlayerController>().health -= 10;
            if (null != transform.parent)
                Destroy((transform.parent).gameObject);
            else
                Destroy(gameObject);
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
}
