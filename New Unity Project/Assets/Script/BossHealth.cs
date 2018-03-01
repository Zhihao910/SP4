using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossHealth : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    public float totalhealth = 100;
    [SerializeField]
    public float health = 100;

    [SerializeField]
    GameObject Healthbar;

    [SerializeField]
    Score playerScore;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //scaling of the healthbar
        Healthbar.transform.localScale = new Vector3(PlayerPrefs.GetFloat("something") / totalhealth, 1, 1);
        Debug.Log(PlayerPrefs.GetFloat("something"));
        if (health <= 0)
        {
            //playerScore.SaveScore();
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }
}
