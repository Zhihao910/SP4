using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEState : BaseState
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (m_Run && m_Queue.Count > 0)
        {
            if (m_Queue.Peek().Key >= m_audioManager.TimeNow())
            {
                KeyValuePair<double, Attack> value = m_Queue.Dequeue();
            }
            m_Queue.Peek().Value();
        }
    }
}
