using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour {
    [SerializeField]
    AudioWrapper frontpeer;
    [SerializeField]
    AudioWrapper backpeer;

    Dictionary<string, AudioClip> m_audioclipmap = new Dictionary<string, AudioClip>();

    // Use this for initialization
    void Awake () {
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
    }

    public void Play()
    {
        frontpeer.StartPlaying();
    }
}
