using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ToGame()
    {
        SceneManager.LoadScene("Overworld");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ToHelp()
    {
        SceneManager.LoadScene("Help");
    }
    public void ToQuit()
    {
        Application.Quit();
    }
}
