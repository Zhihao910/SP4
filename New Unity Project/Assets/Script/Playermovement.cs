using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{

    [SerializeField]
    float movementspeed = 10;

    bool jump2 = false;
    bool jumping = false;
    bool jumpOnce = false;
    // Use this for initialization
    void Start()
    {
        jump2 = false;
        jumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * movementspeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * movementspeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space)&&transform.position.y<=0.5f)
        {
            jumping = true;
            jump2 = false;
        }
            if (jumping == true)
            {
                transform.position += transform.up * 10 * Time.deltaTime;
            }
            if (transform.position.y >= 3)
            {
                jumping = false;
                jump2 = true;
            }
            if (jump2 == true)
            {
                if (transform.position.y >= 0.5)
                    transform.position -= transform.up * 10 * Time.deltaTime;
                if (transform.position.y < 0.5)
                    transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            }
        
    }
}
