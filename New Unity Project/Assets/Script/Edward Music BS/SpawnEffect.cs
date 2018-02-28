using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public static bool _spawnBass;
    public static bool _spawnKick;
    public static bool _spawnCenter;
    public static bool _spawnMelody;
    public static bool _spawnHigh;
    public static bool _spawnThree;

    // Use this for initialization
    void Start ()
    {
        _spawnBass = false;
        _spawnKick = false;
        _spawnCenter = false;
        _spawnMelody = false;
        _spawnHigh = false;
        _spawnThree = false;
    }
}
