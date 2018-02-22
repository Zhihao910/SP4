using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : Projectile {

    [SerializeField]
    GameObject _shockwave;

    int height = 5;

    // Use this for initialization
    protected void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	protected void Update () {
        base.Update();
        if(hittarget)
        {
            //for(float i = 0; i < 2; i += 360/SplitCount)
            //{
            //    GameObject go = Instantiate(proj2, transform.position, Quaternion.identity);
            //    float rad = i * (Mathf.PI / 180);
            //    go.GetComponent<Projectile>().SetDir((new Vector3(Mathf.Cos(rad), Mathf.Sin(rad))).normalized);
            //    go.GetComponent<Projectile>().projectileSpeed = 3;
            //}

            for (int i = 0; i < height; ++i)
            {
                GameObject left = Instantiate(_shockwave, transform.position, Quaternion.identity);
                left.GetComponent<Projectile>().SetDir(new Vector3(-1, 0));
                left.GetComponent<Projectile>().SetSpeed(5);
                left.GetComponent<Projectile>().SetHeight(i);

                GameObject right = Instantiate(_shockwave, transform.position, Quaternion.identity);
                right.GetComponent<Projectile>().SetDir(new Vector3(1, 0));
                right.GetComponent<Projectile>().SetSpeed(5);
                right.GetComponent<Projectile>().SetHeight(i);
            }

            Destroy(gameObject);
        }
	}

    public void SetHeight(int _height)
    {
        height = _height;
    }
}
