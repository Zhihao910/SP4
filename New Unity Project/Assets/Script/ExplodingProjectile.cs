using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile {

    [SerializeField]
    GameObject proj2;

    int SplitCount = 8;

    // Use this for initialization
    protected void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	protected void Update () {
        base.Update();
        if(hittarget)
        {
            for(float i = 0; i < 360; i += 360/SplitCount)
            {
                GameObject go = Instantiate(proj2, transform.position, Quaternion.identity);
                float rad = i * (Mathf.PI / 180);
                go.GetComponent<Projectile>().SetDir((new Vector3(Mathf.Cos(rad), Mathf.Sin(rad))).normalized);
                go.GetComponent<Projectile>().projectileSpeed = 3;
            }
            Destroy(gameObject);
        }
	}

    public void SetSplitCount(int _count)
    {
        SplitCount = _count;
    }
}
