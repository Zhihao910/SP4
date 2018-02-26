using UnityEngine;

sealed public class ParamCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _scaleMultiplier;

    void Start()
    {
        _startScale = 1.0f;
        _scaleMultiplier = 7.5f;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 LS = transform.localScale;
        transform.localScale = new Vector3(LS.x, (AudioPeer._audioBandBuffer64[_band] * _scaleMultiplier) + _startScale, LS.z);

        Vector3 LP = transform.localPosition;
        transform.localPosition = new Vector3(LP.x, (AudioPeer._audioBandBuffer64[_band] * _scaleMultiplier * 0.5f) - 5.0f, LP.z);
	}
}
