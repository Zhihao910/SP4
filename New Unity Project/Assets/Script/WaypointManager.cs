using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public GameObject[] Waypoint;
    public int num = 0;

    float minDistance = 1;
    float speed = 2;

    bool go = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, Waypoint[num].transform.position);

        if (go)
        {
            if (distance > minDistance)
                Move();
            else
            {
                if (num + 1 == Waypoint.Length)
                    num = 0;
                else num++;
            }
        }

    }

    public void Move()
    {
        gameObject.transform.LookAt(Waypoint[num].transform.position);
        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;


    }
}
