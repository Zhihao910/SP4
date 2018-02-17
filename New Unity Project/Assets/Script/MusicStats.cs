using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MusicStats : MonoBehaviour
{
    // File name, no .txt
    public string _fileName;

    // Reads file
    public TextAsset _asset;

    // Writes to file
    private StreamWriter _writer;

    // Reads the file
    private StreamReader _reader;

	// Use this for initialization
	void Start ()
    {
        _fileName = "MusicStats";
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (BPM._bpmbeat)
            WriteToFile("success!");
        else
        {

        }

        if (SearchFor("success!"))
        {
            Debug.Log("Searched and found!");
        }
	}

    public void WriteToFile(string _appendString)
    {
        _asset = Resources.Load(_fileName + ".txt") as TextAsset;
        _writer = new StreamWriter("Assets/Resources/" + _fileName + ".txt");
        _writer.WriteLine(_appendString);
    }

    public bool SearchFor(string _findItem)
    {
        _reader = new StreamReader("Assets/Resources/" + _fileName + ".txt");

        if (_reader.ReadToEnd().Contains(_findItem))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
