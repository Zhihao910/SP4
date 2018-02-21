using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] platform;
    List<GameObject> movingPlatforms = new List<GameObject>();
    Vector3 pos;
    int numOfPlatforms = 5;
    bool stopSpawn = true;
    float spawnTimer = 3;
    bool ground = true;


    public GameObject[] allSprites;

    // Use this for initialization
    void Start()
    {
        allSprites = GameObject.FindGameObjectsWithTag("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0 && stopSpawn)
        {
            stopSpawn = false;
            //GeneratePlatform();
        }
        UpdatePlatform();
    }

    void GeneratePlatform()
    {
        for (int i = 0; i < numOfPlatforms; ++i)
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Vector3 randomPosition = new Vector2(Random.Range(-8, 8), -10);
            movingPlatforms.Add(Instantiate(platform[Random.Range(0, 2)], randomPosition, Quaternion.identity)); //Create platform at different position
            platform[0].SetActive(true); //Show the platform
        }
    }

    void UpdatePlatform()
    {
        if (!ground)
        {
            foreach (GameObject go in allSprites)
            {
                go.transform.Translate(0, -1 * Time.deltaTime, 0);
                if (go.transform.position.y < -10)
                {
                    go.transform.position = new Vector3(go.transform.position.x, -10, 0);
                }
            }
        }
        else
        {
            foreach (GameObject go in allSprites)
            {
                go.transform.Translate(0, 1 * Time.deltaTime, 0);
                if (go.transform.position.y > -4.5f)
                {
                    go.transform.position = new Vector3(go.transform.position.x, -4.5f, 0);
                }
            }
        }

        for (int i = movingPlatforms.Count - 1; i >= 0; --i)
        {
            GameObject go = movingPlatforms[i];
            if (go.GetComponent<Platform>().despawn)
            {
                movingPlatforms.RemoveAt(i);
                Destroy(go);
            }
        }
    }

    public void GeneratePlatform(Vector3 _Position, Vector3 _Target)
    {
        _Target.x -= platform[1].GetComponentInChildren<BoxCollider2D>().size.x;
        _Position.x -= platform[1].GetComponentInChildren<BoxCollider2D>().size.x;
        GameObject newplat = Instantiate(platform[1], _Position, Quaternion.identity); //Create platform at different position
        movingPlatforms.Add(newplat);
        newplat.GetComponent<Platform>().SetTarget(_Target);
        newplat.GetComponent<Platform>().SetDir((_Target - _Position).normalized);

        newplat.SetActive(true); //Show the platform
    }

    public void ToggleGround()
    {
        ground = !ground;
    }

    public void DespawnAll()
    {
        foreach(GameObject go in movingPlatforms)
        {
            go.GetComponent<Platform>().despawn = true;
        }
    }
}

