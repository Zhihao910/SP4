/*
UniBpmAnalyzer
Copyright (c) 2016 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using UnityEngine;

public class UniBpmAnalyzerExample : MonoBehaviour
{
    [SerializeField]
    private AudioClip targetClip;

    public static int _bpm;

    private void Start()
    {
        _bpm = UniBpmAnalyzer.AnalyzeBpm(targetClip);
        if (_bpm < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        }
    }
}
