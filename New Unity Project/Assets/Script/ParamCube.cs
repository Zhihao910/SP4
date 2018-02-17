using UnityEngine;

sealed public class ParamCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _scaleMultiplier;

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 LS = transform.localScale;
        transform.localScale = new Vector3(LS.x, (AudioPeer._audioBandBuffer64[_band] * _scaleMultiplier) + _startScale, LS.z);
	}
}
