using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeComparer : IComparer<float> {

    public static float starttime;

    public int Compare(float start, float end)
    {
        if (start > starttime && start < end)
            return 1.CompareTo(1);
        
            return 1.CompareTo(2);
    }
}
