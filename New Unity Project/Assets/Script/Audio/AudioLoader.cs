using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
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
        
        LoadSongs();
    }
    // Use this for initialization
    void Start()
    {
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



}
