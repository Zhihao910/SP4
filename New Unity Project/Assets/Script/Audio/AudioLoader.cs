using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{

    private static string Directory = "";
    private List<string> Songs = new List<string>();
    Dictionary<string, AudioClip> LoadedSongs = new Dictionary<string, AudioClip>();

    void Awake()
    {
#if UNITY_ANDROID
        Directory = Application.dataPath + "!assets/";
#else
        Directory = Path.Combine(Application.streamingAssetsPath, "UserMusic");
#endif
        print(Directory);
        LoadSongs();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSongs()
    {
#if UNITY_ANDROID
#else
        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Directory);
#endif
        AssetBundle ab = req.assetBundle;
        AssetBundleRequest req2 = ab.LoadAllAssetsAsync<AudioClip>();

        foreach (AudioClip ac in req2.allAssets)
            print(ac.name);
    }

    public void LoadSong(string _name)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(Directory + "/" + _name);
        LoadedSongs.Add(_name, ab.LoadAsset(_name) as AudioClip);
    }

}
