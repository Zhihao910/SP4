using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPeerManager : MonoBehaviour {

    [SerializeField]
    AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    [SerializeField]
    AudioClip _sample;
    [SerializeField]
    AudioClip _sample2;

    [SerializeField]
    StateGenerator _stateGenerator;

    static bool swap = false;
    Queue<BaseState> m_playqueue = new Queue<BaseState>();
    //Queue<string> m_playqueue2 = new Queue<string>();

    Dictionary<string, AudioClip> m_audioclipmap = new Dictionary<string, AudioClip>();

    BaseState curr;

    // Use this for initialization
    void Start()
    {
        //m_audioclipmap.Add("Intro", AudioSplitter.SplitAudio(0, 12, _sample.clip, "Intro"));
        //m_audioclipmap.Add("Verse1", AudioSplitter.SplitAudio(12, 60 - 12, _sample.clip, "Verse1"));
        //m_audioclipmap.Add("BuildUp1", AudioSplitter.SplitAudio(60, 84 - 60, _sample.clip, "BuildUp1"));
        //m_audioclipmap.Add("Drop1", AudioSplitter.SplitAudio(84, 108 - 84, _sample.clip, "Drop1"));
        //m_audioclipmap.Add("Verse2", AudioSplitter.SplitAudio(108, 132 - 108, _sample.clip, "Verse2"));
        //m_audioclipmap.Add("Break1", AudioSplitter.SplitAudio(132, 156 - 132, _sample.clip, "Break1"));
        //m_audioclipmap.Add("Verse3", AudioSplitter.SplitAudio(156, 180 - 156, _sample.clip, "Verse3"));
        //m_audioclipmap.Add("BuildUp2", AudioSplitter.SplitAudio(180, 204 - 180, _sample.clip, "BuildUp2"));
        //m_audioclipmap.Add("Drop2", AudioSplitter.SplitAudio(204, 252 - 204, _sample.clip, "Drop2"));

        //m_playqueue2.Enqueue("Intro");
        //m_playqueue2.Enqueue("Verse1");

        m_audioclipmap.Add("Song", _sample);
        m_playqueue.Enqueue(_stateGenerator.CreateBaseState("Song", _sample));
        Debug.Log(1);

        curr = m_playqueue.Dequeue();
        Debug.Log(curr.GetClipName());
        frontpeer.SetAudioClip(m_audioclipmap[curr.GetClipName()]);
        frontpeer.StartPlaying();
        curr.Run();

        //m_audioclipmap.Add("Intro1", AudioSplitter.SplitAudio(5.8f, 20.7f - 5.8f, _sample.clip, "Intro1"));
        //m_audioclipmap.Add("Break1", AudioSplitter.SplitAudio(20.7f, 35.6f - 20.7f, _sample.clip, "Break1"));
        //m_audioclipmap.Add("Verse1", AudioSplitter.SplitAudio(35.6f, 50.6f - 35.6f, _sample.clip, "Verse1"));
        //m_audioclipmap.Add("BuildUp1", AudioSplitter.SplitAudio(50.6f, 73.2f - 50.6f, _sample.clip, "BuildUp1"));
        //m_audioclipmap.Add("Drop1", AudioSplitter.SplitAudio(73.2f, 103.2f - 73.2f, _sample.clip, "Drop1"));
        //m_audioclipmap.Add("Break2", AudioSplitter.SplitAudio(103.2f, 118f - 103.2f, _sample.clip, "Break2"));
        //m_audioclipmap.Add("Verse2", AudioSplitter.SplitAudio(118f, 133.5f - 118f, _sample.clip, "Verse2"));
        //m_audioclipmap.Add("BuildUp2", AudioSplitter.SplitAudio(133.5f, 148.8f - 133.5f, _sample.clip, "BuildUp2"));
        //m_audioclipmap.Add("Drop2", AudioSplitter.SplitAudio(148.8f, 178 - 148.8f, _sample.clip, "Drop2"));

        //m_audioclipmap.Add("Intro2", AudioSplitter.SplitAudio(5, 19 - 5, _sample2.clip, "Intro2"));
        //m_audioclipmap.Add("Verse3", AudioSplitter.SplitAudio(19, 35 - 19, _sample2.clip, "Verse3"));
        //m_audioclipmap.Add("BuildUp3", AudioSplitter.SplitAudio(35, 50 - 35, _sample2.clip, "BuildUp3"));
        //m_audioclipmap.Add("Drop3", AudioSplitter.SplitAudio(50, 95 - 50, _sample2.clip, "Drop3"));
        //m_audioclipmap.Add("Break3", AudioSplitter.SplitAudio(95, 102 - 95, _sample2.clip, "Break3"));
        //m_audioclipmap.Add("Verse4", AudioSplitter.SplitAudio(102, 132 - 102, _sample2.clip, "Verse4"));
        //m_audioclipmap.Add("Verse5", AudioSplitter.SplitAudio(132, 162 - 132, _sample2.clip, "Verse5"));
        //m_audioclipmap.Add("BuildUp4", AudioSplitter.SplitAudio(162, 173 - 162, _sample2.clip, "BuildUp4"));
        //m_audioclipmap.Add("Drop4", AudioSplitter.SplitAudio(173, 237 - 173, _sample2.clip, "Drop4"));

        //m_playqueue2.Enqueue("Intro1");
        //m_playqueue2.Enqueue("Break1");
        //m_playqueue2.Enqueue("Verse1");


        ////_sample.clip = Audio

        //for (int i = 0; i < 20; ++i)
        //{
        //    m_playqueue2.Enqueue("BuildUp" + Random.Range(1, 3));
        //    m_playqueue2.Enqueue("Drop" + Random.Range(1, 3));
        //    m_playqueue2.Enqueue("Break" + Random.Range(1, 3));
        //    m_playqueue2.Enqueue("Verse" + Random.Range(1, 3));
        //}
        //foreach (KeyValuePair<string, AudioClip> _ac in m_audioclipmap)
        //{
        //    m_playqueue2.Enqueue(_ac.Key);
        //}
        //frontpeer.SetAudioClip(m_audioclipmap[m_playqueue2.Dequeue()]);
        //frontpeer.StartPlaying();
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(curr.GetClipName());

        if (swap && null != m_playqueue.Peek())
        {
            curr.StopRun();
            curr = m_playqueue.Dequeue();
            curr.Run();

            AudioWrapper ap = frontpeer;
            frontpeer.SetFadeOut();
            frontpeer = backpeer;
            frontpeer.SetAudioClip(m_audioclipmap[curr.GetClipName()]);
            frontpeer.SetFadeIn();
            frontpeer.StartPlaying();
            backpeer = ap;
            swap = !swap;
        }
	}

    public static void Swap()
    {
        swap = !swap;
    }
    public void Play()
    {
        frontpeer.StartPlaying();
        curr.Run();
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
