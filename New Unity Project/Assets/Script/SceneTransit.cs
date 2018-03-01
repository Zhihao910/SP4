using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransit : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Screen.orientation = ScreenOrientation.Landscape;

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
    public void ToSongEnded()
    {
        SceneManager.LoadScene("songend");
    }
    public void ToInstruction()
    {
        SceneManager.LoadScene("instruction");
    }
    public void ToQuit()
    {
        Application.Quit();
    }

    public void ToOverWorld()
    {
        SceneManager.LoadScene("Overworld");
    }
}
