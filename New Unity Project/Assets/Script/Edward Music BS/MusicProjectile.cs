using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProjectile : MonoBehaviour
{
    public AudioSource _audioSource;

    [SerializeField]
    AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    [SerializeField]
    AudioClip _sample;

    [SerializeField]
    GameObject Projectile;
    [SerializeField]
    GameObject ProjectileDrag;

    private bool spawnBass;
    private bool spawnKick;
    private bool spawnCenter;
    private bool spawnMelody;
    private bool spawnHigh;
    private bool spawnTwo;
    private bool spawnThree;

    // List of beats
    private List<float> bassList = new List<float>();
    private List<float> kickList = new List<float>();
    private List<float> centerList = new List<float>();
    private List<float> melodyList = new List<float>();
    private List<float> highList = new List<float>();
    private List<float> threeList = new List<float>();

    // cooldown
    private double _coolDown;


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

    // secondary BPM using bass
    private double _bpm;

    // new item
    private bool _newbpm;

    Dictionary<double, int> _bpmList = new Dictionary<double, int>();

    // Use this for initialization
    void Start()
    {
        frontpeer.SetAudioClip(_sample);
        frontpeer.StartPlaying();

        for (int i = 0; i < _tempo.Length; ++i)
        {
            _tempo[i] = 0;
        }

        _newbpm = false;

        _bpmList.Add(128, 1);
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(curr.GetClipName());

        //Vector2 randomPosition = new Vector2(15, Random.Range(-1, -5));

        //// Bass + Kick
        //if (SpawnEffect._spawnBass || SpawnEffect._spawnKick)
        //{
        //    if (!spawnBass)
        //    {
        //        Instantiate(Projectile, new Vector2(15, 0), Quaternion.identity);

        //        spawnBass = true;
        //    }
        //}
        //else
        //{
        //    spawnBass = false;
        //}

        //// Vocal + Instrument
        //if (SpawnEffect._spawnCenter || SpawnEffect._spawnMelody)
        //{
        //    if (!spawnCenter)
        //    {
        //        if (SpawnEffect._spawnHigh)
        //        {
        //            if (!spawnHigh)
        //            {
        //                Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity);

        //                spawnHigh = true;
        //            }
        //        }
        //        else
        //        {

        //            //Instantiate(Projectile, new Vector2(15, -2), Quaternion.identity);

        //            spawnCenter = true;
        //        }
        //    }
        //}
        //else
        //{
        //    spawnCenter = false;
        //}

        //// CHALKBOARD SCREECHING
        //if (SpawnEffect._spawnHigh)
        //{
        //    if (!spawnHigh)
        //    {
        //        if (SpawnEffect._spawnCenter || SpawnEffect._spawnMelody)
        //        {
        //            if (!spawnCenter)
        //            {
        //                Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity);

        //                spawnCenter = true;
        //            }
        //        }
        //        else
        //        {
        //            //Instantiate(Projectile, new Vector2(15, -4), Quaternion.identity);

        //            spawnHigh = true;
        //        }
        //    }
        //}
        //else
        //{
        //    spawnHigh = false;
        //}

        _coolDown += Time.deltaTime;
        _tempBeatTime += Time.deltaTime;

        // BASS
        if (SpawnEffect._spawnBass)
        {
            if (!spawnBass)
            {
                Instantiate(Projectile, new Vector2(15, 1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

                spawnBass = true;

                bassList.Add(_audioSource.time);
            }
            else if (spawnBass)
            {
                Instantiate(ProjectileDrag, new Vector2(15, 1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }
        }
        else
        {
            spawnBass = false;
        }

        // KICK
        if (SpawnEffect._spawnKick)
        {
            if (!spawnKick)
            {
                Instantiate(Projectile, new Vector2(15, 0), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

                spawnKick = true;

                //print(_tempBeatTime);

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

                        _newbpm = true;

                        foreach (double i in _bpmList.Keys)
                        {
                            // If its similar bpm
                            if (WithinRange(i, _bpm))
                            {
                                ++_bpmList[i];
                                _newbpm = false;
                                break;
                            }
                        }

                        if (_newbpm)
                        {
                            print("Add New");
                            _bpmList.Add(_bpm, 1);
                        }
                        _newbpm = false;

                        print(_bpm);
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

                    //print(_tempBeatTime);

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

                        _newbpm = true;

                        foreach (double i in _bpmList.Keys)
                        {
                            // If its similar bpm
                            if (WithinRange(i, _bpm))
                            {
                                ++_bpmList[i];
                                _newbpm = false;
                                break;
                            }
                        }

                        if (_newbpm)
                        {
                            print("Add New");
                            _bpmList.Add(_bpm, 1);
                        }
                        _newbpm = false;

                        print(_bpm);
                        Debug.Log("CHANGE BPM ----------------------------------------------------------------------------------------------------------------");

                        _beatCount = 1;
                    }
                }

                _tempBeatTime = 0.0;
            }
            else if (spawnKick)
            {
                Instantiate(ProjectileDrag, new Vector2(15, 0), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }
        }
        else
        {
            spawnKick = false;
        }

        // ACTUALLY ALSO PART OF VOCALS AND INSTRUMENTS
        if (SpawnEffect._spawnCenter)
        {
            //if (SpawnEffect._spawnMelody || SpawnEffect._spawnHigh)
            //{
            //    if (!spawnThree)
            //    {
            //        Instantiate(Projectile, new Vector2(15, -4), Quaternion.identity);

            //        spawnThree = true;
            //    }
            //    //else
            //    //{
            //    //    Instantiate(ProjectileDrag, new Vector2(15, -4), Quaternion.identity);
            //    //}
            //}
            if (!spawnCenter && _coolDown > 0.2)
            {
                Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

                spawnCenter = true;

                _coolDown = 0.0;
            }
            else if (spawnCenter)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }

            //if (!spawnCenter)
            //{
            //    Instantiate(Projectile, new Vector2(15, -2), Quaternion.identity);

            //    spawnCenter = true;
            //}
        }
        else
        {
            spawnCenter = false;
        }

        // VOCALS AND INSTRUMENTS
        if (SpawnEffect._spawnMelody)
        {
            //if (SpawnEffect._spawnCenter || SpawnEffect._spawnHigh)
            //{
            //    if (!spawnThree)
            //    {
            //        Instantiate(Projectile, new Vector2(15, -4), Quaternion.identity);

            //        spawnThree = true;
            //    }
            //    //else
            //    //{
            //    //    Instantiate(ProjectileDrag, new Vector2(15, -4), Quaternion.identity);
            //    //}
            //}
            if (!spawnMelody && _coolDown > 0.2)
            {
                Instantiate(Projectile, new Vector2(15, -2), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

                spawnMelody = true;

                _coolDown = 0.0;
            }
            else if (spawnMelody)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -2), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }

            //if (!spawnMelody)
            //{
            //    Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity);

            //    spawnMelody = true;
            //}
        }
        else
        {
            spawnMelody = false;
        }

        // CHALKBOARD SCREECHING
        if (SpawnEffect._spawnHigh)
        {
            //if (SpawnEffect._spawnCenter || SpawnEffect._spawnMelody)
            //{
            //    if (!spawnThree)
            //    {
            //        Instantiate(Projectile, new Vector2(15, -4), Quaternion.identity);

            //        spawnThree = true;
            //    }
            //    //else
            //    //{
            //    //    Instantiate(ProjectileDrag, new Vector2(15, -4), Quaternion.identity);
            //    //}
            //}
            if (!spawnHigh && _coolDown > 0.2)
            {
                Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

                spawnHigh = true;

                _coolDown = 0.0;
            }
            else if (spawnHigh)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -3), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }

            //if (!spawnHigh)
            //{
            //    Instantiate(Projectile, new Vector2(15, -4), Quaternion.identity);

            //    spawnHigh = true;
            //}
        }
        else
        {
            spawnHigh = false;
        }

        //if (!SpawnEffect._spawnCenter && !SpawnEffect._spawnMelody && !SpawnEffect._spawnHigh)
        //{
        //    spawnThree = false;
        //}

        double mostbpm = 0;
        int numberOf = 0;

        foreach (KeyValuePair<double, int> stats in _bpmList)
        {
            //print("enter");

            if (mostbpm == 0)
                mostbpm = stats.Key;
            if (numberOf == 0)
                numberOf = stats.Value;

            if (stats.Value > numberOf)
            {
                mostbpm = stats.Key;
                numberOf = stats.Value;
            }
        }

        //print("BPM SET TO:" + mostbpm);
        //print("VALUE:" + numberOf);
    }

    public static void Swap()
    {
    }

    public void Play()
    {
        frontpeer.StartPlaying();
    }

    //public void PrintTime()
    //{
    //    Debug.Log(_sample.time);
    //}

    //public void PrintIntro()
    //{
    //    Debug.Log("Intro");
    //}

    //public void PrintVerse()
    //{
    //    Debug.Log("Verse");
    //}

    //public void PrintBreak()
    //{
    //    Debug.Log("Break");
    //}

    //public void PrintBuildUp()
    //{
    //    Debug.Log("BuildUp");
    //}

    //public void PrintDrop()
    //{
    //    Debug.Log("Drop");
    //}

    public float TimeNow()
    {
        return frontpeer.TimeNow();
    }

    private bool WithinRange(double rangeA, double rangeB)
    {
        double acceptableRange = 0.1;

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
