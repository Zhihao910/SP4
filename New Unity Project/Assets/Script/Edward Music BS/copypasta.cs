﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copypasta : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        GameObject _cube = Resources.Load<GameObject>("beating-cube");
        GameObject _paramcube = Resources.Load<GameObject>("parametric-cube");

        float _spawn = -8;

		for (byte i = 0; i < 64; ++i)
        {
            GameObject _coob = Instantiate(_cube);
            _coob.transform.position = new Vector3(_spawn, 4, 7.5f);
            _coob.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);

            GameObject _paramcoob = Instantiate(_paramcube);
            _paramcoob.GetComponent<ParamCube>()._band = i;
            _paramcoob.transform.position = new Vector3(_spawn, 0, 7.5f);
            _paramcoob.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            _spawn += 0.25f;
        }
	}
}
