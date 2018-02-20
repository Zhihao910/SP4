using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saving : MonoBehaviour {
    public static bool SaveToFile(string _filename, List<string> _data)
    {
        if(!System.IO.File.Exists(_filename))
        {
            System.IO.File.Create(_filename);
        }

        foreach (string s in _data)
        {
            System.IO.File.AppendAllText(_filename, s);
            System.IO.File.AppendAllText(_filename, "\n");

        }
        return true;
    }

    public delegate bool ParserFunction(List<string> _data);

    public static bool LoadingFromFile(string _filename, ParserFunction _func)
    {
        if (!System.IO.File.Exists(_filename))
        {
            return false;
        }

        List<string> ret = new List<string>();

        string[] data = System.IO.File.ReadAllLines(_filename);

        foreach (string s in data)
        {
            ret.Add(s);
        }
        if(_func(ret))
            return true;

        return false;
    }
}
