using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldscript : MonoBehaviour
{
    float movement;
    float change;
    float change2;
    private GameObject _feedback;
    // Use this for initialization
    void Start()
    {
        movement = 0.2f;
        change = 0;
        change = 0;
        _feedback = GameObject.FindGameObjectWithTag("Feedback");
    }

    // Update is called once per frame
    void Update()
    {
        //code for hovering
        if (change > 0.8f)
        {
            movement = -0.2f;
            change = 0;
        }
        if (movement == -0.2f)
        {
            change2 += Time.deltaTime;
        }
        if (movement == 0.2f)
        {
            change += Time.deltaTime;
        }
        if (change2 > 0.8f)
        {
            movement = 0.2f;
            change2 = 0;
        }

        transform.Translate(0, movement * Time.deltaTime, 0);

        Destroy(gameObject, 5.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _feedback.GetComponent<Feedback>().CreateImage("shieldimage", other.gameObject, 5.0f);
            other.GetComponentInParent<PlayerController>().invincible2 = true;
            Destroy(gameObject);
        }
    }
}
