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
        Saving.LoadingFromFile("../Info/Songs.txt", (List<string> _data) =>
        {
            foreach (string s in _data)
            {
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_ANDROID

    public void leftSelectionBtn()
    {
        leftBtn = true;
    }
    public void rightSelectionBtn()
    {
        rightBtn = true;
    }

    public void playSelectionBtn()
    {
        playBtn = true;
    }
#endif

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
        else if (Input.GetKeyDown(KeyCode.RightArrow) || rightBtn)
        {
            if (++index >= songs.Count)
                index = 0;
            _songmanager.Swap(songs[index]);
            GetComponent<BpmAnalyzer>().ReadBpm(songs[index]);
            SongName.text = songs[index];
            BPM.text = "Speed: " + GetComponent<BpmAnalyzer>()._bpm.ToString();
            rightBtn = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) || playBtn)
        {
            PlayerPrefs.SetString("Song", songs[index]);
            SceneManager.LoadScene("MainGame 1");
            playBtn = false;
        }
    }
}
