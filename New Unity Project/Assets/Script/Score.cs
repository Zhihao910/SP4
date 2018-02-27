using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // 10 highest scores
    private int[] _highscore = new int[10];

    // Current Score
    private int _currScore = 0;
    // Multiplier
    private float _multiplier = 1.0f;

    private GameObject _feedback;

	// Use this for initialization
	void Start ()
    {
        // Does it exist? If yes, read and input into _highscore
        if (PlayerPrefs.HasKey("highscore0"))
        {
            for (int _num = 0; _num < _highscore.Length; ++_num)
            {
                print(_num);
                _highscore[_num] = PlayerPrefs.GetInt("highscore" + _num.ToString());
            }
        }
        else
        {
            // If no saved score exists (really?)
            // Create highscores, set all to zero
            ClearAllScores();
        }

        _feedback = GameObject.FindGameObjectWithTag("Feedback");
    }

    // Make everything zero.
    // Also creates if it doesnt exist in the first place.
    private void ClearAllScores()
    {
        for (int _num = 0; _num < _highscore.Length; ++_num)
        {
            PlayerPrefs.SetFloat("highscore" + _num.ToString(), 0);
            _highscore[_num] = 0;
        }
    }

    public int GetCurrScore()
    {
        return _currScore;
    }

    // Just pass in a negative if reducing
    public void AddScore(float score)
    {
        // Does it round up or down?
        // hope its consistent.
        _currScore += (int)(score * _multiplier);
    }

    public float GetMultiplier()
    {
        return _multiplier;
    }

    public void SetMultiplier(float multiplier)
    {
        _multiplier = multiplier;
    }

    public void ResetMultiplier()
    {
        _multiplier = 1.0f;
        _feedback.GetComponent<Feedback>().CreateAudio("Damage");
    }

    public void AddMultiplier(float increase)
    {
        _multiplier += increase;
    }
}
