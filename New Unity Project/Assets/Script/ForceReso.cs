using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Do we need this? 
// Add to main cam
public class ForceReso : MonoBehaviour
{
    float _aspectRatio = (16.0f / 9.0f);

	// Use this for initialization
	void Start ()
    {
        float _variance = _aspectRatio / Camera.main.aspect;

        if (_variance < 1.0f)
        {
            Camera.main.rect = new Rect((1.0f - _variance) * 0.5f, 0, _variance, 1.0f);
        }
        else
        {
            _variance = 1.0f / _variance;
            Camera.main.rect = new Rect(0, (1.0f - _variance) * 0.5f, 1.0f, _variance);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
    }
}
