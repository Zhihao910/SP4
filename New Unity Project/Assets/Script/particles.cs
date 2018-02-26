using System.Collections;
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
    bool downbtn = false;
    public Dictionary<string, ParticleSystem> storeparticle;

    [SerializeField]
    GameObject[] Particles;
    Dictionary<string, GameObject> _ParticleMap = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        storeparticle = new Dictionary<string, ParticleSystem>();

        movementparticle = new ParticleSystem();
        movementparticle1 =new ParticleSystem();
        movementparticle2 = new ParticleSystem();

        //storeparticle.Add("moverightparticle", movementparticle);
        //storeparticle.Add("moveleftparticle", movementparticle1);
        //storeparticle.Add("idleparticle", movementparticle2);

        foreach (GameObject go in Particles)
        {
            _ParticleMap.Add(go.name, go);
        }

    }
    void AddParticle(string particlename, ParticleSystem particlevariable)
    {
        storeparticle.Add(particlename, particlevariable);
    }
    // Update is called once per frame
    void Update()
    {
        //if (storeparticle.ContainsKey("moverightparticle"))
        //{
        //    ParticleSystem lol = storeparticle["moverightparticle"];
        //    //Debug.Log(lol);
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    movementparticle.Emit(0);
        //    movementparticle1.Emit(0);
        //    movementparticle2.Emit(1);
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    //red
        //    movementparticle.Emit(1);
        //    movementparticle2.Emit(0);
        //    idleparticle = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.RightArrow))
        //{
        //    movementparticle.Emit(0);
        //    idleparticle = false;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    movementparticle1.Emit(1);
        //    idleparticle = true;
        //    movementparticle2.Emit(0);
            
        //}
        //else if (Input.GetKeyUp(KeyCode.LeftArrow))
        //{
        //    //red
        //    movementparticle1.Emit(0);
        //    idleparticle = false;
        //if (storeparticle.ContainsKey("moverightparticle"))
        //{
        //    ParticleSystem lol = storeparticle["moverightparticle"];
        //    //Debug.Log(lol);
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    movementparticle.Emit(0);
        //    movementparticle1.Emit(0);
        //    movementparticle2.Emit(1);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    //red
        //    //movementparticle.Emit(1);
        //    //movementparticle2.Emit(0);
        //    movementparticle.Play();
        //    idleparticle = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.RightArrow))
        //{
        //    //movementparticle.Emit(0);
        //    movementparticle.Stop();
        //    idleparticle = false;
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    movementparticle1.Emit(1);
        //    movementparticle2.Emit(0);
        //    movementparticle1.Play();
        //    idleparticle = true;
            
        //}
        //else if (Input.GetKeyUp(KeyCode.LeftArrow))
        //{
        //    //red
        //    movementparticle1.Emit(0);
        //    movementparticle.Stop();
        //    idleparticle = false;
          
        ////}
        ////green colour particle
        ////if (idleparticle == false)
        ////movementparticle2.Emit(1);

    }

    public void ApplyParticle(GameObject _parent, string _particlename, float _lifetime = 0.1f, float _speed = 1f, bool _loop = false)
    {
        GameObject newparticle = Instantiate(_ParticleMap[_particlename]);
        newparticle.transform.parent = _parent.transform;
        newparticle.transform.position = _parent.transform.position;

        ParticleSystem.MainModule main = newparticle.GetComponent<ParticleSystem>().main;
        main.startLifetime = _lifetime;
        main.startSpeed = _speed;
        main.loop = _loop;
        newparticle.GetComponent<ParticleSystem>().Emit(0);
    }
}
