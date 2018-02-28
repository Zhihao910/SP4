using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    // LOCAL HIGHSCORE
    public int[] _highscore = new int[10];

    // Current Score
    private int _currScore = 0;
    // Default Multiplier
    private float _defaultMulti = 1.0f;
    // Multiplier
    private float _multiplier = 1.0f;
    // To make noise whenever damage is done
    private GameObject _feedback;

    public Text _text;

    // Use this for initialization
    void Start()
    {
        // Does it exist? If yes, read and input into _highscore
        if (PlayerPrefs.HasKey("highscore0"))
        {
            for (int _num = 0; _num < _highscore.Length; ++_num)
            {
                _highscore[_num] = PlayerPrefs.GetInt("highscore" + _num.ToString());
            }
        }
        else
        {
            // If no saved score exists (really?)
            // Create highscores, set all to zero
            ClearAllScores();
        }

        // locate feedback gameobject
        _feedback = GameObject.FindGameObjectWithTag("Feedback");

        // Initialise score text
        DisplayScore();

        if (SceneManager.GetActiveScene().name == "HighscoreScene")
        {
            print("THIS IS HIGHSCORE SCENE");
            DisplayHighScore();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // uh
        // maybe i dont need this
    }

    // Make everything zero.
    // Also creates if it doesnt exist in the first place.
    public void ClearAllScores()
    {
        print("CLEARING ALL SCORES");

        for (int _num = 0; _num < _highscore.Length; ++_num)
        {
            PlayerPrefs.SetInt("highscore" + _num.ToString(), 0);
            _highscore[_num] = 0;
        }
    }

    public int GetCurrScore()
    {
        return PlayerPrefs.GetInt("currentscore");
    }

    // Just pass in a negative if reducing
    public void AddScore(float score)
    {
        // Does it round up or down?
        // hope its consistent.
        _currScore += (int)(score * _multiplier);
        DisplayScore();
    }

    //public float GetMultiplier()
    //{
    //    return _multiplier;
    //}

    public void SetMultiplier(float multiplier)
    {
        _multiplier = multiplier;
        DisplayScore();
    }

    public void ResetMultiplier()
    {
        _multiplier = _defaultMulti;
        _feedback.GetComponent<Feedback>().CreateAudio("Damage");
        DisplayScore();
    }

    public void AddMultiplier(float increase)
    {
        _multiplier += increase;
        DisplayScore();
    }

    void DisplayScore()
    {
        _multiplier = Mathf.Round(_multiplier * 10.0f) / 10.0f;

        _text.text = "Score:" + _currScore.ToString() + "\n" + "Multiplier:" + _multiplier.ToString();
    }

    void DisplayHighScore()
    {
        _text.text = "List of currentscore ";
        for (int _num = 0; _num < _highscore.Length; ++_num)
        {
            print(GetCurrScore());
            _text.text += "\n" + PlayerPrefs.GetInt("highscore" + _num.ToString()).ToString();
        }
    }

    public void SetDefaultMultiplier(float _DM)
    {
        _defaultMulti = _DM;
        _multiplier = _defaultMulti;
    }

    // Call this when saving score after boss/death
    public void SaveScore()
    {
        for (int _num = (_highscore.Length - 1); _num >= 0; --_num)
        {
            if (_currScore > _highscore[_num])
            {
                if (_num == 0)
                {
                    // Shifting highscore buffer
                    int[] tempScoreN = new int[_highscore.Length];

                    for (int i = 0; i < (_highscore.Length - 1); ++i)
                    {
                        tempScoreN[i + 1] = _highscore[i];
                    }

                    tempScoreN[0] = _currScore;

                    for (int i = 0; i < _highscore.Length; ++i)
                    {
                        _highscore[i] = tempScoreN[i];
                    }

                    break;
                }
                continue;
            }
            if (_num + 1 > 9)
                break;

            // Shifting highscore buffer
            int[] tempScore = new int[_highscore.Length];

            for (int i = 0; i < (_highscore.Length - 1); ++i)
            {
                tempScore[i + 1] = _highscore[i];
            }

            for (int i = (_num + 2); i < _highscore.Length; ++i)
            {
                _highscore[i] = tempScore[i];
            }

            _highscore[_num + 1] = _currScore;

            break;
        }

        for (int _num = 0; _num < _highscore.Length; ++_num)
        {
            //print(_highscore[_num]);
            PlayerPrefs.SetInt("highscore" + _num.ToString(), _highscore[_num]);
        }

        PlayerPrefs.SetInt("currentscore", _currScore);

        _currScore = 0;
        _multiplier = _defaultMulti;
    }
}
