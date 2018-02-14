using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour {

    public AudioPeerManager m_audioManager;

    [SerializeField]
    List<double> m_Attacks = new List<double>();

    Queue<double> m_Queue = new Queue<double>();

    [SerializeField]
    string m_clipname;

    bool m_Run = false;

    // Use this for initialization
    void Start () {
        foreach (double a in m_Attacks)
            m_Queue.Enqueue(a);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Run)
        {
            Debug.Log(m_audioManager.TimeNow());
            if (m_Queue.Peek() <= m_audioManager.TimeNow())
            {
                Debug.Log("PEW PEW PEW");

                double a = m_Queue.Dequeue();
                
            }
        }
	}

    public void AddAttack(double _go)
    {
        m_Attacks.Add(_go);
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
        m_Run = true;
    }

    public void StopRun()
    {
        m_Run = false;
    }
}
