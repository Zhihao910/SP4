using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Saving : MonoBehaviour {

    public static bool SaveToFile(string _filename, List<string> _data)
    {
        string ret2 = "Assets/Resources/" + _filename;

        if (!System.IO.File.Exists("Assets/Resources/" + _filename))
        {

            print("Making File");
            System.IO.File.Create("Assets/Resources/" + _filename);
        }
        foreach (string s in _data)
        {
            System.IO.File.AppendAllText(ret2, s);
            System.IO.File.AppendAllText(ret2, "\n");
        }
        
        return true;
    }

    public delegate bool ParserFunction(List<string> _data);

    public static bool LoadingFromFile(string _filename, ParserFunction _func)
    {
        string ret2 = "";

#if UNITY_STANDALONE
        ret2 = Application.dataPath+ "/Resources/" + _filename;
        print(ret2);
        if (!System.IO.File.Exists(ret2))
        {
            print("File doesnt exist!");
            return false;
        }
        List<string> ret = new List<string>();

        string[] data = System.IO.File.ReadAllLines(ret2);

        foreach (string s in data)
        {
            ret.Add(s);
        }
        if(_func(ret))
            return true;
#elif UNITY_ANDROID

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Songs.txt");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        www.Send();

        while (!www.isDone)
        {
            print("LOLOLOLOL");
        }
        string datameme = www.downloadHandler.text;
        string[] strings = datameme.Split('\n');
        print(datameme);
        List<string> listofstring = new List<string>();
        foreach (string s in strings) { print(s); listofstring.Add(s); }
        _func(listofstring); 
        // result = www.downloadHandler.text;


#endif


        return false;
    }

    public static bool TEMPLoadingFromFile(string _filename, ParserFunction _func)
    {
        string ret2 = "";

#if UNITY_STANDALONE
        ret2 = Application.streamingAssetsPath + "/" + _filename;
        print(ret2);
        if (!System.IO.File.Exists(ret2))
        {
            print("File doesnt exist!");
            return false;
        }
        List<string> ret = new List<string>();

        string[] data = System.IO.File.ReadAllLines(ret2);

        foreach (string s in data)
        {
            ret.Add(s);
        }
        if (_func(ret))
            return true;
#elif UNITY_ANDROID

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Songs.txt");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        www.Send();

        while (!www.isDone)
        {
            print("LOLOLOLOL");
        }
        string datameme = www.downloadHandler.text;
        string[] strings = datameme.Split('\n');
        print(datameme);
        List<string> listofstring = new List<string>();
        foreach (string s in strings) { print(s); listofstring.Add(s); }
        _func(listofstring); 
        // result = www.downloadHandler.text;


#endif


        return false;
    }


}
