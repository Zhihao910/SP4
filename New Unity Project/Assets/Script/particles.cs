﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particles : MonoBehaviour
{
    bool idleparticle = false;
    [SerializeField]
    public ParticleSystem movementparticle;
    [SerializeField]
    public ParticleSystem movementparticle1;
    [SerializeField]
    public ParticleSystem movementparticle2;

    public Dictionary<string, ParticleSystem> storeparticle;
    // Use this for initialization
    void Start()
    {
        storeparticle = new Dictionary<string, ParticleSystem>();

        storeparticle.Add("moverightparticle", movementparticle);
        storeparticle.Add("moveleftparticle", movementparticle1);
        storeparticle.Add("idleparticle", movementparticle2);
    }
    // Update is called once per frame
    void Update()
    {
        if (storeparticle.ContainsKey("moverightparticle"))
        {
            ParticleSystem lol = storeparticle["moverightparticle"];
            Debug.Log(lol);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementparticle.Emit(1);
            movementparticle2.Emit(0);
            idleparticle = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            movementparticle.Emit(0);
            idleparticle = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementparticle1.Emit(1);
            idleparticle = true;
            movementparticle2.Emit(0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movementparticle1.Emit(0);
            idleparticle = false;
        }
        if (idleparticle == false)
            movementparticle2.Emit(1);

    }
}
