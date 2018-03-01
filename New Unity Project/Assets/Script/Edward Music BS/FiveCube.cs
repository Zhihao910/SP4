using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveCube : MonoBehaviour
{
    public int _range;
    private Vector3 _originalScale;
    public float _scaleMultiplier;
    private bool _beat;

    //public particles _particle;

	// Use this for initialization
	void Start ()
    {
        _originalScale = gameObject.transform.localScale;
        _scaleMultiplier = 5.0f;
        _beat = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        beatCube();
	}

    private void beatCube()
    {
        switch (_range)
        {
            case 0:
                for (int i = 0; i < 4; ++i)
                {
                    _beat = (AudioPeer._audioBandBuffer64[i] > 0.6f);

                    if (_beat)
                        break;
                }
                break;
            case 1:
                for (int i = 4; i < 6; ++i)
                {
                    _beat = (AudioPeer._audioBandBuffer64[i] > 0.6f);

                    if (_beat)
                        break;
                }
                break;
            case 2:
                for (int i = 6; i < 12; ++i)
                {
                    _beat = (AudioPeer._audioBandBuffer64[i] > 0.5f);

                    if (_beat)
                        break;
                }
                break;
            case 3:
                for (int i = 12; i < 19; ++i)
                {
                    _beat = (AudioPeer._audioBandBuffer64[i] > 0.6f);

                    if (_beat)
                        break;
                }
                break;
            case 4:
                for (int i = 19; i < 64; ++i)
                {
                    _beat = (AudioPeer._audioBandBuffer64[i] > 0.7f);

                    if (_beat)
                        break;
                }
                break;
        }

        if (_beat)
        {
            transform.localScale = new Vector3(
                _originalScale.x * _scaleMultiplier,
                _originalScale.y * _scaleMultiplier,
                _originalScale.z * _scaleMultiplier);


            //_particle.ApplyParticle(gameObject, "beatParticle", 1.0f, 3.0f);
        }
        else
        {
            transform.localScale = _originalScale;
        }
    }
}
