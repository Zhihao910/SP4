using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Duration of Shake
    private float _shakeTime;
    // Intensity of Shake
    private float _shakeIntensity;
    // Amount to drop shake intensity by
    private float _shakeReduction;

	// Use this for initialization
	void Start ()
    {
        _shakeTime = 0.0f;
        _shakeIntensity = 0.3f;
        _shakeReduction = 0.95f;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (_shakeTime > 0.0f)
        {
            transform.localPosition = (Random.insideUnitSphere * _shakeIntensity);
            _shakeIntensity *= _shakeReduction;
            _shakeTime -= Time.deltaTime;

            if (_shakeIntensity <= Mathf.Epsilon)
            {
                _shakeTime = 0.0f;
            }
        }
	}

    public void ShakeCamera(float _sT = 1.0f, float _sI = 0.3f, float _sR = 0.95f)
    {
        // Default function is a smol shake
        _shakeTime = _sT;
        _shakeIntensity = _sI;
        _shakeReduction = _sR;
    }
}
