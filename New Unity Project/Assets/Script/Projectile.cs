using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifeTime;
    public float projectileSpeed;
    Vector3 dir;
    Vector3 target;

    // Use this for initialization
    void Start()
    {
        lifeTime = 5;
        projectileSpeed = -5;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        transform.Translate(dir.x * projectileSpeed * Time.deltaTime, dir.y * projectileSpeed * Time.deltaTime, 0);
        if (lifeTime <= 0)
            Destroy(gameObject);
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
            if (other.GetComponent<PlayerController>().invincible == false)
                other.GetComponent<PlayerController>().health -= 10;
            Destroy(this.gameObject);
        }
    }

    public void SetDir(Vector3 _Dir)
    {
        dir = _Dir;
    }
}
