using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public List<string> songs = new List<string>();
    AudioSource _source;
    [SerializeField]
    SongManager _songmanager;
    UnityEngine.UI.Text SongName;
    UnityEngine.UI.Text BPM;

    int index = 0;
    bool leftBtn, rightBtn, playBtn = false;

    void Awake()
    {
        SongName = GameObject.FindGameObjectWithTag("SongName").GetComponent<UnityEngine.UI.Text>();
        BPM = GameObject.FindGameObjectWithTag("BPM").GetComponent<UnityEngine.UI.Text>();

        _source = GetComponent<AudioSource>();
        Saving.LoadingFromFile("StreamingAssets/Songs.txt", (List<string> _data) =>
        {
            foreach (string s in _data)
            {
                print(s);
                songs.Add(s);
            }
            return true;
        });
        _songmanager.Init();
    }

    // Use this for initialization
    void Start()
    {
        _songmanager.Swap(songs[index]);
        _songmanager.Play();
        SongName.text = songs[index];
        GetComponent<BpmAnalyzer>().ReadBpm(songs[index]);
        BPM.text = "Speed: " + GetComponent<BpmAnalyzer>()._bpm.ToString();
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
            if (--index < 0)
                index = songs.Count - 1;
            //send song to gamescene
            _songmanager.Swap(songs[index]);
            GetComponent<BpmAnalyzer>().ReadBpm(songs[index]);
            SongName.text = songs[index];
            BPM.text = "Speed: " + GetComponent<BpmAnalyzer>()._bpm.ToString();
            leftBtn = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || rightBtn)
        {
            if (++index >= songs.Count)
                index = 0;
            _songmanager.Swap(songs[index]);
            GetComponent<BpmAnalyzer>().ReadBpm(songs[index]);
            SongName.text = songs[index];
            BPM.text = "Speed: " + GetComponent<BpmAnalyzer>()._bpm.ToString();
            rightBtn = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) || playBtn)
        {
            PlayerPrefs.SetString("Song", songs[index]);
            SceneManager.LoadScene("MainGame 1");
            playBtn = false;
        }
    }
#endif
}
