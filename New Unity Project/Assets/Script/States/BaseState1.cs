using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState1 : BaseState {
    public enum Type
    {
        BASS_TYPE,
        KICK_TYPE,
        GENERAL_TYPE,
        TYPE_NUM,
    };

    Attack[] _attacks = new Attack[(int)(Type.TYPE_NUM)];

    private List<float>[] beatList = new List<float>[(int)(Type.TYPE_NUM)];

    void Awake()
    {
        for (int i = 0; i < (int)Type.TYPE_NUM; ++i)
            beatList[i] = new List<float>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        base.FixedUpdate();
	}

    public void AddAttack(Type _go,Attack _function)
    {
        _attacks[(int)_go] = _function;
    }

    public void PushAttacksIntoList()
    {
        for (int i = 0; i < (int)Type.TYPE_NUM; ++i)
        {
            foreach (float f in beatList[i])
            {
                m_Attacks.Add(new KeyValuePair<double, Attack>((double)f, _attacks[i]));
            }
        }
        Sort();
    }
}
