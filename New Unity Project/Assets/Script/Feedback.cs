using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _pass;
    [SerializeField]
    AudioClip _fail;

    // Use this for initialization
    void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Create an object. _name doesnt need "Prefabs/". Lifetime default is 1.0f.
    public bool CreateImage(string _name, Vector3 _pos, float _lifetime = 1.0f)
    {
        //print("made!");
        //print(_pos);
        GameObject _temp = Instantiate(Resources.Load("Prefabs/" + _name) as GameObject);
        _temp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        _pos.z = -1;

        _temp.transform.localPosition = _pos;

        Destroy(_temp, _lifetime);

        return true;
    }

    // Use this if you want it to follow something
    public bool CreateImage(string _name, GameObject _go, float _lifetime = 1.0f)
    {
        // I'm not sure what I'm doing here tbh fam
        GameObject _temp = Instantiate(Resources.Load("Prefabs/" + _name) as GameObject);

        _temp.transform.parent = _go.transform;

        Destroy(_temp, _lifetime);

        return true;
    }

    public bool CreateAudio(string _name)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        switch (_name)
        {
            case "Pass":
                _audioSource.clip = _pass;
                _audioSource.Play();
                break;
            case "Fail":
                _audioSource.clip = _fail;
                _audioSource.Play();
                break;
        }

        return true;
    }
}