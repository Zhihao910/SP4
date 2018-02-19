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

    public BaseState CreateBaseState(string _clipname, AudioClip _clip,float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clipname);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588

        BaseState.Attack att = () =>
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Vector3 target = new Vector3(-1, 1, 0);
            Object o = Resources.Load("Prefabs/Projectile1");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, pos, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            newgo.GetComponent<Projectile>().SetDir(target);
            newgo.GetComponent<Projectile>().projectileSpeed = 10 / multiplier;

            for (int i = 0; i < 4; ++i)
            {
                GameObject newergo = Instantiate(newgo, pos, Quaternion.identity);
                target.x += 0.5f;
                newergo.GetComponent<Projectile>().SetDir(target);
                newergo.GetComponent<Projectile>().projectileSpeed = 10 / multiplier;
            }
        };

        BaseState.Attack att2 = () =>
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Vector3 target = new Vector3(-0.75f, 1, 0);
            Object o = Resources.Load("Prefabs/Projectile1");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, pos, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            newgo.GetComponent<Projectile>().SetDir(target);
            newgo.GetComponent<Projectile>().projectileSpeed = 10 / multiplier;

            for (int i = 0; i < 3; ++i)
            {
                GameObject newergo = Instantiate(newgo, pos, Quaternion.identity);
                target.x += 0.5f;
                newergo.GetComponent<Projectile>().SetDir(target);
                newergo.GetComponent<Projectile>().projectileSpeed = 10 / multiplier;
            }
        };

        //int beatcount = 0;
        for (double time = 0; time < _clip.length; time += beattime)
        {
            result.AddAttack(time, att);
            result.AddAttack(time + beattime/2, att2);

            result.m_audioManager = ap;
        }

        m_StateMap[_clipname] = result;
        result.Sort();
        return result;
    }

    public BaseState CreateDropState(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clipname);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588

        Vector3 leftlimit = new Vector3(-0.75f, 1, 0);
        Vector3 rightlimit = new Vector3(0.75f, 1, 0);
        float prevprev = 0;
        float prev = 0.5f;
        float mult = -1;
        BaseState.Attack att = () =>
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;

            if (prevprev != 0 && (prev == leftlimit.x || prev == rightlimit.x))
                mult = -mult;


            for (float i = -0.25f; i < 0.5f; i += 0.25f)
            {
                Object o;
                if (i == 0)
                   o = Resources.Load("Prefabs/Projectile2");
                else
                    o = Resources.Load("Prefabs/Projectile1");

                if (o == null) Debug.Log("Load failed");
                GameObject go = o as GameObject;
                if (go == null) Debug.Log("Loaded object isn't GameObject");
                GameObject newgo = Instantiate(go, pos, Quaternion.identity);
                if (newgo == null) Debug.Log("Couldn't instantiate");
                newgo.GetComponent<Projectile>().SetDir(new Vector3(prev + (i * mult), 1, 0));
                newgo.GetComponent<Projectile>().projectileSpeed = 10;
                if (i == 0.25)
                {
                    prevprev = prev;
                    prev = prev + (i * mult);
                }
                Debug.Log(prev);
            }
        };

        for (double time = 0; time < _clip.length; time += beattime)
        {
            result.AddAttack(time, att);
            result.m_audioManager = ap;
        }

        m_StateMap[_clipname] = result;
        result.Sort();

        return result;
    }

    public BaseState GetState(string _clipname)
    {
        return m_StateMap[_clipname];
    }
}
