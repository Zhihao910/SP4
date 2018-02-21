using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioWrapper : MonoBehaviour {
    private AudioSource _audioSource;

    bool _fadeout = false;
    bool _fadein = false;

    float startVolume;

    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();
        startVolume = _audioSource.volume;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (null != _audioSource.clip)
        {
            if (!_fadeout && _audioSource.time > _audioSource.clip.length - 0.1)
            {
                Debug.Log("SWAP");
                AudioPeerManager.Swap();
            }
        }
        if (_fadeout)
        {
            FadeOut();
        }
        else if(_fadein)
        {
            FadeIn();
        }
    }

    public void SetAudioClip(AudioClip _clip)
    {
        if(!_audioSource)
            _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip;
    }

    public void StopPlaying()
    {
        _audioSource.Stop();
    }

    public void StartPlaying()
    {
        _audioSource.Play();
    }

    public void PlayFromSavedLocation(AudioClip _ac, float _time = 0)
    {
        _audioSource.time = _time;
        _audioSource.clip = _ac;
        StartPlaying();
    }

    public void SetFadeOut()
    {
        _fadeout = !_fadeout;
    }

    void FadeOut()
    {
        //Debug.Log("FADE");

        if (_audioSource.volume > 0)
        {
            _audioSource.volume -= startVolume * Time.deltaTime / 0.1f;

            return;
        }

        _audioSource.Stop();
        _fadeout = false;
    }

    public void SetFadeIn()
    {
        _fadein = !_fadein;
    }

    void FadeIn()
    {
        //Debug.Log("FADE");

        if (_audioSource.volume < startVolume)
        {
            _audioSource.volume += startVolume * Time.deltaTime / 0.1f;
            return;
        }
        _fadein = false;
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public float TimeNow()
    {
        return _audioSource.time;
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    public void UnPause()
    {
        _audioSource.UnPause();
    }

}
