using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttack : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("parableProjectile"))
        {
            Destroy(other.gameObject);
            Debug.Log("gained mana");
        }
    }
}
