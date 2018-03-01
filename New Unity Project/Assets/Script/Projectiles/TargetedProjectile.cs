using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedProjectile : Projectile {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;
        lifeTime = 999f;
	}
	
	// Update is called once per frame
	void Update () {
        target = player.transform.position;
        //transform.LookAt(target);
        dir = (target - transform.position).normalized;
        dir.z = 0;
        if((target - transform.position).magnitude < 2.5)
        {
            Texture2D tex = Resources.Load("Sprite/musicalNote2") as Texture2D;
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            print(GetComponent<SpriteRenderer>().sprite.name);
            gameObject.tag = "parableProjectile";
        }
        base.Update();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);

    }
}
