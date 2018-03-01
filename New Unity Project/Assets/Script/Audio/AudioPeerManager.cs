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
        int length = Mathf.RoundToInt(_sample.length);
        playerScore.SetWinValue(Mathf.RoundToInt(300.0f * Mathf.Pow(length, 1.3f)));
    }

    // Use this for initialization
    void Start()
    {
        foreach (AudioClip ac in AudioSplitter.SplitClip(_sample))
        {
            m_audioclipmap.Add(ac.name, ac);
            //BaseState1 bs = _stateGenerator.MultiHighState(ac.name, ac, 4.0f);
            //foreach (float f in musicprojectile.bassList)
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
            //m_playqueue.Enqueue(_stateGenerator.CreateVerticalLaserAttack(ac.name,ac,4.0f));
        }
        curr = m_playqueue.Dequeue();
        curr.Run();
        Debug.Log(m_audioclipmap[curr.GetClipName()]);
        frontpeer.SetAudioClip(m_audioclipmap[curr.GetClipName()]);
        frontpeer.StartPlaying();
    }

    bool QTE = false;
    // Update is called once per frame
    void Update()
    {
        if (PlayerController._crescendo)
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
        else if (m_playqueue.Count <= 0)
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
