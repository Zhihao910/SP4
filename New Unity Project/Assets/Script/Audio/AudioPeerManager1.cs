using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edwardstuff : MonoBehaviour {

    [SerializeField]
    AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    [SerializeField]
    AudioClip _sample;

    // Use this for initialization
    void Start()
    {
        frontpeer.SetAudioClip(_sample);
        frontpeer.StartPlaying();
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(curr.GetClipName());
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
