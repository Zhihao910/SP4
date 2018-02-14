using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGenerator : MonoBehaviour {

    static Dictionary<string, BaseState> m_StateMap = new Dictionary<string, BaseState>();

    [SerializeField]
    AudioPeerManager ap;
   

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public BaseState CreateBaseState(string _clipname, AudioClip _clip)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clipname);
        //Run adding attacks here

        double beattime = 0.588; //0.5357

        int beatcount = 0;
        for (double time = 0; time < _clip.length; time += beattime)
        {
            beatcount++;
            result.AddAttack(time);
            result.m_audioManager = ap;
        }

        Debug.Log(result.GetClipName());

        m_StateMap[_clipname] = result;

        return result;
    }
}
