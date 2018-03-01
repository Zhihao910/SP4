using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject Projectile1;
    [SerializeField]
    GameObject Projectile2;
    float lifeTime;
    float spawnTime;
    float spawnTime2;

    bool Activate = false;

    // Use this for initialization
    void Start()
    {
        lifeTime = 3;
        spawnTime = 0;
        spawnTime2 = 0;
        Projectile1 = Resources.Load("Prefabs/Projectile5") as GameObject;
        Projectile2 = Resources.Load("Prefabs/Projectile2") as GameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Activate)
            return;
        spawnTime += Time.deltaTime;
        spawnTime2 += Time.deltaTime;
        Vector2 randomPosition = new Vector2(15, Random.Range(-1, -5));

        if (spawnTime >= 1)
        {
            Instantiate(Projectile1, randomPosition, Quaternion.identity);
            spawnTime = 0;
        }
        if (spawnTime2 >= 3)
        {
            Instantiate(Projectile2, randomPosition, Quaternion.identity);
            spawnTime2 = 0;
        }
    }

    public void ActivateSpawn()
    {
        Activate = !Activate;
        spawnTime = 0;
        spawnTime2 = 0;
    }
}

