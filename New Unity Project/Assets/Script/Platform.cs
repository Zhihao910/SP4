using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float projectileSpeed = 1;
    Vector3 dir;
    Vector3 target = new Vector3(999, 999, 999);
    Vector3 pos;
    public bool despawn;
    protected bool hittarget = false;

    // Use this for initialization
    void Start () {
        projectileSpeed = 1;
    }
	
	// Update is called once per frame
	void Update () {
        pos = transform.position;
        if (!hittarget)
        {
            print((target - transform.position).magnitude);
            transform.Translate(dir.x * projectileSpeed * Time.deltaTime, dir.y * projectileSpeed * Time.deltaTime, 0);
            if (null != target)
            {
                if ((target - transform.position).magnitude < 0.01f)
                    hittarget = true;
            }
        }
    }


    public void SetDir(Vector3 _Dir)
    {
        dir = _Dir;
    }

    public void SetTarget(Vector3 _Target)
    {
        target = _Target;
    }
}
