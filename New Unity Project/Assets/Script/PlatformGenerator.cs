using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject platform;
    int numOfPlatforms = 5;
    bool stopSpawn = true;
    float spawnTimer = 3;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0 && stopSpawn)
        {
            stopSpawn = false;
            GeneratePlatform();
        }
    }

    void GeneratePlatform()
    {
        for (int i = 0; i < numOfPlatforms; ++i)
        {
            Vector2 position = new Vector2(Random.Range(-8, 8), -5);
            Instantiate(platform, position, Quaternion.identity);
            platform.SetActive(true);
        }
    }
}
