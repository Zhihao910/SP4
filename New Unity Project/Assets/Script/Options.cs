using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioSource audio;
    public Slider audioSlider;

    // Use this for initialization
    void Start()
    {
        audioSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(audio.volume = audioSlider.value);
        SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", audio.volume);
    }

}
