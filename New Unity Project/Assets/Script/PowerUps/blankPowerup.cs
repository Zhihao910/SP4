using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blankPowerup : MonoBehaviour {
    float movement;
    float change;
    float change2;
    // Use this for initialization
    void Start () {
        movement = 0.2f;
        change = 0;
        change = 0;
    }
	
	// Update is called once per frame
	void Update () {
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
        GameObject[] gameObjects;
        if (other.CompareTag("Player"))
        {
            gameObjects = GameObject.FindGameObjectsWithTag("Projectile");
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Destroy(gameObjects[i]);
            }
            Destroy(this.gameObject);
        }
    }
}
