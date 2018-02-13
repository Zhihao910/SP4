using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSplitter : MonoBehaviour {

    public static AudioClip SplitAudio(int _offset, int _size, AudioClip _full)
    {
        AudioClip _audioclip = AudioClip.Create(_full.name + "1", _size, _full.channels, _full.frequency, false);
        float[] _samples = new float[_size];
        _full.GetData(_samples, _offset);
        _audioclip.SetData(_samples, 0);

        return _audioclip;
    }
}
