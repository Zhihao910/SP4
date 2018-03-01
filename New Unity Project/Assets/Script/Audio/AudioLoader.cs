using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    [SerializeField]
    bool LoadOnStart = true;
    WWW songloader;
    private static string Directory = "";
    private List<string> Songs = new List<string>();
    Dictionary<string, AudioClip> LoadedSongs = new Dictionary<string, AudioClip>();
    void Awake()
    {
#if UNITY_ANDROID
        Directory = "jar:file:///" + Application.dataPath + "/!assets/";
#else
        Directory = "file:///" + Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "UserMusic" + Path.AltDirectorySeparatorChar ;
#endif
        if(LoadOnStart)
            LoadSong();
    }
    // Use this for initialization
    void Start()
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSongs()
    {
#if UNITY_ANDROID
#else
        foreach (string s in System.IO.Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "UserMusic/")))
        {
            if (s.Contains(".meta"))
                continue;
            songloader = new WWW("file:///" + s);
            while(songloader.progress != 1)
            {

            }
            LoadedSongs.Add(s, songloader.GetAudioClip(true, true));
            print(songloader.error);
            GetComponent<AudioSource>().clip = LoadedSongs[s];
            LoadedSongs[s].name = s;
        }
#endif
    }

    public void LoadSong()
    {
        print(PlayerPrefs.GetString("Song"));
        songloader = new WWW("file:///" + PlayerPrefs.GetString("Song"));
        while (songloader.progress != 1)
        {
        }
        GetComponent<AudioSource>().clip = songloader.GetAudioClip(true, false);
    }

    public AudioClip LoadSong(string _name)
    {
        songloader = new WWW("file:///"+Application.streamingAssetsPath + "/UserMusic/" + _name);
        while (songloader.progress != 1)
        {
        }
        AudioClip ac = songloader.GetAudioClip(true, false);
        ac.name = _name;

        return ac;
    }

}
