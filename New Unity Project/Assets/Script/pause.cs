using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    public bool paused;
    [SerializeField]
    AudioPeerManager _peerMan;
    // Use this for initialization
    void Start()
    {
        paused = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Pause()
    {

        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0;
            _peerMan.Pause();
        }
        else if (!paused)
        {
            Time.timeScale = 1;
            _peerMan.UnPause();

        }
    }
}
