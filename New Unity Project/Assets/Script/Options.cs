using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider audioSlider;

    // Use this for initialization
    void Start()
    {
        audioSlider.value = AudioManager.instance.Volume;
    }

    // Update is called once per frame
    void Update()
    {
        AudioManager.instance.UpdateVolume( audioSlider.value);
    }
    
}
