using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckerScript : MonoBehaviour {
    [SerializeField]
    UnityEngine.UI.Dropdown dropdown;
    Dictionary<string, string> dict = new Dictionary<string, string>();

    
    List<string> listOfStuff = new List<string>();

    // Use this for initialization
    void Start()
    {
        dropdown = gameObject.GetComponent<UnityEngine.UI.Dropdown>();
        foreach (string s in System.IO.Directory.GetFiles(System.IO.Path.Combine(Application.streamingAssetsPath, "UserMusic/")))
        {
            if (s.Contains(".meta"))
                continue;
            string lmoa = s.Replace(System.IO.Path.Combine(Application.streamingAssetsPath, "UserMusic/"), "");
            listOfStuff.Add(lmoa);
            dict.Add(lmoa, s);
        }
        AddStuff();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Transit()
    {
        PlayerPrefs.SetString("Song", dict[dropdown.options[dropdown.value].text]);
        SceneManager.LoadScene("SongDetection");

    }

    public void AddStuff()
    {
        dropdown.AddOptions(listOfStuff);
    }
}
