﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public List<string> songs = new List<string>();
    public List<string> usersongs = new List<string>();

    AudioSource _source;
    [SerializeField]
    SongManager _songmanager;
    UnityEngine.UI.Text SongName;
    UnityEngine.UI.Text BPM;

    int index = 0;
    int index2 = 0;
    bool user = false;
    bool leftBtn, rightBtn, playBtn = false;

    void Awake()
    {
        SongName = GameObject.FindGameObjectWithTag("SongName").GetComponent<UnityEngine.UI.Text>();
        BPM = GameObject.FindGameObjectWithTag("BPM").GetComponent<UnityEngine.UI.Text>();

        _source = GetComponent<AudioSource>();
        Saving.TEMPLoadingFromFile("Songs.txt", (List<string> _data) =>
        {
            foreach (string s in _data)
            {
                print(s);
                songs.Add(s);
            }
            return true;
        });

        foreach (string s in System.IO.Directory.GetFiles(System.IO.Path.Combine(Application.streamingAssetsPath, "UserMusic/")))
        {
            if (s.Contains(".meta"))
                continue;
            string lmoa = s.Replace(System.IO.Path.Combine(Application.streamingAssetsPath, "UserMusic/"), "");
            usersongs.Add(lmoa);
        }

        _songmanager.Init();
    }

    // Use this for initialization
    void Start()
    {
        _songmanager.Swap(songs[index]);
        _songmanager.Play();
        SongName.text = songs[index];
        BPM.text = "Time: " + _songmanager.GetTime(songs[index]);
    }

    // Update is called once per frame
    void Update()
    {
        Selection();
    }

#if UNITY_ANDROID || UNITY_STANDALONE

    public void leftSelectionBtn()
    {
        leftBtn = true;
        print("leftpressed");
    }
    public void rightSelectionBtn()
    {
        rightBtn = true;
        print("rightpressed");
    }

    public void playSelectionBtn()
    {
        playBtn = true;
        print("play");
    }

    public void Selection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || leftBtn)
        {
            if (!user)
            {
                if (--index < 0)
                    index = songs.Count - 1;
                //send song to gamescene
                _songmanager.Swap(songs[index]);
                SongName.text = songs[index];
                BPM.text = "Time: " + _songmanager.GetTime(songs[index]);

                leftBtn = false;
            }
            else
            {
                if (--index2 < 0)
                    index = usersongs.Count - 1;
                //send song to gamescene
                _songmanager.Swap(usersongs[index]);
                SongName.text = usersongs[index];
                BPM.text = "Time: " + _songmanager.GetTime(usersongs[index]);

                leftBtn = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || rightBtn)
        {
            if (!user)
            {
                if (++index >= songs.Count)
                    index = 0;
                _songmanager.Swap(songs[index]);
                SongName.text = songs[index];
                BPM.text = "Time: " + _songmanager.GetTime(songs[index]);

                rightBtn = false;
            }
            else
            {
                if (++index2 >= usersongs.Count)
                    index = 0;
                //send song to gamescene
                _songmanager.Swap(usersongs[index]);
                SongName.text = usersongs[index];
                BPM.text = "Time: " + _songmanager.GetTime(usersongs[index]);

                leftBtn = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) || playBtn)
        {
            PlayerPrefs.SetString("Song", songs[index]);
            SceneManager.LoadScene("MainGame 1");
            playBtn = false;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            user = !user;
            if (!user)
            {
                //send song to gamescene
                _songmanager.Swap(songs[index]);
                SongName.text = songs[index];
                BPM.text = "Time: " + _songmanager.GetTime(usersongs[index]);

                leftBtn = false;
            }
            else
            {
                //send song to gamescene
                _songmanager.Swap(usersongs[index]);
                SongName.text = usersongs[index];
                BPM.text = "Time: " + _songmanager.GetTime(usersongs[index]);

                leftBtn = false;
            }
        }
    }
#endif
}
