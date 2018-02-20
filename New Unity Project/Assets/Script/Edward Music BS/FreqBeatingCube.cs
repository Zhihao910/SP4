using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqBeatingCube : MonoBehaviour
{
    public int _band;
    private Vector3 _originalScale;
    public float _scaleMultiplier;

    GameObject _effectBass;
    GameObject _effectKick;
    GameObject _effectCenter;
    GameObject _effectMelody;
    GameObject _effectHigh;

    // Use this for initialization
    void Start()
    {
        _originalScale = gameObject.transform.localScale;

        _effectBass = Resources.Load<GameObject>("effectBass");
        _effectKick = Resources.Load<GameObject>("effectKick");
        _effectCenter = Resources.Load<GameObject>("effectCenter");
        _effectMelody = Resources.Load<GameObject>("effectMelody");
        _effectHigh = Resources.Load<GameObject>("effectHigh");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //beatCube();
        soundCube();

        // If like, you know, there's a thing..
        // uhh. make more things
        //spawnEffect();

        // Reset all values (HAHAA TOTALLY NOT HARDCODED HAAAAAAAAA
        if (_band == 63)
        {
            SpawnEffect._spawnBass = false;
            SpawnEffect._spawnKick = false;
            SpawnEffect._spawnCenter = false;
            SpawnEffect._spawnMelody = false;
            SpawnEffect._spawnHigh = false;
        }
    }

    //private bool isBeat()
    //{
    //    return BPM._bpmbeat;
    //}

    private void beatCube()
    {
        if (FreqBeat._freqbeat[_band])
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

    // 0 - 3    bass
    // 4 - 6    kick
    // 7 - 11   forgotten middle child (melody??) (WHAT?)
    // 12 - 18  melody
    ///  7 to 18 also happens to be vocals + instruments ok
    // 19 - 63  unstable time bombs that may erupt and kill the entire earth

    private void soundCube()
    {
        // Just to see in what order it prints in
        //print(_band);

        if (AudioPeer._audioBandBuffer64[_band] > (FreqBeat._highList[FreqBeat._highSection][_band])) // * 0.4f
        {
            transform.localScale = new Vector3(
                _originalScale.x * _scaleMultiplier,
                _originalScale.y * _scaleMultiplier,
                _originalScale.z * _scaleMultiplier);

            if (!SpawnEffect._spawnHigh && _band >= 19)
            {
                //print("spawn high");

                GameObject _spawn = Instantiate(_effectHigh);
                Destroy(_spawn, 0.05f);

                SpawnEffect._spawnHigh = true;
            }
            else if (!SpawnEffect._spawnMelody && _band >= 12 && _band < 19)
            {
                //print("spawn melody");

                GameObject _spawn = Instantiate(_effectMelody);
                Destroy(_spawn, 0.05f);

                SpawnEffect._spawnMelody = true;
            }
            else if (!SpawnEffect._spawnCenter && _band >= 6 && _band < 12)
            {
                //print("spawn center");

                GameObject _spawn = Instantiate(_effectCenter);
                Destroy(_spawn, 0.05f);

                SpawnEffect._spawnCenter = true;
            }
            else if (!SpawnEffect._spawnKick && _band >= 4 && _band < 6)
            {
                //print("spawn kick");

                GameObject _spawn = Instantiate(_effectKick);
                Destroy(_spawn, 0.05f);

                SpawnEffect._spawnKick = true;
            }
            else if (!SpawnEffect._spawnBass && _band < 4)
            {
                //print("spawn bass");

                GameObject _spawn = Instantiate(_effectBass);
                Destroy(_spawn, 0.05f);

                SpawnEffect._spawnBass = true;
            }
        }
        else
        {
            transform.localScale = _originalScale;
        }
    }

    //private void spawnEffect()
    //{
    //    if (_spawnBass)
    //    {

    //    }

    //    if (_spawnKick)
    //    {

    //    }

    //    if (_spawnMelody)
    //    {

    //    }

    //    if(_spawnHigh)
    //    {

    //    }
    //}
}
