using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour {

    int Beat;
    double StartTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBeat(int _beat)
    {
        Beat = _beat;
    }

    public void SetTime(double _time)
    {
        StartTime = _time;
    }

    public double GetTime()
    {
        return StartTime;
    }

}
