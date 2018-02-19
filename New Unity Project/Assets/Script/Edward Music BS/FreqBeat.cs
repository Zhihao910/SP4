using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqBeat : MonoBehaviour
{
    public AudioSource _source;

    // if there is a beat, _beated = true;
    public static bool[] _freqbeat          = new bool[64];

    // History Buffer
    //private float[,] _historyBuffer = new float[64, 13];
    private float[][] _historyBufferArray   = new float[64][];

    //// Right and Left audio channels
    //private float[] _rightChannel;
    //private float[] _leftChannel;

    //// Take in 512 samples, so size is 512
    //private int _sampleSize = 1024;

    // current/average values from spectrogram
    private float[] _instantEnergy          = new float[64];
    private float[] _averageEnergy          = new float[64];

    // Variance calculated for more accuracy in beat detection
    private float[] _variance               = new float[64];
    private float[] _sumVariance            = new float[64];

    // Constant
    private float[] _constant               = new float[64];

    // Highs
    public static float[][] _highs          = new float[64][];

    // HighsSection
    public static int _highSection;

    // HighsBeat
    private int _highBeat;

    // HighsBeat Added
    private bool _highAdd;

    // or every few seconds?
    private double _highTime;

    // Check if detection is done
    private bool _detectedFinish = false;

    // Use this for initialization
    void Start ()
    {
        //_source = gameObject.GetComponent<AudioSource>();

        _highSection = 0;
        _highBeat = 0;
        _highAdd = false;

        for (int i = 0; i < 64; ++i)
        {
            _freqbeat[i] = false;
            _historyBufferArray[i] = new float[43];
            _highs[i] = new float[21600];
            // who the fuck puts a song thats 24 hours long?
            // this is enough.
        }

        //_source.pitch = 10;
        //Time.timeScale = 10;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!_source.isPlaying)
        {
            if (!_detectedFinish)
            {
                _detectedFinish = true;

                //for (int i = 0; i < _highs.Length; ++i)
                //{
                //    _highs[i][_highSection] *= 0.2f;
                //    //_highs[i][_highSection] *= (0.3f + ((float)i * 0.02f));
                //    //// AAAAAAAAAAAAAAAAAH
                //    //// HOW DO I DO THIS?

                //    //if (i > 5)
                //    //{
                //    //  _highs[i][_highSection] *= (Mathf.Sin((float)i * 0.36f));
                //    //}
                //    //else if (i > 2)
                //    //{
                //    //    _highs[i][_highSection] *= (Mathf.Sin((float)i * 0.2f));
                //    //}
                //    //else
                //    //{
                //    //    _highs[i][_highSection] *= 0.3f;
                //    //}
                //}

                Debug.Log("DONE DETECTING FREQ -----------------------------------------------------------");
            }

            _source.Play();

            _highBeat = 0;
            _highSection = 0;
            _highTime = 0.0;
        }

        if (_detectedFinish)
        {
            //_source.pitch = 1;
            //Time.timeScale = 1;

            SpawnEffect._spawnBass = false;
            SpawnEffect._spawnKick = false;
            SpawnEffect._spawnCenter = false;
            SpawnEffect._spawnMelody = false;
            SpawnEffect._spawnHigh = false;

            for (int i = 0; i < 64; ++i)
            {
                if (AudioPeer._audioBandBuffer64[i] > _highs[i][_highSection])
                {
                    if (!SpawnEffect._spawnHigh && i >= 19)
                    {
                        //print("spawn high");

                        SpawnEffect._spawnHigh = true;
                    }
                    else if (!SpawnEffect._spawnMelody && i >= 12 && i < 19)
                    {
                        //print("spawn melody");

                        SpawnEffect._spawnMelody = true;
                    }
                    else if (!SpawnEffect._spawnCenter && i >= 6 && i < 12)
                    {
                        //print("spawn center");

                        SpawnEffect._spawnCenter = true;
                    }
                    else if (!SpawnEffect._spawnKick && i >= 4 && i < 6)
                    {
                        //print("spawn kick");

                        SpawnEffect._spawnKick = true;
                    }
                    else if (!SpawnEffect._spawnBass && i > 2 && i < 4) // bass is 0-3 but
                    {                                                   // it goes in a wave
                        //print("spawn bass");                          // so like, yeah

                        SpawnEffect._spawnBass = true;
                    }
                }
            }
        }
        else
        {
            Detect();
        }

        _highTime += Time.deltaTime;

        // 16 or 8 or 4
        if (_highTime > 8.0)
        {
            if (!_detectedFinish)
            {
                // 0 - 3    bass
                // 4 - 6    kick
                // 7 - 11   forgotten middle child (melody??) (WHAT?)
                // 12 - 18  melody
                ///  7 to 18 also happens to be vocals + instruments ok
                // 19 - 63  unstable time bombs that may erupt and kill the entire earth

                for (int i = 0; i < 64; ++i)
                {
                    if (i > 18)
                    {
                        _highs[i][_highSection] *= 0.8f; // 0.7
                    }
                    else if (i > 6)
                    {
                        _highs[i][_highSection] *= 0.6f; // 0.5
                    }
                    else if (i > 3)
                    {
                        _highs[i][_highSection] *= 0.7f; //0.7
                    }
                    else
                    {
                        _highs[i][_highSection] *= 0.5f; //0.5
                    }

                    //if (i > 20)
                    //{
                    //    //_highs[i][_highSection] *= 0.6f;
                    //    _highs[i][_highSection] *= 0.7f;
                    //}
                    //else if (i > 8)
                    //{
                    //    //_highs[i][_highSection] *= 0.4f;
                    //    _highs[i][_highSection] *= 0.5f;
                    //}
                    //else
                    //{
                    //    // Bass, generally okay
                    //    //_highs[i][_highSection] *= 2.5f;
                    //    _highs[i][_highSection] *= 0.5f;
                    //}
                }
            }

            _highTime = 0.0;
          ++_highSection;
        }

        /*
        if (BPM._bpmbeat && !_highAdd)
        {
            _highAdd = true;
            ++_highBeat;
        }
        else
        {
            _highAdd = false;
        }

        if (_highBeat >= 32)
        {
            if (!_detectedFinish)
            {
                for (int i = 0; i < _highs.Length; ++i)
                {
                    _highs[i][_highSection] *= (0.3f + ((float)i * 0.01f));
                }
            }

            _highBeat = 0;
            ++_highSection;
        }*/
    }


    private void Detect()
    {
        // THIS IS SO CONFUSING OMG
        for (int i = 0; i < 64; ++i)
        {
            _instantEnergy[i] = AudioPeer._audioBandBuffer64[i];

            _averageEnergy[i] = (512 / _historyBufferArray[i].Length) * SumHistoryEnergy(_historyBufferArray[i]);
            //_averageEnergy[i] = SumHistoryEnergy(_historyBufferArray[i]) / _historyBufferArray[i].Length;

            _variance[i] = SumVariance(_historyBufferArray[i], _averageEnergy[i]) / _historyBufferArray[i].Length;

            _constant[i] = (float)((-0.0025714 * _variance[i]) + 1.5142857);

            // Used as a temp array to shift history buffer
            float[] _shiftingHistoryBuffer = new float[_historyBufferArray[i].Length];

            for (int k = 0; k < (_historyBufferArray[i].Length - 1); ++k)
            {
                // Shift sound enegery history buffer 1 index to the right
                // This flushes the oldest one out too
                _shiftingHistoryBuffer[k + 1] = _historyBufferArray[i][k];
            }

            // newest energy value at the front
            _shiftingHistoryBuffer[0] = _instantEnergy[i];

            for (int k = 0; k < _historyBufferArray[i].Length; ++k)
            {
                // transfer values back to original array
                _historyBufferArray[i][k] = _shiftingHistoryBuffer[k];
            }
        }

        IsHigher(_instantEnergy);

        for (int i = 0; i < 64; ++i)
        {
            // Compare instant energy to constant * average energy
            if (_instantEnergy[i] > (_constant[i] * _averageEnergy[i]))
            {
                // If it is more, we have a beat!
                if (!_freqbeat[i])
                {
                    //Debug.Log("FreqBeat!");
                    _freqbeat[i] = true;
                }
                else
                {
                    if (_freqbeat[i])
                    {
                        _freqbeat[i] = false;
                    }
                }
            }
        }
    }

    // Part of Formula (R2), calculating historical energy
    private float SumHistoryEnergy(float[] historyBuffer)
    {
        // Average Historical Energy
        float ae = 0;

        for (int i = 0; i < historyBuffer.Length; ++i)
        {
            float toSquare = historyBuffer[i];
            ae += (toSquare * toSquare);
        }

        return ae;
    }

    // Formula (R4)
    private float SumVariance(float[] historyBuffer, float _avgEnergy)
    {
        // sum of variance
        float sv = 0;

        for (int i = 0; i < historyBuffer.Length; ++i)
        {
            float toSquare = historyBuffer[i] - _avgEnergy;
            sv += (toSquare * toSquare);
        }

        return sv;
    }

    private void IsHigher(float[] _energy)
    {
        for (int i = 0; i < _energy.Length; ++i)
        {
            if (_highs[i][_highSection] < _energy[i])
            {
                _highs[i][_highSection] = _energy[i];
            }
        }
    }
}
