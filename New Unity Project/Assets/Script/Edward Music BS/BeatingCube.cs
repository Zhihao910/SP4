using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatingCube : MonoBehaviour
{
    private Vector3 _originalScale;
    public float _scaleMultiplier;

    private BpmAnalyzer _ba;
    double _beatTime = 0.0;

    // Use this for initialization
    void Start()
    {
        _originalScale = gameObject.transform.localScale;
        // change this to read from file
        _ba = GameObject.FindGameObjectWithTag("Boss").GetComponent<BpmAnalyzer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _beatTime += (Time.deltaTime * 0.25f);
        beatCube();
    }

    //private bool isBeat()
    //{
    //    return BPM._bpmbeat;
    //}

    private void beatCube()
    {
        if (_beatTime > _ba.GetBeatTime())
        {
            transform.localScale = new Vector3(
                _originalScale.x * _scaleMultiplier,
                _originalScale.y * _scaleMultiplier,
                _originalScale.z * _scaleMultiplier);

            if (_beatTime > (_ba.GetBeatTime() * 1.1f))
            {
                _beatTime = 0.0;
            }
        }
        else
        {
            transform.localScale = _originalScale;
        }
    }
}
