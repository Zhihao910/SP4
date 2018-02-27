using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpmAnalyzer : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip targetClip;
    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioPeerManager _apm;

    public int _bpm = 0;
    private float _bpmTime = 0.0f;

    // Dont need to call this before calling getbpm/getbeattime
    // I did it anyway
    public void FindBpm()
    {
        //if (_audioSource.clip == null && _apm == null)
        //{
        //    //_audioSource = GetComponent<SongManager>().frontpeer.GetComponent<AudioSource>();
        //}

        //_audioSource = GetComponent<SongManager>().frontpeer.GetComponent<AudioSource>();

        if (_audioSource.clip == null)
        {
            if (_apm == null)
            {
                _bpm = UniBpmAnalyzer.AnalyzeBpm(GetComponent<SongManager>().frontpeer.GetComponent<AudioSource>().clip);
            }
            else
            {
                _bpm = UniBpmAnalyzer.AnalyzeBpm(_apm._sample2);
            }

            if (_bpm < 0)
            {
                Debug.LogError("AudioClip is null.");
                return;
            }

            _bpmTime = 60.0f / (float)_bpm;

            return;
        }

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

    public void ReadBpm(string _name)
    {
        if (Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
        {
            // See if song exists
            string checker = "<name>" + _name;
            if (_data.Contains(checker as string))
            {
                print("Song Data found, reading now...");
            }
            else
            {
                print("could not find song:" + _name);

                print(checker);
                print("<name>Quintino - Go Hard (Official Music Video)");
                print(checker.Equals("<name>Quintino - Go Hard (Official Music Video)"));
                print(_data.Contains("<name>Quintino - Go Hard (Official Music Video)"));

                return false;
            }

            List<string> songData = new List<string>();
            songData = _data.GetRange(_data.IndexOf("<name>" + _name), (_data.IndexOf(_name + "end") - _data.IndexOf("<name>" + _name)));

            // FIND BPM
            List<string> bpmData = new List<string>();
            bpmData = songData.GetRange(songData.IndexOf("<bpm>"), (songData.IndexOf("</bpm>") - songData.IndexOf("<bpm>")));
            bpmData.Remove("<bpm>");
            string bpmString = "";
            foreach (string num in bpmData)
                bpmString += num;

            this.SetBpm(Convert.ToInt32(bpmString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));

            return true;
        }
    ))
        {
            print("Successfully loaded from file.");

            return;
        }
        else
        {
            print("Song does not exist or error in reading file. Will detect.");
            return;
        }
    }
}
