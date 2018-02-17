using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGenerator : MonoBehaviour {

    static Dictionary<string, BaseState> m_StateMap = new Dictionary<string, BaseState>();

    [SerializeField]
    AudioPeerManager ap;

    [SerializeField]
    GameObject p;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public BaseState CreateBaseState(string _clipname, AudioClip _clip,float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clipname);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588

        int beatcount = 0;
        for (double time = 0; time < _clip.length; time += beattime)
        {
            result.name = _clipname;
            result.AddAttack(time);
            result.m_audioManager = ap;
            result._go = p;
        }

        Debug.Log(result.GetClipName());

        m_StateMap[_clipname] = result;

        return result;
    }

    public BaseState GetState(string _clipname)
    {
        return m_StateMap[_clipname];
    }
}
