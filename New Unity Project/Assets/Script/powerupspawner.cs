using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupspawner : MonoBehaviour {
    [SerializeField]
    GameObject Powerup;

    float spawntime;

    // Use this for initialization
    void Start()
    {
        spawntime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spawntime += Time.deltaTime;
        Vector2 randomPosition = new Vector2(Random.Range(-5, 5), 0);
        if (spawntime >= 10)
        {
            Instantiate(Powerup, randomPosition, Quaternion.identity);
            spawntime = 0;
        }
    }
}
