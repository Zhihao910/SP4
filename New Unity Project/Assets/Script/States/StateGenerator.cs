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

        double beattime = 0.4918 * multiplier * 2;//0.5357; // 0.588

        BaseState.Attack att = () =>
        {
            Debug.Log("Hello");
            Vector2 randomPosition = new Vector2(10, Random.Range(-4, 0));
            Vector2 randomTarget = new Vector2(Random.Range(-15, 5), randomPosition.y);

            Object o = Resources.Load("Prefabs/Projectile1");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, randomPosition, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            newgo.GetComponent<Projectile>().projectileSpeed = 10 / multiplier;

        };

        //int beatcount = 0;
        for (double time = 0; time < _clip.length; time += beattime)
        {
            result.AddAttack(time, att);
            result.m_audioManager = ap;
        }

        m_StateMap[_clipname] = result;

        return result;
    }

    public BaseState CreateDropState(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clipname);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588

        //int beatcount = 0;
        BaseState.Attack att = () =>
        {
            for (int i = 0; i < 2; ++i)
            {
                Vector2 randomPosition = new Vector2(10, Random.Range(-4, 2));
                Vector2 randomTarget = new Vector2(Random.Range(-15, 5), randomPosition.y);

                Object o = Resources.Load("Prefabs/Projectile2");
                if (o == null) Debug.Log("Load failed");
                GameObject go = o as GameObject;
                if (go == null) Debug.Log("Loaded object isn't GameObject");
                GameObject newgo = Instantiate(go, randomPosition, Quaternion.identity);
                if (newgo == null) Debug.Log("Couldn't instantiate");

                newgo.GetComponent<Projectile>().projectileSpeed = 10;
            }
        };

        for (double time = 0; time < _clip.length; time += beattime)
        {
            result.AddAttack(time, att);
            result.m_audioManager = ap;
        }

        m_StateMap[_clipname] = result;

        return result;
    }

    public BaseState GetState(string _clipname)
    {
        return m_StateMap[_clipname];
    }
}
