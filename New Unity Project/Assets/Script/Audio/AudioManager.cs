using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    private AudioSource _audioSource;
    public static AudioManager instance = null;
    public GameObject audio;
    
    float startVolume;
    public float Volume { get { return startVolume; }}
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(audio);
        }
    }

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        startVolume = PlayerPrefs.GetFloat("Volume");
        _audioSource.volume = startVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MusicSelection" || SceneManager.GetActiveScene().name == "MainGame 1" || SceneManager.GetActiveScene().name == "GameOver")
        {
            _audioSource.Pause();
        }
        else if (!_audioSource.isPlaying)
        {
            _audioSource.UnPause();
        }  
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("Volume",startVolume);
    }

    public void UpdateVolume(float value)
    {
        startVolume = value;
        _audioSource.volume = startVolume;
    }
}
