﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copypasta : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        GameObject _cube = Resources.Load<GameObject>("FreqBeat-cube");
        GameObject _paramcube = Resources.Load<GameObject>("parametric-cube");

		for (byte i = 0; i < 64; ++i)
        {
            GameObject _coob = Instantiate(_cube);
            _coob.GetComponent<FreqBeatingCube>()._band = i;
            _coob.transform.position = new Vector3(i, -1, i);
            _coob.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            GameObject _paramcoob = Instantiate(_paramcube);
            _paramcoob.GetComponent<ParamCube>()._band = i;
            _paramcoob.transform.position = new Vector3(i, 0, i);
            _paramcoob.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
        }
	}
}
