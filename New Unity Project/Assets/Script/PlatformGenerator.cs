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
    bool platforms = false;


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
        if(Input.GetKeyDown(KeyCode.P))
        {
            ToggleGround();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            TogglePlatforms();
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
        platforms = true;
    }

    void UpdatePlatform()
    {
        if (platforms)
        {
            foreach (GameObject go in movingPlatforms)
            {

                go.transform.Translate(0, 1 * Time.deltaTime, 0);
                if (go.transform.position.y >= -10)
                {
                    go.transform.position = new Vector3(go.transform.position.x, -10, 0);
                }
            }
        }
        else
        {
            foreach (GameObject go in movingPlatforms)
            {

                go.transform.Translate(0, 1 * Time.deltaTime, 0);
                if (go.transform.position.y <= -10)
                {
                    go.transform.position = new Vector3(go.transform.position.x, -10, 0);
                }
                Destroy(go);
            }
        }


        if (!ground)
        {
            foreach (GameObject go in allSprites)
            {

                go.transform.Translate(0, -1 * Time.deltaTime, 0);
                if (go.transform.position.y < -10)
                {
                    go.transform.position = new Vector3(go.transform.position.x, -6, 0);
                }
            }
        }
        else
        {
            foreach (GameObject go in allSprites)
            {

                //go.transform.Translate(0, -1 * Time.deltaTime, 0);
                if (go.transform.position.y > -6)
                {
                    go.transform.position = new Vector3(go.transform.position.x, -6, 0);
                }
            }
        }
    }

    public void GeneratePlatform(Vector3 _Position)
    {
        Vector3 position = new Vector3(_Position.x - platform[1].GetComponentInChildren<BoxCollider2D>().size.x, -10, 0);
        GameObject newplat = Instantiate(platform[1], position, Quaternion.identity); //Create platform at different position
        movingPlatforms.Add(newplat);
        newplat.SetActive(true); //Show the platform
    }

    public void ToggleGround()
    {
        ground = !ground;
    }

    public void TogglePlatforms()
    {
        platforms = !platforms;
    }
}

