using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTETappable : MonoBehaviour
{
    public static bool _tapped;

	// Use this for initialization
	void Start ()
    {
        _tapped = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RunOnClick()
    {
        _tapped = true;
        //Destroy(this, 0.2f);
    }
}
