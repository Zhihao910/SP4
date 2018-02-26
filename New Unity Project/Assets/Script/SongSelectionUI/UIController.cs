using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
    public List<string> songs = new List<string>();
    AudioSource _source;
    [SerializeField]
    SongManager _songmanager;
    int index = 0;
    void Awake()
    {
        _source = GetComponent<AudioSource>();
        Saving.LoadingFromFile("Assets/Info/Songs.txt", (List<string> _data) =>
        {
            foreach (string s in _data)
            {
                songs.Add(s);
            }
            return true;
        });

    }

    // Use this for initialization
    void Start () {
        _songmanager.Swap(songs[index]);
        _songmanager.Play();
    }
	
	// Update is called once per frame
	void Update () {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        print(index);
        if ((Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            if (--index < 0)
                index = songs.Count - 1;
            //send song to gamescene
            _songmanager.Swap(songs[index]);
        }
        else if ((Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            if (++index > songs.Count)
                index = 0;
            _songmanager.Swap(songs[index]);
        }
        else if((Input.GetKeyDown(KeyCode.Space)))
        {
            PlayerPrefs.SetString("Song", songs[index]);
            SceneManager.LoadScene("MainGame 1");
        }
#elif UNITY_ANDROID


#endif

    }
}
