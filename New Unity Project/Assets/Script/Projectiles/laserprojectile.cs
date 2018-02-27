using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserprojectile : Projectile
{
    [SerializeField]
    GameObject Laser;

    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (hittarget)
        {
            GameObject go = Instantiate(Laser, transform.position, Quaternion.identity);
            go.GetComponent<Projectile>().SetDir(new Vector3(1, 0, 0));
            Destroy(gameObject);
        }
    }
}