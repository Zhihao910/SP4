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

    // Use this for initialization
    void Start()
    {
        frontpeer.SetAudioClip(_sample);
        frontpeer.StartPlaying();
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

        // BASS
        if (SpawnEffect._spawnBass)
        {
            if (!spawnBass)
            {
                Instantiate(Projectile, new Vector2(15, 1), Quaternion.identity);

                spawnBass = true;

                bassList.Add(_audioSource.time);
            }
            else if (spawnBass)
            {
                Instantiate(ProjectileDrag, new Vector2(15, 1), Quaternion.identity);
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
                Instantiate(Projectile, new Vector2(15, 0), Quaternion.identity);

                spawnKick = true;
            }
            else if (spawnKick)
            {
                Instantiate(ProjectileDrag, new Vector2(15, 0), Quaternion.identity);
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
                Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity);

                spawnCenter = true;

                _coolDown = 0.0;
            }
            else if (spawnCenter)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity);
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
                Instantiate(Projectile, new Vector2(15, -2), Quaternion.identity);

                spawnMelody = true;

                _coolDown = 0.0;
            }
            else if (spawnMelody)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -2), Quaternion.identity);
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
                Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity);

                spawnHigh = true;

                _coolDown = 0.0;
            }
            else if (spawnHigh)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -3), Quaternion.identity);
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
}
