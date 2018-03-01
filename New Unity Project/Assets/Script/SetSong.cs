using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSong : MonoBehaviour
{
    AudioLoader _al;

	// Use this for initialization
	void Start ()
    {
        _al = GetComponent<AudioLoader>();
        _al.LoadSong(PlayerPrefs.GetString("Song"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
