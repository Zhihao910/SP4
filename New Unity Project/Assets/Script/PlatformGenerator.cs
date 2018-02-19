using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] platform;
    List<GameObject> movingPlatforms = new List<GameObject>();
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
        UpdatePlatform();
    }

    void GeneratePlatform()
    {
        for (int i = 0; i < numOfPlatforms; ++i)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-8, 8), -10);
           
            movingPlatforms.Add(Instantiate(platform[Random.Range(0,2)], randomPosition, Quaternion.identity)); //Create platform at different position
            platform[0].SetActive(true); //Show the platform
        }
    }

    void UpdatePlatform()
    {
        foreach (GameObject go in movingPlatforms)
        {
            go.transform.Translate(0, 1 * Time.deltaTime, 0);
            if (go.transform.position.y >= -6)
            {
                go.transform.position = new Vector3(go.transform.position.x, -6, 0);
            }
        }
    }
}
