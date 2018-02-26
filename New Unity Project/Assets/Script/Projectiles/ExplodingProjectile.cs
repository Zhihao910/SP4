using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile
{

    [SerializeField]
    GameObject proj2;

    private GameObject _feedback;

    int SplitCount = 8;

    // Use this for initialization
    protected void Start ()
    {
        _feedback = GameObject.FindGameObjectWithTag("Feedback");
        base.Start();
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        base.Update();
        if(hittarget)
        {
            _feedback.GetComponent<Feedback>().CreateImage("ParryFail", gameObject.transform.position);
            _feedback.GetComponent<Feedback>().CreateAudio("Fail");

            for (float i = 0; i < 360; i += 360/SplitCount)
            {
                GameObject go = Instantiate(proj2, transform.position, Quaternion.identity);
                float rad = i * (Mathf.PI / 180);
                go.GetComponent<Projectile>().SetDir((new Vector3(Mathf.Cos(rad), Mathf.Sin(rad))).normalized);
                go.GetComponent<Projectile>().SetSpeed(3);
            }
            Destroy(transform.parent.gameObject);
        }
	}

    public void SetSplitCount(int _count)
    {
        SplitCount = _count;
    }
}
