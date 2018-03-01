using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour {
    //[SerializeField]
    public AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    [SerializeField]
    BpmAnalyzer _ba;

    Dictionary<string, AudioClip> m_audioclipmap = new Dictionary<string, AudioClip>();

    // Use this for initialization
    public void Init () {
	    foreach(string s in GetComponent<UIController>().songs)
        {
            m_audioclipmap.Add(s, (Resources.Load("Audio/" + s) as AudioClip));
        }	
	}
	
	// Update is called once per frame
	void Update () {
      
    }

    public void Swap(string _name)
    {
        AudioWrapper ap = frontpeer;
        frontpeer.SetFadeOut();
        frontpeer = backpeer;
        frontpeer.SetAudioClip(m_audioclipmap[_name]);
        frontpeer.SetFadeIn();
        frontpeer.StartPlaying();
        backpeer = ap;

        _ba.ReadBpm(frontpeer.GetComponent<AudioSource>().clip.name);

        //UniBpmAnalyzer.AnalyzeBpm(frontpeer.GetComponent<AudioSource>().clip);
    }

    public void Play()
    {
        frontpeer.StartPlaying();
    }
}
