using System;
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

    public static List<float[]> _highList = new List<float []>();

    // HighsSection
    public static int _highSection;

    // HighsBeat
    private float _highBeat;

    // or every few seconds?
    private double _highTime;

    // Check if detection is done
    private bool _detectedFinish = false;

    // Check if beats are saved
    private bool _isbeatSaved = false;

    // After recognised note, make next note harder to detect as a beat, only if its higher
    private float[] _offset = new float[5];

    [SerializeField]
    public MusicProjectile _mp;

    [SerializeField]
    public BpmAnalyzer _ba;

    public static Dictionary<string, int> _songInfo = new Dictionary<string, int>();

    //[SerializeField]
    //AudioPeerManager _apm; // korean apm :worry:

    // Use this for initialization
    void Start ()
    {
        //_source = gameObject.GetComponent<AudioSource>();

        _highList.Add(new float[64]);
        _highSection = 0;
        _highBeat = 0.0f;

        for (int i = 0; i < 64; ++i)
        {
            _freqbeat[i] = false;
            _historyBufferArray[i] = new float[43];
            //_highs[i] = new float[21600];
            // who the fuck puts a song thats 24 hours long? ok
            // this is enough.
        }

        for (int i = 0; i < _offset.Length; ++i)
        {
            _offset[i] = 1.0f;
        }

        //print(_source.clip.name.ToString());

        // _detectedFinish = checkSong(_apm._sample.name) && checkSong(_apm._sample2.name);
        _detectedFinish = checkSong(_source.clip.name);

        _isbeatSaved = _mp.checkSong();
            //(Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
            //{
            //    return _data.Contains("<FreqName>" + _source.clip.name);
            //}
            //));

        print(_isbeatSaved);

        _highBeat = 60.0f / _ba.GetBpm();

        //if (!_detectedFinish)
        //{
        //    _songInfo.Add(_source.clip.name.ToString(), _ba.GetBpm());
        //}

        // this being last is essential
        _source.Play();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!_source.isPlaying)
        {
            if (!_isbeatSaved && _detectedFinish)
            {
                _isbeatSaved = true;

                _mp.saveSong();
                Destroy(_mp);
            }

            if (!_detectedFinish)
            {
                _detectedFinish = true;

                if (Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
                {
                    return !_data.Contains(_source.clip.name.ToString());
                }))
                {
                    print("saving");

                    List<string> saver = new List<string>();

                    saver.Add("<name>" + _source.clip.name.ToString());
                    saver.Add("</name>");

                    saver.Add("<bpm>");
                    saver.Add(_ba.GetBpm().ToString());
                    saver.Add("</bpm>");

                    saver.Add("<highcount>");
                    saver.Add((_highList.Count - 1).ToString());
                    saver.Add("</highcount>");

                    saver.Add("<highs>");
                    for (int i = 0; i < _highList.Count; ++i)
                    {
                        saver.Add("<c" + i.ToString() + ">");
                        for (int k = 0; k < _highList[i].Length; ++k)
                        {
                            saver.Add("<v" + k.ToString() + ">");
                            saver.Add(_highList[i][k].ToString());
                            saver.Add("<v/" + k.ToString() + ">");
                        }
                        saver.Add("<c/" + i.ToString() + ">");
                    }
                    saver.Add("</highs>");

                    saver.Add("<freqHighs>");
                    for (int i = 0; i < AudioPeer._freqBandHighest64.Length; ++i)
                    {
                        saver.Add("<f" + i.ToString() + ">");
                        saver.Add(AudioPeer._freqBandHighest64[i].ToString());
                        saver.Add("<f/" + i.ToString() + ">");
                    }
                    saver.Add("</freqHighs>");

                    saver.Add(_source.clip.name.ToString() + "end\n");

                    Saving.SaveToFile("MusicData.txt", saver);
                }

                Debug.Log("DONE DETECTING FREQ -----------------------------------------------------------");
            }

            _source.Play();

            //_highBeat = 0;
            _highSection = 0;
            _highTime = 0.0;
        }

        if (_detectedFinish)
        {
            //print(AudioPeer._audioBandBuffer64[3]);

            //_source.pitch = 1;
            //Time.timeScale = 1;

            SpawnEffect._spawnBass = false;
            SpawnEffect._spawnKick = false;
            SpawnEffect._spawnCenter = false;
            SpawnEffect._spawnMelody = false;
            SpawnEffect._spawnHigh = false;

            for (int i = 0; i < 64; ++i)
            {
                //if (_freqbeat[i])

                if (AudioPeer._audioBandBuffer64[i] > _highList[_highSection][i]) //audiobandbuffer64
                {
                    //print("hit");
                    //print(_highList[_highSection][i]);

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
                    else if (!SpawnEffect._spawnBass && i < 4) // bass is 0-3 but
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
        if (_highTime > (_highBeat * 8))
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
                        //if (_highList[_highSection][i] > 0.4f)
                        //    _highList[_highSection][i] *= 0.75f; // 0.75
                        //else
                            _highList[_highSection][i] *= 0.97f;
                    }
                    else if (i > 6)
                    {
                        //if (_highList[_highSection][i] > 0.4f)
                        //    _highList[_highSection][i] *= 0.7f; // 0.7
                        //else
                            _highList[_highSection][i] *= 0.97f;
                    }
                    else if (i > 3)
                    {
                        //if (_highList[_highSection][i] > 0.4f)
                        //    _highList[_highSection][i] *= 0.55f; // 0.55
                        //else
                            _highList[_highSection][i] *= 0.65f;
                    }
                    else
                    {
                        //if (_highList[_highSection][i] > 0.4f)
                        //    _highList[_highSection][i] *= 0.65f; // 0.65
                        //else
                            _highList[_highSection][i] *= 0.45f;
                    }

                    //print(AudioPeer._audioBandBuffer64[i]);
                    //print(_highList[_highSection][i]);
                }

                //print("reduce highs");
            }

            _highTime = 0.0;
            _highList.Add(new float[64]);
            ++_highSection;
        }
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
            //if (_highs[i][_highSection] < _energy[i])
            //{
            //    _highs[i][_highSection] = _energy[i];
            //}

            if (_highList[_highSection][i] < _energy[i])
            {
                _highList[_highSection][i] = _energy[i];
            }
        }
    }

    private bool checkSong(string _name)
    {
        if (Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
        {
            // See if song exists
            if (_data.Contains("<name>" + _name))
            {
                print("Song Data found, reading now...");
            }
            else
            {
                print("could not find song:" + _name);
                return false;
            }

            List<string> songData = new List<string>();
            songData = _data.GetRange(_data.IndexOf("<name>" + _name), (_data.IndexOf(_name + "end") - _data.IndexOf("<name>" + _name)));

            //why the fuck did i make this function
            //I might be mentally handicapped
            //// FIND NAME
            //List<string> nameData = new List<string>();
            //nameData = songData.GetRange(songData.IndexOf("<name>" + _name), (songData.IndexOf(_name + "</name>") - songData.IndexOf("<name>" + _name)));
            //nameData.Remove("<name>");
            //string nameString = "";
            //foreach (string num in songData)
            //    nameString += num;

            // FIND BPM
            List<string> bpmData = new List<string>();
            bpmData = songData.GetRange(songData.IndexOf("<bpm>"), (songData.IndexOf("</bpm>") - songData.IndexOf("<bpm>")));
            bpmData.Remove("<bpm>");
            string bpmString = "";
            foreach (string num in bpmData)
                bpmString += num;

            _songInfo.Add(_name, Convert.ToInt32(bpmString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));

            _ba.SetBpm(Convert.ToInt32(bpmString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));

            // FIND NUMBER OF SEGMENTS
            List<string> highsegmentData = new List<string>();
            highsegmentData = songData.GetRange(songData.IndexOf("<highcount>"), (songData.IndexOf("</highcount>") - songData.IndexOf("<highcount>")));
            highsegmentData.Remove("<highcount>");
            string highsegmentString = "";
            foreach (string num in highsegmentData)
                highsegmentString += num;

            for (int i = 0; i < (Convert.ToInt32(highsegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
                _highList.Add(new float[64]);
            //print(_highList.Count);

            // Define out where the highs value will be found
            List<string> highsectionData = songData.GetRange(songData.IndexOf("<highs>"), (songData.IndexOf("</highs>") - songData.IndexOf("<highs>")));
            highsectionData.Remove("<highs>");

            // FIND HIGHS
            for (int i = 0; i < _highList.Count; ++i)
            {
                // Yeah okay this is getting a *bit* convoluted
                // In the high section (from <high> to </high>
                // find each segment of high data (0 to 63)
                List<string> highsectionhighsegmentData = highsectionData.GetRange(highsectionData.IndexOf("<c" + i.ToString() + ">"), (highsectionData.IndexOf("<c/" + i.ToString() + ">") - highsectionData.IndexOf("<c" + i.ToString() + ">")));
                highsectionhighsegmentData.Remove("<c" + i.ToString() + ">");

                for (int k = 0; k < _highList[i].Length; ++k)
                {
                    List<string> highData = highsectionhighsegmentData.GetRange(highsectionhighsegmentData.IndexOf("<v" + k.ToString() + ">"), (highsectionhighsegmentData.IndexOf("<v/" + k.ToString() + ">") - highsectionhighsegmentData.IndexOf("<v" + k.ToString() + ">")));
                    highData.Remove("<v" + k.ToString() + ">");

                    string highString = "";
                    foreach (string num in highData)
                        highString += num;

                    // WHY IN FRESH HELL IS EVERYTHING ELSE NICELY LABELLED "ToDouble", "ToInt" BUT FLOAT IS "TO SINGLE" ?? WHY?
                    _highList[i][k] = Convert.ToSingle(highString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    //print(_highList[i][k]);
                }
            }

            // FIND FREQHIGHS
            List<string> freqHighs = songData.GetRange(songData.IndexOf("<freqHighs>"), (songData.IndexOf("</freqHighs>") - songData.IndexOf("<freqHighs>")));
            freqHighs.Remove("<freqHighs>");

            for (int i = 0; i < AudioPeer._freqBandHighest64.Length; ++i)
            {
                List<string> freqData = freqHighs.GetRange(freqHighs.IndexOf("<f" + i.ToString() + ">"), (freqHighs.IndexOf("<f/" + i.ToString() + ">") - freqHighs.IndexOf("<f" + i.ToString() + ">")));
                freqData.Remove("<f" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in freqData)
                    freqString += num;

                AudioPeer._freqBandHighest64[i] = Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            }

            return true;
        }
            ))
        {
            print("Successfully loaded from file.");
            _source.Stop();

            return true;
        }
        else
        {
            print("Song does not exist or error in reading file. Will detect.");
            return false;
        }
    }
}
