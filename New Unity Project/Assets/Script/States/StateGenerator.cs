using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateGenerator : MonoBehaviour
{

    static Dictionary<string, BaseState> m_StateMap = new Dictionary<string, BaseState>();

    public enum GenerateType
    {
        INTROSTATE,

        BASESTATE,
        DROPSTATE,
        PARRYSTATE,
        QUICKTIMEEVENTSTATE,
        SHOCKWAVESTATE,
        MULTISTATE,
        LAZERSTATE,
        NUMSTATE,//Default
    };
    float timerdestroy;
    [SerializeField]
    AudioPeerManager ap;

    [SerializeField]
    BpmAnalyzer ba; // ba black sheep

    [SerializeField]
    PlayerController pc;

    [SerializeField]
    Score playerScore;

    [SerializeField]
    BossHealth _bossHP;

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
        _GenerateDictionary.Add(GenerateType.MULTISTATE, MultiHighState);
        _GenerateDictionary.Add(GenerateType.LAZERSTATE, CreateLaserAttack);

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public BaseState GenerateState(GenerateType _type, string _clipname, AudioClip _clip, float _multiplier)
    {
        if (_type == GenerateType.NUMSTATE)
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

    public BaseState CreateBaseState(string _clipname, AudioClip _clip, float multiplier = 1f)
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
            result.AddAttack(time + beattime / 2, att2);

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

        //multiplier *= 0.75f;
        double beattime = ba.GetBeatTime() * multiplier;

        // 1 - Up
        // 2 - Right
        // 3 - Down
        // 4 - Left

        int _counter = 0;
        int _buttonPressed = 0;
        double _QTETime = 0.0;
        int totalDmg = 0;
        double fifthTime = (beattime * 0.19);
        float warnTime = (float)(beattime * (0.2 * multiplier));
        int _countdown = 0;

        //Possible Keys
        List<int> _InputKeys = new List<int>();
        //List<bool> _outcomeKeys = new List<bool>();

        int numberofKeys = 30;

        for (int i = 0; i < numberofKeys; ++i)
        {
            // which fucking idiot
            // thought it was a smart idea
            // to have min and max
            // have the min be "inclusive"
            // BUT THE MAX IS "EXCLUSIVE"
            // WHY?
            // WHY ARE YOU LIKE THIS?
            _InputKeys.Add(Random.Range(1, 5));
        }

        GameObject _buttonQTE;
        List<GameObject> _buttonList = new List<GameObject>();

        GameObject _feedback = GameObject.FindGameObjectWithTag("Feedback");

        BaseState.Attack warn = () =>
        {
            ++_countdown;

            switch (_countdown)
            {
                case 1:
                    _feedback.GetComponent<Feedback>().CreateImage("CountReady", new Vector3(0, 0), warnTime);
                    break;
                case 2:
                    _feedback.GetComponent<Feedback>().CreateImage("CountThree", new Vector3(0, 0), warnTime);
                    break;
                case 3:
                    _feedback.GetComponent<Feedback>().CreateImage("CountTwo", new Vector3(0, 0), warnTime);
                    break;
                case 4:
                    _feedback.GetComponent<Feedback>().CreateImage("CountOne", new Vector3(0, 0), warnTime);
                    break;
                case 5:
                    _feedback.GetComponent<Feedback>().CreateImage("CountGo", new Vector3(0, 0), warnTime);
                    break;
            }
        };

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        BaseState.Attack att = () =>
        {
            if (_counter >= numberofKeys)
            {
                print("done");

                return;
            }

            if (_QTETime == 0.0)
            {
                if (_InputKeys[_counter] == 1)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/KeyboardQ") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
                if (_InputKeys[_counter] == 2)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/KeyboardW") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
                if (_InputKeys[_counter] == 3)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/KeyboardE") as GameObject);
                    _buttonQTE.transform.parent = pc.transform;

                    Vector3 _buttonPos = pc.transform.position;
                    Vector2 _randPos = Random.insideUnitCircle;
                    _buttonPos += new Vector3(_randPos.x, _randPos.y);
                    _buttonPos.y += 2;
                    _buttonQTE.transform.position = _buttonPos;

                    _buttonList.Add(_buttonQTE);

                    //Destroy(_buttonQTE, (float)beattime);
                }
                if (_InputKeys[_counter] == 4)
                {
                    _buttonQTE = Instantiate(Resources.Load("Prefabs/KeyboardR") as GameObject);
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
            _QTETime += Time.deltaTime * 8;

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

            if (_buttonPressed == _InputKeys[_counter])
            {
                //succ ess full
                print("QTE SUCCESS");

                // spawn a tick above player head
                // con-fookin-gratis
                // you pressed a button
                _feedback.GetComponent<Feedback>().CreateImage("ParryPass", pc.transform.position + new Vector3(0, 1));
                _feedback.GetComponent<Feedback>().CreateAudio("Pass");

                _QTETime = 0.0;
                Destroy(_buttonList[0]);
                _buttonList.Remove(_buttonList[0]);

                //_outcomeKeys.Add(true);
                ++totalDmg;
                ++_counter;

                // Add base 100 score
                playerScore.AddScore(100.0f);
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

                    // how are you so bad
                    _feedback.GetComponent<Feedback>().CreateImage("ParryFail", pc.transform.position + new Vector3(0, 1));
                    _feedback.GetComponent<Feedback>().CreateAudio("Fail");

                    _QTETime = 0.0;
                    Destroy(_buttonList[0]);
                    _buttonList.Remove(_buttonList[0]);

                    pc.screenShake.ShakeCamera();
                    //_outcomeKeys.Add(false);
                    ++_counter;
                } // Out of time
                else if (_QTETime > beattime)
                {
                    // boi you fuked up
                    // oof ooch owie
                    print("Too slow!");

                    // how are you so bad
                    _feedback.GetComponent<Feedback>().CreateImage("ParryFail", pc.transform.position + new Vector3(0, 1));
                    _feedback.GetComponent<Feedback>().CreateAudio("Fail");

                    _QTETime = 0.0;
                    Destroy(_buttonList[0]);
                    _buttonList.Remove(_buttonList[0]);

                    pc.screenShake.ShakeCamera();
                    //_outcomeKeys.Add(false);
                    ++_counter;
                }
            }
        };
#elif UNITY_ANDROID
        BaseState.Attack att = () =>
        {
            if (_counter >= numberofKeys)
            {
                print("done");

                return;
            }

            if (_QTETime == 0.0)
            {
                _buttonQTE = Instantiate(Resources.Load("Prefabs/QTETappable") as GameObject);
                _buttonQTE.transform.SetParent(GameObject.FindGameObjectWithTag("MobileCanvas").transform);
                Vector2 _randPos = Random.insideUnitCircle;
                _buttonQTE.transform.position = new Vector3(400 + (_randPos.x * 100.0f), 200 + (_randPos.y * 100.0f));
                _buttonList.Add(_buttonQTE);
            }

            // ever wondered why you have no time to qte?
            _QTETime += Time.deltaTime * 8;

            if (QTETappable._tapped)
            {
                //succ ess full
                print("QTE SUCCESS");

                // spawn a tick above player head
                // con-fookin-gratis
                // you pressed a button
                _feedback.GetComponent<Feedback>().CreateImage("ParryPass", pc.transform.position + new Vector3(0, 1));
                _feedback.GetComponent<Feedback>().CreateAudio("Pass");

                _QTETime = 0.0;
                Destroy(_buttonList[0]);
                _buttonList.Remove(_buttonList[0]);

                //_outcomeKeys.Add(true);
                ++totalDmg;
                ++_counter;

                // Add base 100 score
                playerScore.AddScore(100.0f);

                QTETappable._tapped = false;
            }

            if (_QTETime > beattime)
            {
                // boi you fuked up
                // oof ooch owie
                print("Too slow!");

                // how are you so bad
                _feedback.GetComponent<Feedback>().CreateImage("ParryFail", pc.transform.position + new Vector3(0, 1));
                _feedback.GetComponent<Feedback>().CreateAudio("Fail");

                _QTETime = 0.0;
                Destroy(_buttonList[0]);
                _buttonList.Remove(_buttonList[0]);

                pc.screenShake.ShakeCamera();
                //_outcomeKeys.Add(false);
                ++_counter;

                QTETappable._tapped = false;
            }
        };
#endif

        BaseState.Attack clear = () =>
        {
            while (_buttonList.Count != 0)
            {
                Destroy(_buttonList[0]);
                _buttonList.Remove(_buttonList[0]);
            }

            // Add base 250 per successful parry
            playerScore.AddScore(250.0f * totalDmg);
            // Minus boss HP by totalDmg (or something)
            _bossHP.health -= totalDmg;
        };

        for (double time = 0; time < (beattime * 4); time += (double)warnTime)
        {
            result.AddAttack(time, warn);
            result.m_audioManager = ap;
        }

        for (double time = (beattime * 4); time < (_clip.length - 0.2f); time += beattime)
        {
            result.AddAttack(time, att);
            result.m_audioManager = ap;
        }

        result.AddAttack((_clip.length - 0.2f), clear);

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
            if (newgo == null) Debug.Log("Couldn't instantiate"); ;

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
        Vector3 target = new Vector3(-8.5f, transform.position.y, transform.position.z);
        Vector3 target2 = new Vector3(-12f, transform.position.y, transform.position.z);
        float lifetime = 2.0f;
        bool alternate = false;

        BaseState.Attack warn = () =>
        {
            target.y = Random.Range(-4, 2f);
            target2.y = target.y;

            Object o = Resources.Load("Prefabs/Indicator");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, target, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");
            Debug.Log(alternate);
            Destroy(newgo,lifetime);
        };


        BaseState.Attack att = () =>
        {
            Object o = Resources.Load("Prefabs/laserprojectile");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, target2, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            newgo.GetComponent<Projectile>().SetDir(new Vector3(1, 0, 0));
            newgo.GetComponent<Projectile>().SetSpeed(15);
        };
        for (double time = 0; time < _clip.length; time += beattime)
        {
            if (alternate)
            {
                result.AddAttack(time, att);
            }
            else
            {
                result.AddAttack(time, warn);
            }

            alternate = !alternate;
            //result.AddAttack(time, att);
            result.m_audioManager = ap;
        }
        m_StateMap[_clipname] = result;
        result.Sort();

        return result;
    }
    public BaseState CreateBlindAttack(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState result = gameObject.AddComponent<BaseState>();
        result.SetClipName(_clip.name);
        //Run adding attacks here

        double beattime = ba.GetBeatTime() * multiplier;
        //for blackness
        Vector3 target = new Vector3(transform.position.x, 10, transform.position.z);
        //for the projectile
        Vector3 target2 = new Vector3(transform.position.x, 10, transform.position.z);
        Vector3 target3 = new Vector3(transform.position.x, 10, transform.position.z);
        Vector3 target4 = new Vector3(transform.position.x, 10, transform.position.z);
        Vector3 target5 = new Vector3(transform.position.x, 10, transform.position.z);
        target.x = 0;
        target.z = -5;
        bool alternate = false;

        // uhh how do i slow down spawning omegalul
        float randomspeed;
        randomspeed = Random.Range(5, 10);
        target2.x = Random.Range(-8, 8);
        target3.x = Random.Range(-8, 8);
        target4.x = Random.Range(-8, 8);
        target5.x = Random.Range(-8, 8);
        BaseState.Attack warn = () =>
        {
            Object o = Resources.Load("Prefabs/Black");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, target, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");
        };


        BaseState.Attack att = () =>
        {
            Object o = Resources.Load("Prefabs/Projectile4");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, target2, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate");

            newgo.GetComponent<Projectile>().SetDir(new Vector3(0, -1, 0));
            newgo.GetComponent<Projectile>().SetSpeed(randomspeed);

            Object o1 = Resources.Load("Prefabs/Projectile4");
            if (o1 == null) Debug.Log("Load failed");
            GameObject go1 = o1 as GameObject;
            if (go1 == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo1 = Instantiate(go, target3, Quaternion.identity);
            if (newgo1 == null) Debug.Log("Couldn't instantiate");

            newgo1.GetComponent<Projectile>().SetDir(new Vector3(0, -1, 0));
            newgo1.GetComponent<Projectile>().SetSpeed(randomspeed);

            Object o2 = Resources.Load("Prefabs/Projectile4");
            if (o2 == null) Debug.Log("Load failed");
            GameObject go2 = o2 as GameObject;
            if (go2 == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo2 = Instantiate(go, target4, Quaternion.identity);
            if (newgo2 == null) Debug.Log("Couldn't instantiate");

            newgo2.GetComponent<Projectile>().SetDir(new Vector3(0, -1, 0));
            newgo2.GetComponent<Projectile>().SetSpeed(randomspeed);

            Object o3 = Resources.Load("Prefabs/Projectile4");
            if (o3 == null) Debug.Log("Load failed");
            GameObject go3 = o3 as GameObject;
            if (go3 == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo3 = Instantiate(go, target5, Quaternion.identity);
            if (newgo3 == null) Debug.Log("Couldn't instantiate");

            newgo3.GetComponent<Projectile>().SetDir(new Vector3(0, -1, 0));
            newgo3.GetComponent<Projectile>().SetSpeed(randomspeed);

        };

        for (double time = 0; time < _clip.length; time += beattime)
        {

            if (alternate)
            {
                result.AddAttack(time, att);
            }
            else
            {
                result.AddAttack(time, warn);
            }

            alternate = !alternate;
            //result.AddAttack(time, att);
        }

        result.m_audioManager = ap;

        m_StateMap[_clipname] = result;
        result.Sort();

        return result;
    }

    public BaseState1 MultiHighState(string _clipname, AudioClip _clip, float multiplier = 1f)
    {
        BaseState1 result = gameObject.AddComponent<BaseState1>();
        result.SetClipName(_clip.name);
        //Run adding attacks here
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

        Vector3 target1 = new Vector3(transform.position.x, -4, transform.position.z);

        // uhh how do i slow down spawning omegalul

        BaseState.Attack att3 = () =>
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            pos.y = 9;
            pos.x = Random.Range(-7, 8);
            target1.x = pos.x;

            Object o = Resources.Load("Prefabs/ProjectileShockwave");
            if (o == null) Debug.Log("Load failed");
            GameObject go = o as GameObject;
            if (go == null) Debug.Log("Loaded object isn't GameObject");
            GameObject newgo = Instantiate(go, pos, Quaternion.identity);
            if (newgo == null) Debug.Log("Couldn't instantiate"); ;

            GameObject parent = Instantiate(Resources.Load("Prefabs/FakeParent") as GameObject);
            newgo.transform.parent = parent.transform;
            newgo.GetComponent<Projectile>().SetTarget(target1);
            newgo.GetComponent<Projectile>().SetDir((target1 - pos).normalized);
            newgo.GetComponent<ShockwaveProjectile>().SetWaves(20);
            newgo.GetComponent<Projectile>().SetSpeed(10);
            newgo.GetComponent<Projectile>().transform.localScale *= 2;
        };

        result.AddAttack(BaseState1.Type.BASS_TYPE, att);
        result.AddAttack(BaseState1.Type.KICK_TYPE, att2);
        result.AddAttack(BaseState1.Type.GENERAL_TYPE, att3);

        result.m_audioManager = ap;

        double beattime = ba.GetBeatTime() * multiplier;
        result.m_audioManager = ap;

        m_StateMap[_clipname] = result;
        return result;
    }

    public BaseState GetState(string _clipname)
    {
        return m_StateMap[_clipname];
    }
}
