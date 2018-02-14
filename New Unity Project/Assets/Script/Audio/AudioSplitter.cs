using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSplitter : MonoBehaviour {

    public static AudioClip SplitAudio(float _offset, float _seconds, AudioClip _full,int clipno = 0)
    {
        //Debug.Log(_size);
        int samples = (int)(_seconds * _full.frequency);
        AudioClip _audioclip = AudioClip.Create(_full.name + clipno, samples, _full.channels, _full.frequency, false);
        float[] _samples = new float[samples * 2];
        _full.GetData(_samples, (int)(_offset * _full.frequency));
        _audioclip.SetData(_samples, 0);
       // Debug.Log(_audioclip.length);

        return _audioclip;
    }

    public static AudioClip SplitAudio(float _offset, float _seconds, AudioClip _full, string _name = "")
    {
        //Debug.Log(_size);
        int samples = (int)(_seconds * _full.frequency);
        AudioClip _audioclip = AudioClip.Create(_name, samples, _full.channels, _full.frequency, false);
        float[] _samples = new float[samples * 2];
        _full.GetData(_samples, (int)(_offset * _full.frequency));
        _audioclip.SetData(_samples, 0);
        // Debug.Log(_audioclip.length);

        return _audioclip;
    }
}
