using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupspawner : MonoBehaviour {
    [SerializeField]
    GameObject Powerup;
    [SerializeField]
    GameObject powerup1;

    float spawntime;
    float spawntime2;
    // Use this for initialization
    void Start()
    {
        spawntime = 0;
        spawntime2 = 0;
    }
    // Update is called once per frame
    void Update()
    {
        spawntime += Time.deltaTime;
        spawntime2 += Time.deltaTime;
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
    }
}
