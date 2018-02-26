using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]
sealed public class AudioPeer : MonoBehaviour
{
    public AudioSource _audioSource;
    public static float[] _samples = new float[512]; // originally 512

    private float[] _freqBand = new float[8];

    private float[] _freqBand64 = new float[64];
    private float[] _bandBuffer64 = new float[64];
    float[] _bufferDecrease64 = new float[64];
    public static float[] _freqBandHighest64 = new float[64];

    [HideInInspector]
    public static float[] _audioBand64 = new float[64];
    public static float[] _audioBandBuffer64;

	// Use this for initialization
	private void Start ()
    {
        Application.runInBackground = true;

        _audioBand64 = new float[64];
        _audioBandBuffer64 = new float[64];

        //_audioSource = GetComponent<AudioSource>();

        //_audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate ()
    {
        _audioSource = GetComponent<AudioPeerManager>().frontpeer.GetComponent<AudioSource>();

        GetSpectrumAudioSource();
        //MakeFrequencyBands();
        MakeFrequencyBands64();
        BandBuffer64();
        CreateAudioBands64();
    }

    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands()
    {
        /*
         * 20 - 60 hz       Sub-Bass
         * 60 - 250 hz      Bass
         * 250 - 500 hz     Low Midrange
         * 500 - 2000 hz    Midrange
         * 2000 - 4000 hz   Upper Midrange
         * 4000 - 6000 hz   Presence
         * 6000 - 20000 hz  Brilliance
         * 
         * 44100 hz / 512s = 86 hz per sample
         * 0th - 2s   = 86 hz      - 0     - 86 hz
         * 1st - 4s   = 172 hz     - 87    - 258 hz
         * 2nd - 8s   = 344 hz     - 259   - 602 hz
         * 3rd - 16s   = 688 hz    - 603   - 1290 hz
         * 4th - 32s  = 1376 hz    - 1921  - 2666 hz
         * 5th - 64s  = 2752 hz    - 2667  - 5418 hz
         * 6th - 128s  = 5504 hz   - 5419  - 10922 hz
         * 7th - 256s = 11008 hz   - 10923 - 21930 hz
         * 510
         */

        int count = 0;

        //Create 8 bands of frequencies.
        for (int i = 1; i <= 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i);

            if (i == 8) sampleCount += 2;
            
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                //average += _samples[count];
                count++;
            }

            average /= count;

            _freqBand[i-1] = average * 10;
        }
    }

    private void MakeFrequencyBands64()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; ++i)
        {
            float average = 0;

            if (i > 0 && i % 16 == 0)
            {
                ++power;
                sampleCount = (int)Mathf.Pow(2, power);

                if (power == 3)
                {
                    sampleCount -= 2;
                }
            }

            for (int j = 0; j < sampleCount; ++j)
            {
                average += _samples[count] * (count + 1);

                ++count;
            }

            average /= count;

            _freqBand64[i] = average * 80;
        }
    }

    void BandBuffer64()
    {
        for (int i = 0; i < 64; ++i)
        {
            if (_freqBand64[i] > _bandBuffer64[i])
            {
                _bandBuffer64[i] = _freqBand64[i];
                _bufferDecrease64[i] = 0.05f;
            }
            else if (_freqBand64[i] < _bandBuffer64[i])
            {
                _bandBuffer64[i] -= _bufferDecrease64[i];

                if (i > 18)
                {
                    _bufferDecrease64[i] *= 2.4f;
                }
                else if (i > 6)
                {
                    _bufferDecrease64[i] *= 1.2f;
                }
                else if (i > 3)
                {
                    _bufferDecrease64[i] *= 4.8f;
                }
                else
                {
                    _bufferDecrease64[i] *= 4.8f;
                }

                //_bufferDecrease64[i] *= 2.4f; // 3.6

                if (_bandBuffer64[i] < 0)
                {
                    _bandBuffer64[i] = 0.0f;
                }
            }
        }
    }

    void CreateAudioBands64()
    {
        for (int i = 0; i < 64; ++i)
        {
            if (_freqBand64[i] > _freqBandHighest64[i])
            {
                _freqBandHighest64[i] = _freqBand64[i];
            }

            _audioBand64[i] = (_freqBand64[i] / _freqBandHighest64[i]);
            _audioBandBuffer64[i] = (_bandBuffer64[i] / _freqBandHighest64[i]);
        }
    }
}
