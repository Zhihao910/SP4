using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour {

    public delegate void Attack();

    public AudioPeerManager m_audioManager;

    [SerializeField]
    List<KeyValuePair<double, Attack>> m_Attacks = new List<KeyValuePair<double, Attack>>();

    Queue<KeyValuePair<double, Attack>> m_Queue = new Queue<KeyValuePair<double, Attack>>();
    

    [SerializeField]
    string m_clipname;

    bool m_Run = false;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (m_Run && m_Queue.Count > 0)
        {
            if (m_Queue.Peek().Key <= m_audioManager.TimeNow())
            {
                KeyValuePair<double, Attack> value = m_Queue.Dequeue();
                double StartTIme = value.Key;
                value.Value();
            }
        }
	}

    public void AddAttack(double _go,Attack _function)
    {
        m_Attacks.Add(new KeyValuePair<double, Attack>(_go, _function));
    }


    public string GetClipName()
    {
        return m_clipname;
    }

    public void SetClipName(string _clipname)
    {
        m_clipname = _clipname;
    }

    public void Run()
    {
        foreach (KeyValuePair<double,Attack> a in m_Attacks)
            m_Queue.Enqueue(a);
        m_Run = true;
    }

    public void StopRun()
    {
        m_Run = false;
    }

    public void Sort()
    {
        m_Attacks.Sort((x, y) => x.Key.CompareTo(y.Key));
    }
}
