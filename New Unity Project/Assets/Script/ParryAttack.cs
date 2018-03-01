using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttack : MonoBehaviour
{
    private GameObject _feedback;

    [SerializeField]
    Score playerScore;


    [SerializeField]
    BossHealth _bh;

    // Use this for initialization
    void Start()
    {
        _feedback = GameObject.FindGameObjectWithTag("Feedback");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("parableProjectile"))
        {
            print("Parried!");
            _feedback.GetComponent<Feedback>().CreateImage("ParryPass", other.gameObject.transform.position);
            _feedback.GetComponent<Feedback>().CreateAudio("Pass");

            //this.GetComponent<PlayerController>().mana += 10;
            gameObject.GetComponentInParent<PlayerController>().mana += 10;

            _bh.health -= 3;

            print(_bh.health);

            // Add base 300 score
            playerScore.AddScore(300.0f);

            // Per successful hit, increase multiplier
            // Like uhh, combo.. bonus?
            playerScore.AddMultiplier(0.1f);

            //other.gameObject.SetActive(false);
            if (other.transform.parent)
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}
