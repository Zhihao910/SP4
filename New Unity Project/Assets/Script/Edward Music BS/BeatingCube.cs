using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatingCube : MonoBehaviour
{
    private Vector3 _originalScale;
    public float _scaleMultiplier;

    // Use this for initialization
    void Start()
    {
        _originalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        beatCube();
    }

    //private bool isBeat()
    //{
    //    return BPM._bpmbeat;
    //}

    private void beatCube()
    {
        if (BPM._bpmbeat)
        {
            transform.localScale = new Vector3(
                _originalScale.x * _scaleMultiplier,
                _originalScale.y * _scaleMultiplier,
                _originalScale.z * _scaleMultiplier);
        }
        else
        {
            transform.localScale = _originalScale;
        }
    }
}
