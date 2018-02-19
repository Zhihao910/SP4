using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPM : MonoBehaviour
{
    // AudioSource
    public AudioSource _audioSource;

    // if there is a beat, _beated = true
    private bool _beated;

    // actual beats
    public static bool _bpmbeat;

    // History Buffer
    private float[] _historyBuffer = new float[22];

    // Right and Left audio channels
    private float[] _rightChannel;
    private float[] _leftChannel;

    // Take in 512 samples, so size is 512
    private int _sampleSize = 1024;

    private float[] _samples = new float[1024];

    // current/average values from spectrogram
    private float _instantEnergy;
    private float _averageEnergy;

    // Variance calculated for more accuracy in beat detection
    private float _variance;
    private float _sumVariance;

    // Constant
    private float _constant;

    // Time between beats
    private double _beatTime;

    // Time counting up until nextbeat
    private double _nextBeat;

    // Temp time between beats in Detect()
    private double _tempBeatTime;

    // Temp time counting for next beat in Detect()
    private double _tempNextBeat;

    // Temp beat count
    private int _tempBeatCount;

    // Used for WithinRange() cuz tempo
    private int _beatCount;

    // 4 beats
    // what if the time signature is different?
    // well fuck
    private double[] _tempo = new double[4];

    // previous saved bpms
    //private double[] _bpms = new double[10];

    // Beats Per Minute
    private double _bpm;

    // List of bpms
    private List<double> _bpmList = new List<double>();

    // Check if detection is done
    private bool _detectedFinish = false;

    // Use this for initialization
    void Start ()
    {
        _beated = false;
        _bpmbeat = false;

        _beatCount = 1;

        _nextBeat = 0.0;

        // assume default is 128 BPM
        _beatTime = 0.468;

        _bpm = 60.0 / _beatTime;

        for (int i = 0; i < _tempo.Length; ++i)
        {
            _tempo[i] = _beatTime;
        }

        //for (int i = 0; i < _bpms.Length; ++i)
        //{
        //    _bpms[i] = 0.0;
        //}

        //songOver = 254.0;
        //_addTime = 0.0;

        //divideBy = 0;

        //Time.timeScale = 5;
        //_audioSource.pitch = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        _nextBeat += Time.deltaTime;
        _tempNextBeat += Time.deltaTime;
        //_addTime += Time.deltaTime;

        if (!_audioSource.isPlaying)
        {
            if (!_detectedFinish)
            {
                _detectedFinish = true;

                foreach (double _bpmIndex in _bpmList)
                {
                    _bpm += _bpmIndex;
                }

                _bpm /= _bpmList.Capacity;

                _beatTime = 60.0 / _bpm;

                Debug.Log("DONE DETECTING BPM -----------------------------------------------------------");
            }
        }

        if (_detectedFinish)
        {
            //Time.timeScale = 1;
            //_audioSource.pitch = 1;
        }
        else
        {
            Detect();
        }

        if (_bpm < 30.0)
        {
            _bpm *= 5;
            _beatTime /= 5;
        }
        
        if (_bpm < 58)
        {
            _bpm *= 2;
            _beatTime /= 2;
        }

        if (_nextBeat > _beatTime)
        {
            _bpmbeat = true;
            _nextBeat = 0.0;
            Debug.Log(_bpm);
            Debug.Log(FreqBeat._highSection);
            //Debug.Log("BPM BEAT");
        }
        else
        {
            _bpmbeat = false;
        }

        //isBeat();
    }

    private void Detect()
    {
        // Referenced from:
        // https://www.gamedev.net/articles/programming/math-and-physics/beat-detection-algorithms-r1952/?tab=comments
        // Formulas will be listed in their style too

        // Hamming? Maybe see if others work better.
        // Maybe write my own.

        // Fetch current playing time spec
        //_instantEnergy = SumStereo(_audioSource.GetSpectrumData(_sampleSize, 0, FFTWindow.Hamming));
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Hamming);
        _instantEnergy = SumStereo(_samples);

        //_rightChannel = _audioSource.GetSpectrumData(1024, 1, FFTWindow.Hamming);
        //_leftChannel = _audioSource.GetSpectrumData(1024, 2, FFTWindow.Hamming);

        //_instantEnergy = SumChannels(_rightChannel, _leftChannel);

        // Formula (R2)
        _averageEnergy = (_sampleSize / _historyBuffer.Length) * SumHistoryEnergy(_historyBuffer);
        //_averageEnergy = SumHistoryEnergy(_historyBuffer) / _historyBuffer.Length;

        // Calculate Variance
        _variance = SumVariance(_historyBuffer) / _historyBuffer.Length;

        //for (int i = 0; i < 43; ++i)
        //{
        //    float toSquare = (_historyBuffer[i] - _averageEnergy);
        //    _sumVariance += (toSquare * toSquare);
        //}

        //_variance = _sumVariance / _historyBuffer.Length;

        // Calculate Constant
        // I have no idea what this means.
        // Formula (R6)
        _constant = (float)((-0.0025714 * _variance) + 1.5142857);
        //_constant = 1.5f;

        // Used as a temp array to shift history buffer
        float[] _shiftingHistoryBuffer = new float[_historyBuffer.Length];

        for (int i = 0; i < (_historyBuffer.Length - 1); ++i)
        {
            // Shift sound enegery history buffer 1 index to the right
            // This flushes the oldest one out too
            _shiftingHistoryBuffer[i + 1] = _historyBuffer[i];
        }

        // newest energy value at the front
        _shiftingHistoryBuffer[0] = _instantEnergy;

        for (int i = 0; i < _historyBuffer.Length; ++i)
        {
            // transfer values back to original array
            _historyBuffer[i] = _shiftingHistoryBuffer[i];
        }

        // Compare instant energy to constant * average energy
        if (_instantEnergy > (_constant * _averageEnergy))
        {
            // If it is more, we have a beat!
            if (!_beated)
            {
                _tempBeatTime = _tempNextBeat;
                _tempNextBeat = 0.0;

                //Debug.Log("Beat!");
                _beated = true;

                //Debug.Log(_beatTime);
                //Debug.Log(_tempo[0]);
                //Debug.Log(_tempBeatTime);
                //Debug.Log(_tempo[0] + _tempBeatTime);

                if (WithinRange(_tempo[1], _tempo[0] + _tempBeatTime))
                {
                    //Debug.Log("OUTLIER, go back");

                    _tempo[0] = _tempBeatTime + _tempo[0];

                    _beatCount = _tempBeatCount + 1;

                    //Debug.Log(_beatCount);
                    //Debug.Log("CONTINUE");

                    if (_beatCount >= 4)
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            _beatTime += _tempo[i];
                        }
                        _beatTime /= 4;

                        _bpm = 60.0 / _beatTime;
                        _bpmList.Add(_bpm);

                        Debug.Log("CHANGE BPM ----------------------------------------------------------------------------------------------------------------");

                        _beatCount = 1;
                    }
                }
                else if (!WithinRange(_tempo[0], _tempBeatTime))
                {
                    _tempo[0] = _tempBeatTime;

                    if (_beatCount > 1)
                        _tempBeatCount = _beatCount;

                    _beatCount = 1;
                    //Debug.Log("CHANGE");
                }
                else
                {
                    _tempo[_beatCount] = _tempBeatTime;
                    ++_beatCount;

                    //Debug.Log(_beatCount);
                    //Debug.Log("CONTINUE");

                    if (_beatCount >= 4)
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            _beatTime += _tempo[i];
                        }
                        _beatTime /= 4;

                        _bpm = 60.0 / _beatTime;
                        _bpmList.Add(_bpm);
                        print(_bpm);

                        Debug.Log("CHANGE BPM ----------------------------------------------------------------------------------------------------------------");

                        _beatCount = 1;
                    }
                }
            }
        }
        else
        {
            if (_beated)
            {
                _beated = false;
            }
        }
    }

    //private float SumChannels(float[] channel1, float[] channel2)
    //{
    //    float e = 0;

    //    for (int i = 0; i < channel1.Length; ++i)
    //    {
    //        e += ((channel1[i] * channel1[i]) + (channel2[i] * channel2[i]));
    //    }

    //    return e;
    //}

    // Formula (R1)
    private float SumStereo(float[] channel)
    {
        // Energy
        float e = 0;

        for (int i = 0; i < channel.Length; ++i)
        {
            float toSquare = channel[i];
            e += (toSquare * toSquare);
        }

        return e;
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
    private float SumVariance(float[] historyBuffer)
    {
        // sum of variance
        float sv = 0;

        for (int i = 0; i < historyBuffer.Length; ++i)
        {
            float toSquare = historyBuffer[i] - _averageEnergy;
            sv += (toSquare * toSquare);
        }

        return sv;
    }

    private bool WithinRange(double rangeA, double rangeB)
    {
        double acceptableRange = 0.25;

        if ((rangeA + acceptableRange) < rangeB)
        {
            return false;
        }
        else if ((rangeA - acceptableRange) > rangeB)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
