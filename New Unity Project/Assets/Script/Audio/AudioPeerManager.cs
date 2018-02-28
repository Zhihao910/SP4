using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPeerManager : MonoBehaviour
{
    public AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    [SerializeField]
    public AudioClip _sample;
    [SerializeField]
    public AudioClip _sample2;

    [SerializeField]
    StateGenerator _stateGenerator;

    MusicProjectile musicprojectile;

    static bool swap = false;
    Queue<BaseState> m_playqueue = new Queue<BaseState>();

    Dictionary<string, AudioClip> m_audioclipmap = new Dictionary<string, AudioClip>();

    public bool RemixVersion = false;

    BaseState curr;
    BaseState next;

    string song;

    private List<float> bassList = new List<float>();
    private List<float> kickList = new List<float>();
    private List<float> highList = new List<float>();

    [SerializeField]
    Score playerScore;

    void Awake()
    {
        if (_sample == null)
        {
            song = PlayerPrefs.GetString("Song");
            _sample = Resources.Load("Audio/" + song) as AudioClip;
        }
        musicprojectile = gameObject.AddComponent<MusicProjectile>();
        musicprojectile.detected = musicprojectile.ChckSongName(_sample.name);


    }

    // Use this for initialization
    void Start()
    {
        if (RemixVersion)
        {
            m_audioclipmap.Add("Intro1", AudioSplitter.SplitAudio(5.8f, 20.7f - 5.8f, _sample, "Intro1"));
            m_playqueue.Enqueue(_stateGenerator.CreateIntroState("Intro1", m_audioclipmap["Intro1"], 8));
            m_audioclipmap.Add("Break1", AudioSplitter.SplitAudio(20.7f, 35.6f - 20.7f, _sample, "Break1"));
            m_audioclipmap.Add("Verse1", AudioSplitter.SplitAudio(35.6f, 50.6f - 35.6f, _sample, "Verse1"));
            m_audioclipmap.Add("BuildUp1", AudioSplitter.SplitAudio(50.6f, 73.2f - 50.6f, _sample, "BuildUp1"));
            m_audioclipmap.Add("Drop1", AudioSplitter.SplitAudio(73.2f, 103.2f - 73.2f, _sample, "Drop1"));
            m_audioclipmap.Add("Break2", AudioSplitter.SplitAudio(103.2f, 118.2f - 103.2f, _sample, "Break2"));
            m_audioclipmap.Add("Verse2", AudioSplitter.SplitAudio(118.2f, 133.1f - 118.2f, _sample, "Verse2"));
            m_audioclipmap.Add("BuildUp2", AudioSplitter.SplitAudio(133.1f, 148.1f - 133.1f, _sample, "BuildUp2"));
            m_audioclipmap.Add("Drop2", AudioSplitter.SplitAudio(148.1f, 178.2f - 148.1f, _sample, "Drop2"));
            m_audioclipmap.Add("Break3", AudioSplitter.SplitAudio(178.2f, _sample.length - 178.2f, _sample, "Break3"));

            m_playqueue.Enqueue(_stateGenerator.CreateParryState("Break1", m_audioclipmap["Break1"], 4));
            m_playqueue.Enqueue(_stateGenerator.CreateBaseState("Verse1", m_audioclipmap["Verse1"], 3));
            _stateGenerator.CreateBaseState("BuildUp1", m_audioclipmap["BuildUp1"], 2);
            _stateGenerator.CreateDropState("Drop1", m_audioclipmap["Drop1"]);
            _stateGenerator.CreateBaseState("Break2", m_audioclipmap["Break2"], 4);
            _stateGenerator.CreateBaseState("Verse2", m_audioclipmap["Verse2"], 3);
            _stateGenerator.CreateBaseState("BuildUp2", m_audioclipmap["BuildUp2"], 2);
            _stateGenerator.CreateDropState("Drop2", m_audioclipmap["Drop2"]);
            _stateGenerator.CreateBaseState("Break3", m_audioclipmap["Break3"], 4);

            m_audioclipmap.Add("Intro2", AudioSplitter.SplitAudio(1.1f, 19.7f - 1.1f, _sample2, "Intro2"));
            m_audioclipmap.Add("Verse3", AudioSplitter.SplitAudio(19.7f, 34.8f - 19.7f, _sample2, "Verse3"));
            m_audioclipmap.Add("BuildUp3", AudioSplitter.SplitAudio(34.8f, 49.7f - 34.8f, _sample2, "BuildUp3"));
            m_audioclipmap.Add("Drop3", AudioSplitter.SplitAudio(49.7f, 94.8f - 49.7f, _sample2, "Drop3"));
            m_audioclipmap.Add("Break4", AudioSplitter.SplitAudio(94.8f, 101.5f - 94.8f, _sample2, "Break4"));
            m_audioclipmap.Add("Verse4", AudioSplitter.SplitAudio(101.5f, 162.3f - 101.5f, _sample2, "Verse4"));
            m_audioclipmap.Add("BuildUp4", AudioSplitter.SplitAudio(162.3f, 177.3f - 162.3f, _sample2, "BuildUp4"));
            m_audioclipmap.Add("Drop4", AudioSplitter.SplitAudio(177.3f, 237 - 177.3f, _sample2, "Drop4"));

            _stateGenerator.CreateBaseState("Intro2", m_audioclipmap["Intro2"], 8);
            _stateGenerator.CreateBaseState("Verse3", m_audioclipmap["Verse3"], 3);
            _stateGenerator.CreateBaseState("BuildUp3", m_audioclipmap["BuildUp3"], 2);
            _stateGenerator.CreateDropState("Drop3", m_audioclipmap["Drop3"]);
            _stateGenerator.CreateBaseState("Break4", m_audioclipmap["Break4"], 4);
            _stateGenerator.CreateBaseState("Verse4", m_audioclipmap["Verse4"], 3);
            _stateGenerator.CreateBaseState("BuildUp4", m_audioclipmap["BuildUp4"], 2);
            _stateGenerator.CreateDropState("Drop4", m_audioclipmap["Drop4"]);

            _stateGenerator.CreateParryState("BuildUp5", m_audioclipmap["BuildUp1"], 2);
            _stateGenerator.CreateParryState("Break5", m_audioclipmap["Break2"], 4);
            _stateGenerator.CreateParryState("Verse5", m_audioclipmap["Verse2"], 3);
            _stateGenerator.CreateParryState("BuildUp6", m_audioclipmap["BuildUp2"], 2);
            _stateGenerator.CreateParryState("Break6", m_audioclipmap["Break3"], 4);
            _stateGenerator.CreateParryState("Verse6", m_audioclipmap["Verse3"], 3);
            _stateGenerator.CreateParryState("BuildUp7", m_audioclipmap["BuildUp3"], 2);
            _stateGenerator.CreateParryState("Break7", m_audioclipmap["Break4"], 4);
            _stateGenerator.CreateParryState("Verse7", m_audioclipmap["Verse4"], 3);
            _stateGenerator.CreateParryState("BuildUp8", m_audioclipmap["BuildUp4"], 2);


            for (int i = 0; i < 20; ++i)
            {

                m_playqueue.Enqueue(_stateGenerator.GetState("BuildUp" + Random.Range(1, 9)));
                m_playqueue.Enqueue(_stateGenerator.GetState("Drop" + Random.Range(1, 5)));
                m_playqueue.Enqueue(_stateGenerator.GetState("Verse" + Random.Range(1, 8)));
                m_playqueue.Enqueue(_stateGenerator.GetState("Break" + Random.Range(1, 8)));
            }
        }
        else if (!RemixVersion)
        {
            float offset = 0;
            foreach (AudioClip ac in AudioSplitter.SplitClip(_sample))
            {
                m_audioclipmap.Add(ac.name, ac);
                //BaseState1 bs = _stateGenerator.MultiHighState(ac.name, ac, 4.0f);
                //foreach(float f in musicprojectile.bassList)
                //{
                //    if (f > offset + ac.length)
                //        break;
                //    else if (f < offset)
                //        continue;
                //    if (f > offset && f < offset + ac.length)
                //        bs.AddBeat(BaseState1.Type.BASS_TYPE, f - offset);
                //}

                //foreach (float f in musicprojectile.kickList)
                //{
                //    if (f > offset + ac.length)
                //        break;
                //    else if (f < offset)
                //        continue;
                //    if (f > offset && f < offset + ac.length)
                //        bs.AddBeat(BaseState1.Type.KICK_TYPE, f - offset);

                //}

                //foreach (float f in musicprojectile.highList)
                //{
                //    if (f > offset + ac.length)
                //        break;
                //    else if (f < offset)
                //        continue;
                //    if (f > offset && f < offset + ac.length)
                //        bs.AddBeat(BaseState1.Type.GENERAL_TYPE, f - offset);
                //}
                //bs.PushAttacksIntoList();
                //m_playqueue.Enqueue(bs);
                //offset += ac.length;
                m_playqueue.Enqueue(_stateGenerator.GenerateState(StateGenerator.GenerateType.NUMSTATE, ac.name, ac, 4.0f));
            }
        }
        curr = m_playqueue.Dequeue();
        curr.Run();
        Debug.Log(m_audioclipmap[curr.GetClipName()]);
        frontpeer.SetAudioClip(m_audioclipmap[curr.GetClipName()]);
        frontpeer.StartPlaying();
    }
    bool QTE = false;
    // Update is called once per frame
    void Update () {
        if(PlayerController._crescendo)
        {
            if (m_playqueue.Count > 0 && !QTE)
            {
                next = _stateGenerator.GenerateState(StateGenerator.GenerateType.QUICKTIMEEVENTSTATE, m_playqueue.Peek().GetClipName(), m_audioclipmap[m_playqueue.Peek().GetClipName()]);
                QTE = true;
            }
        }
        if (swap && m_playqueue.Count > 0)
        {
            curr.StopRun();
            curr = m_playqueue.Dequeue();
            if (QTE)
            {

                curr = next;
                QTE = false;
            }
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
        else if(m_playqueue.Count <= 0)
        {
            playerScore.SaveScore();
            SceneManager.LoadScene("GameOver");
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

    public float TimeNow()
    {
        return frontpeer.TimeNow();
    }

    public void Pause()
    {
        frontpeer.Pause();
    }

    public void UnPause()
    {
        frontpeer.UnPause();
    }
}
