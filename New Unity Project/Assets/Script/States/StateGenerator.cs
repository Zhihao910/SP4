using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGenerator : MonoBehaviour {

    static Dictionary<string, BaseState> m_StateMap = new Dictionary<string, BaseState>();

    [SerializeField]
    AudioPeerManager ap;

    [SerializeField]
    BpmAnalyzer ba; // ba black sheep

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public BaseState CreateBaseState(string _clipname, AudioClip _clip,float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        // ba.FindBpm();

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588 //0.4918

        BaseState.Attack att = () =>
        {

            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Vector3 target = new Vector3(-1, -1, 0);
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
            Vector3 target = new Vector3(-0.75f, -1, 0);
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
        result.SetClipName(_clip.name);
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
                newgo.GetComponent<Projectile>().SetDir(new Vector3(prev + (i * mult), -1, 0));
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

    public BaseState CreateIntroState(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588 0.4918 

        BaseState.Attack att = () =>
        {
            for (int i = -2; i < 3; ++i)
            {
                gameObject.GetComponent<PlatformGenerator>().GeneratePlatform(new Vector3(i * 4, -10), new Vector3(i * 4, -6));
            }
        };

        result.AddAttack(0, att);
        result.m_audioManager = ap;

        m_StateMap[_clipname] = result;
        result.Sort();

        return result;
    }

    public BaseState CreateParryState(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;//0.5357; // 0.588
        Vector3 target = new Vector3(-8, -2, transform.position.z);
        int mult = 1;
        int prevprev = 0;

        BaseState.Attack att = () =>
        {
            if (prevprev != 0 && Mathf.Abs(target.x) == 8)
                mult = -mult;
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Object o = Resources.Load("Prefabs/Projectile4");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, pos, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            GameObject parent = Instantiate(Resources.Load("Prefabs/FakeParent") as GameObject);
            newgo.transform.parent = parent.transform;
            newgo.GetComponent<Projectile>().SetTarget(target);
            newgo.GetComponent<Projectile>().SetDir((target - pos).normalized);
            newgo.GetComponent<ExplodingProjectile>().SetSplitCount(8);
            newgo.GetComponent<Projectile>().projectileSpeed = 1;
            prevprev = (int)(target.x);
            target.x += (4 * mult);

        };

        for (double time = 0; time < _clip.length; time += beattime)
        {
            result.AddAttack(time, att);
            result.m_audioManager = ap;
        }

        result.AddAttack(_clip.length - 0.5f, GetComponent<PlatformGenerator>().DespawnAll);


        m_StateMap[_clipname] = result;
        result.Sort();

        return result;
    }

    // Quick Time Event

    public BaseState CreateQuickTimeEvent(string _clipname, AudioClip _clip, float multiplier = 1.0f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        double beattime = 0.4918 * multiplier;

        //Vector3 leftlimit = new Vector3(-0.75f, 1, 0);
        //Vector3 rightlimit = new Vector3(0.75f, 1, 0);
        //float prevprev = 0;
        //float prev = 0.5f;
        //float mult = -1;
        //BaseState.Attack att = () =>
        //{
        //    Vector3 pos = gameObject.GetComponent<Transform>().position;

        //    if (prevprev != 0 && (prev == leftlimit.x || prev == rightlimit.x))
        //        mult = -mult;


        //    for (float i = -0.25f; i < 0.5f; i += 0.25f)
        //    {
        //        Object o;
        //        if (i == 0)
        //            o = Resources.Load("Prefabs/Projectile2");
        //        else
        //            o = Resources.Load("Prefabs/Projectile1");

        //        if (o == null) Debug.Log("Load failed");
        //        GameObject go = o as GameObject;
        //        if (go == null) Debug.Log("Loaded object isn't GameObject");
        //        GameObject newgo = Instantiate(go, pos, Quaternion.identity);
        //        if (newgo == null) Debug.Log("Couldn't instantiate");
        //        newgo.GetComponent<Projectile>().SetDir(new Vector3(prev + (i * mult), -1, 0));
        //        newgo.GetComponent<Projectile>().projectileSpeed = 10;
        //        if (i == 0.25)
        //        {
        //            prevprev = prev;
        //            prev = prev + (i * mult);
        //        }
        //        Debug.Log(prev);
        //    }
        //};

        BaseState.Attack att = () =>
        {

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
