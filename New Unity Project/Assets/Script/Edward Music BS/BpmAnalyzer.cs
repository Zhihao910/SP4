using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpmAnalyzer : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip targetClip;
    public AudioSource _audioSource;

    private int _bpm = 0;
    private float _bpmTime = 0.0f;

    // Dont need to call this before calling getbpm/getbeattime
    // I did it anyway
    public void FindBpm()
    {
        _bpm = UniBpmAnalyzer.AnalyzeBpm(_audioSource.clip);

        if (_bpm < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        }

        _bpmTime = 60.0f / (float)_bpm;
    }
    
    public void SetBpm(int _b)
    {
        _bpm = _b;
        _bpmTime = 60.0f / (float)_bpm;
    }

    public int GetBpm()
    {
        if (_bpm == 0)
        {
            FindBpm();
        }

        return _bpm;
    }

    public float GetBeatTime()
    {
        if (_bpmTime == 0.0f)
        {
            FindBpm();
        }

        return _bpmTime;
    }
}
