using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGenerator : MonoBehaviour {

    static Dictionary<string, BaseState> m_StateMap = new Dictionary<string, BaseState>();

    public enum GenerateType
    {
        INTROSTATE,

        BASESTATE,
        DROPSTATE,
        PARRYSTATE,
        QUICKTIMEEVENTSTATE,
        SHOCKWAVESTATE,
        NUMSTATE,//Default
    };

    [SerializeField]
    AudioPeerManager ap;

    [SerializeField]
    BpmAnalyzer ba; // ba black sheep

    [SerializeField]
    PlayerController pc;

    delegate BaseState GenerateFunc(string _clipname, AudioClip _clip, float _multiplier = 1f);
    Dictionary<GenerateType, GenerateFunc> _GenerateDictionary = new Dictionary<GenerateType, GenerateFunc>();

    void Awake()
    {
        _GenerateDictionary.Add(GenerateType.BASESTATE, CreateBaseState);
        _GenerateDictionary.Add(GenerateType.DROPSTATE, CreateDropState);
        _GenerateDictionary.Add(GenerateType.INTROSTATE, CreateIntroState);
        _GenerateDictionary.Add(GenerateType.PARRYSTATE, CreateParryState);
        _GenerateDictionary.Add(GenerateType.QUICKTIMEEVENTSTATE, CreateQuickTimeEvent);
        _GenerateDictionary.Add(GenerateType.SHOCKWAVESTATE, CreateShockwaveProjectile);
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public BaseState GenerateState(GenerateType _type, string _clipname, AudioClip _clip, float _multiplier)
    {
        if(_type == GenerateType.NUMSTATE)
        {
            GenerateType g = (GenerateType)Random.Range((int)GenerateType.BASESTATE, (int)GenerateType.NUMSTATE);
            print(g);
            return _GenerateDictionary[g](_clipname, _clip, _multiplier);
        }
        return _GenerateDictionary[_type](_clipname, _clip, _multiplier);
    }

    public BaseState GenerateState(GenerateType _type, string _clipname, AudioClip _clip)
    {
        if (_type == GenerateType.NUMSTATE)
        {
            GenerateType g = (GenerateType)Random.Range((int)GenerateType.BASESTATE, (int)GenerateType.NUMSTATE);
            print(g);
            return _GenerateDictionary[g](_clipname, _clip);
        }
        return _GenerateDictionary[_type](_clipname, _clip);
    }

    public BaseState CreateBaseState(string _clipname, AudioClip _clip,float multiplier = 4f)
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
            newgo.GetComponent<Projectile>().SetSpeed(10 / multiplier);

            
            for (int i = 0; i < 4; ++i)
            {
                GameObject newergo = Instantiate(newgo, pos, Quaternion.identity);
                target.x += 0.5f;
               
                newergo.GetComponent<Projectile>().SetDir(target);
                newergo.GetComponent<Projectile>().SetSpeed(10 / multiplier);
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
            newgo.GetComponent<Projectile>().SetSpeed(10 / multiplier);

            for (int i = 0; i < 3; ++i)
            {
                GameObject newergo = Instantiate(newgo, pos, Quaternion.identity);
                target.x += 0.5f;
                
                newergo.GetComponent<Projectile>().SetDir(target);
                newergo.GetComponent<Projectile>().SetSpeed(10 / multiplier);
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
                newgo.GetComponent<Projectile>().SetSpeed(10);
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

        BaseState.Attack spawnplatforms = () =>
        {
            for (int i = -2; i < 3; ++i)
            {
                gameObject.GetComponent<PlatformGenerator>().GeneratePlatform(new Vector3(i * 4, -8), new Vector3(i * 4, -6));
            }
        };
        result.AddAttack(0, spawnplatforms);
        result.AddAttack(0.2f, gameObject.GetComponent<PlatformGenerator>().ToggleGround);

        result.AddAttack(_clip.length - 0.01f, gameObject.GetComponent<PlatformGenerator>().DespawnAll);
        result.AddAttack(_clip.length - 0.02f, gameObject.GetComponent<PlatformGenerator>().ToggleGround);

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
            newgo.GetComponent<ExplodingProjectile>().SetSplitCount(6);
            newgo.GetComponent<Projectile>().SetSpeed(5);
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

        double beattime = ba.GetBeatTime() * multiplier;

        //Possible Keys
        List<int> _arrowKeys = new List<int>();
        //List<bool> _outcomeKeys = new List<bool>();

        int numberofKeys = 7;

        for (int i = 0; i < numberofKeys; ++i)
        {
            // which fucking idiot
            // thought it was a smart idea
            // to have min and max
            // have the min be "inclusive"
            // BUT THE MAX IS "EXCLUSIVE"
            // WHY?
            // WHY ARE YOU LIKE THIS?
            _arrowKeys.Add(Random.Range(1, 5));
        }

        // 1 - Up
        // 2 - Right
        // 3 - Down
        // 4 - Left

        int _counter = 0;
        int _buttonPressed = 0;
        double _QTETime = 0.0;
        int totalDmg = 0;

        GameObject _buttonQTE;
        List<GameObject> _buttonList = new List<GameObject>();

        BaseState.Attack att = () =>
        {
            if (_counter >= numberofKeys)
            {
                print("done");

                return;
            }

            if (_QTETime == 0.0)
            {
                if (_arrowKeys[_counter] == 1)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/ArrowUp") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
                if (_arrowKeys[_counter] == 2)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/ArrowRight") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
                if (_arrowKeys[_counter] == 3)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/ArrowDown") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
                if (_arrowKeys[_counter] == 4)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/ArrowLeft") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
            }

            _buttonPressed = 0;
            _QTETime += Time.deltaTime * 2;

            for (int i = 1; i < 5; ++i)
            {
                if (pc._keys[i] > 0.0)
                {
                    // will this even.. get called, at all? wtf.
                    // If newer key is pressed, OVERRIDE
                    if (_buttonPressed != 0 && pc._keys[_buttonPressed] > pc._keys[i])
                        continue;

                    _buttonPressed = i;
                }
            }

            for (int i = 1; i < 5; ++i)
            {
                pc._keys[i] = -1.0;
            }

            if (_buttonPressed == _arrowKeys[_counter])
            {
                //succ ess full
                print("QTE SUCCESS");

                _QTETime = 0.0;
                Destroy(_buttonList[0]);
                _buttonList.Remove(_buttonList[0]);

                //_outcomeKeys.Add(true);
                ++totalDmg;
                ++_counter;
            }
            else
            {
                // If they do the same shit, why not merge them?
                // Because what if we do different things :worry:

                // Press wrong
                if (_buttonPressed != 0)
                {
                    // boi you fuked up
                    // oof ooch owie
                    print("Wrong Button!");

                    _QTETime = 0.0;
                    Destroy(_buttonList[0]);
                    _buttonList.Remove(_buttonList[0]);

                    pc.screenShake.ShakeCamera();
                    //_outcomeKeys.Add(false);
                    ++totalDmg;
                    ++_counter;
                } // Out of time
                else if (_QTETime > beattime)
                {
                    // boi you fuked up
                    // oof ooch owie
                    print("Too slow!");

                    _QTETime = 0.0;
                    Destroy(_buttonList[0]);
                    _buttonList.Remove(_buttonList[0]);

                    pc.screenShake.ShakeCamera();
                    //_outcomeKeys.Add(false);
                    ++totalDmg;
                    ++_counter;
                }
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

    // Shockwave Projectile (Drop 2 or some shit?)
    public BaseState CreateShockwaveProjectile(string _clipname, AudioClip _clip, float multiplier = 8.0f)
    {
        multiplier = 8.0f;
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        double beattime = ba.GetBeatTime() * multiplier;

        //            Vector3 pos = new Vector3(Random.Range(-7, 8), 8);
            //target.x = pos.x;
        //gameObject.GetComponent<Transform>().position.Set(Random.Range(-7, 8), gameObject.GetComponent<Transform>().position.y, gameObject.GetComponent<Transform>().position.z);
        Vector3 target = new Vector3(transform.position.x, -4, transform.position.z);

        // uhh how do i slow down spawning omegalul

        BaseState.Attack att = () =>
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            pos.y = 9;
            pos.x = Random.Range(-7, 8);
            target.x = pos.x;

            Object o = Resources.Load("Prefabs/ProjectileShockwave");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, pos, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");;

            GameObject parent = Instantiate(Resources.Load("Prefabs/FakeParent") as GameObject);
            newgo.transform.parent = parent.transform;
            newgo.GetComponent<Projectile>().SetTarget(target);
            newgo.GetComponent<Projectile>().SetDir((target - pos).normalized);
            newgo.GetComponent<ShockwaveProjectile>().SetWaves(5);
            newgo.GetComponent<Projectile>().SetSpeed(10);
            newgo.GetComponent<Projectile>().transform.localScale *= 2;
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

    public BaseState CreateLaserAttack(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        double beattime = ba.GetBeatTime() * multiplier;

        Vector3 target = new Vector3(transform.position.x, -4, transform.position.z);

        // uhh how do i slow down spawning omegalul

        BaseState.Attack att = () =>
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Object o = Resources.Load("Prefabs/laserprojectile");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, pos, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            target.x = Random.Range(-7, 8);

            GameObject parent = Instantiate(Resources.Load("Prefabs/FakeParent") as GameObject);
            newgo.transform.parent = parent.transform;
            newgo.GetComponent<Projectile>().SetTarget(target);
            newgo.GetComponent<Projectile>().SetDir((target - pos).normalized);
            newgo.GetComponent<Projectile>().SetSpeed(5);
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
