using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState1 : BaseState {
    enum Type
    {
        BASS_TYPE,
        KICK_TYPE,
        GENERAL_TYPE,
        TYPE_NUM,
    };

    public AudioPeerManager m_audioManager;

    Attack[] _attacks = new Attack[(int)(Type.TYPE_NUM)];

    [SerializeField]
    string m_clipname;

    bool m_Run = false;

    private List<float> bassList = new List<float>();
    private List<float> kickList = new List<float>();
    private List<float> threeList = new List<float>();

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        foreach(Attack a in _attacks)
        {
            //Condition
            //if()
            //a();
        }
	}

    public void AddAttack(int _go,Attack _function)
    {
        _attacks[_go] = _function;
    }


    public string GetClipName()
    {
        return m_clipname;
    }

    public void SetClipName(string _clipname)
    {
        m_clipname = _clipname;
    }

    public void StopRun()
    {
        m_Run = false;
    }
}
