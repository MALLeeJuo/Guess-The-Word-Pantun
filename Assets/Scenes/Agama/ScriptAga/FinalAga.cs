using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class FinalAga : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    private float elapsedTime;
    private int score = 0;
    private TimeSpan timePlaying;




    // Loader playerpref XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
        }
        if (PlayerPrefs.HasKey("Timer"))
        {
            elapsedTime = PlayerPrefs.GetFloat("Timer");
        }
    }

    public void ForceLoadGame()
    {
        LoadGame();
    }

    //End Loader Game XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    void Start()
    {
        LoadGame();
        scoreText.text = "Score: " + score;
        elapsedTime += Time.deltaTime;
        timePlaying = TimeSpan.FromSeconds(elapsedTime);
        string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss");
        timerText.text = timePlayingStr;
        //timerText.text = " " + elapsedTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
