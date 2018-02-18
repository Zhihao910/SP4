﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edwardstuff : MonoBehaviour {

    [SerializeField]
    AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    [SerializeField]
    AudioClip _sample;

    [SerializeField]
    GameObject Projectile;

    private bool spawnBass;
    private bool spawnKick;
    private bool spawnMelody;
    private bool spawnCenter;
    private bool spawnHigh;

    // Use this for initialization
    void Start()
    {
        frontpeer.SetAudioClip(_sample);
        frontpeer.StartPlaying();

        spawnBass = false;
        spawnKick = false;
        spawnMelody = false;
        spawnCenter = false;
        spawnHigh = false;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(curr.GetClipName());

        //Vector2 randomPosition = new Vector2(15, Random.Range(-1, -5));

        // BASS
        if (SpawnEffect._spawnBass)
        {
            if (!spawnBass)
            {
                Instantiate(Projectile, new Vector2(15, 0), Quaternion.identity);

                spawnBass = true;
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
                Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity);

                spawnKick = true;
            }
        }
        else
        {
            spawnKick = false;
        }

        // ACTUALLY ALSO PART OF VOCALS AND INSTRUMENTS
        if (SpawnEffect._spawnCenter)
        {
            if (!spawnCenter)
            {
                Instantiate(Projectile, new Vector2(15, -2), Quaternion.identity);

                spawnCenter = true;
            }
        }
        else
        {
            spawnCenter = false;
        }

        // VOCALS AND INSTRUMENTS
        if (SpawnEffect._spawnMelody)
        {
            if (!spawnMelody)
            {
                Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity);

                spawnMelody = true;
            }
        }
        else
        {
            spawnMelody = false;
        }

        // CHALKBOARD SCREECHING
        if (SpawnEffect._spawnHigh)
        {
            if (!spawnHigh)
            {
                Instantiate(Projectile, new Vector2(15, -4), Quaternion.identity);

                spawnHigh = true;
            }
        }
        else
        {
            spawnHigh = false;
        }
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
