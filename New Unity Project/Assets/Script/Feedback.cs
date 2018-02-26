using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Create an object. _name doesnt need "Prefabs/". Lifetime default is 1.0f.
    public bool Create(string _name, Vector3 _pos, float _lifetime = 1.0f)
    {
        print("made!");
        print(_pos);
        GameObject _temp = Instantiate(Resources.Load("Prefabs/" + _name) as GameObject);

        _pos.z = -1;

        _temp.transform.localPosition = _pos;

        Destroy(_temp, _lifetime);

        return false;
    }

    // Use this if you want it to follow something
    public bool Create(string _name, GameObject _go, float _lifetime = 1.0f)
    {
        // I'm not sure what I'm doing here tbh fam
        GameObject _temp = Instantiate(Resources.Load("Prefabs/" + _name) as GameObject);

        _temp.transform.parent = _go.transform;

        Destroy(_temp, _lifetime);

        return false;
    }
}