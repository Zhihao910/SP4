using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particles : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem movementparticle;
    [SerializeField]
    public ParticleSystem movementparticle1;
    [SerializeField]
    public ParticleSystem movementparticle2;
    [SerializeField]
    public ParticleSystem movementparticle3;
    // Use this for initialization
    void Start()
    {
    }
    void OnParticleCollision(GameObject other)
    {
        EmitAtLocation();
    }
    void EmitAtLocation()
    {
        movementparticle2.Emit(1);
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementparticle.Emit(1);
            movementparticle3.Emit(0);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            movementparticle.Emit(0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementparticle1.Emit(1);
            movementparticle3.Emit(0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movementparticle1.Emit(0);
        }
        movementparticle3.Emit(1);
    }
}
