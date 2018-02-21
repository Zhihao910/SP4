using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldscript : MonoBehaviour
{
    float lifetime;
    float movement;
    float change;
    float change2;

    // Use this for initialization
    void Start()
    {
        lifetime = 5;
        movement = 0.2f;
        change = 0;
        change = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
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
        if (lifetime < 0)
            Destroy(gameObject);

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().invincible2 = true;
            Destroy(this.gameObject);
        }
    }
}
