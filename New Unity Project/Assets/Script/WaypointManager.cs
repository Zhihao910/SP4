using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaypointManager : MonoBehaviour
{
    public GameObject[] Waypoint;
    public int num = 0;
    Vector3 target;

    float minDistance = 1;
    float speed = 2;

    bool go = false;
    bool confirmEnter = false;

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            go = true;
        }

        if (go)
        {
            animator.SetInteger("States", 2);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed * Time.deltaTime);
            //print(gameObject.transform.position);
            if (gameObject.transform.position == target)
                animator.SetInteger("States", 3);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Waypoint"))
        {
            print("TouchedWaypoint");
            SceneManager.LoadScene("OptionScene");
        }
        else if (other.CompareTag("Waypoint1"))
        {
            print("TouchedWaypoint1");
            SceneManager.LoadScene("Help");
        }
        else if (other.CompareTag("Waypoint2"))
        {
            print("TouchedWaypoint2");
            SceneManager.LoadScene("MusicSelection");
        }
        else if (other.CompareTag("Waypoint3"))
        {
            print("TouchedWaypoint3");
            SceneManager.LoadScene("HighscoreScene");
        }
    }
}
