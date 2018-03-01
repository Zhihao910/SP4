using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greet : MonoBehaviour
{
    Feedback _fb;

    // Use this for initialization
	void Start ()
    {
        _fb = GetComponent<Feedback>();
        _fb.CreateAudio("Greeting");
	}
}
