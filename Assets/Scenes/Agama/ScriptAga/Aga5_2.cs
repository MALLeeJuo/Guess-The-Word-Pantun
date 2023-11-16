using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class Aga5_2 : MonoBehaviour //please change here
{
    public TMP_Text wordDisplay2;
    public TMP_Text feedbackText;
    public TMP_Text lastScoreLose;
    public TMP_Text remainingAttemptsText;
    public TMP_Text hintCountText;
    public TMP_Text scoreText;
    public TMP_Text timeCounter;
    [Header("Fulfill the buttons to fix Hint issues")]
    [Header("Buttons are assign in object not here")]
    public Button[] letterButtons;
    public Button hintButton;
    [Header("Please sort = Win, Lose , QuitPrompt")]
    public GameObject Panel;
    public GameObject Panel2;
    public GameObject Panel3;

    private string[] wordBank = { "amalan" }; //please change here
    private bool[] letterUsedAsHint;
    private bool timerGoing;
    private string currentWord;
    private string hiddenWord;
    private int maxAttempts = 3;
    private int remainingAttempts;
    private int hintCount = 3;
    private int score = 0;
    private float elapsedTime;
    private List<char> correctGuesses = new List<char>();
    private List<char> incorrectGuesses = new List<char>();
    private TimeSpan timePlaying;

    // Save&Load playerpref
    private void SaveGame()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("HintCount", hintCount);
        PlayerPrefs.SetFloat("Timer", elapsedTime);
        PlayerPrefs.SetInt("RemainingAttempts", remainingAttempts);
        PlayerPrefs.Save();
    }

    public void ForceSaveGame()
    {
        SaveGame();
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
        }
        if (PlayerPrefs.HasKey("HintCount"))
        {
            hintCount = PlayerPrefs.GetInt("HintCount");
            //double script on
        }
        if (PlayerPrefs.HasKey("Timer"))
        {
            elapsedTime = PlayerPrefs.GetFloat("Timer");
        }
        if (PlayerPrefs.HasKey("RemainingAttempts"))
        {
            remainingAttempts = PlayerPrefs.GetInt("RemainingAttempts");
        }
    }

    public void ForceLoadGame()
    {
        Debug.Log("Reloading...");
        LoadGame();
    }
    //End Save&Load playerpref

    private void Start()
    {
        LoadGame();
        Debug.Log("Aga5_2 Script loaded"); //please change here
        ChooseWord();
        UpdateUI();

        timeCounter.text = "Time: 00:00";
        timerGoing = false;

        BeginTimer();
    }

    public void BeginTimer()
    {
        timerGoing = true;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }

    private void ChooseWord()
    {
        currentWord = wordBank[UnityEngine.Random.Range(0, wordBank.Length)].ToLower();
        hiddenWord = new string('_', currentWord.Length);
        remainingAttempts = maxAttempts;
        correctGuesses.Clear();
        incorrectGuesses.Clear();
        feedbackText.text = "";
        hintCountText.text = "Hints: " + hintCount;
        scoreText.text = "Score: " + score;
        lastScoreLose.text = "Your Last score: " + score;
        letterUsedAsHint = new bool[26];

        for (int i = 0; i < letterUsedAsHint.Length; i++)
        {
            letterUsedAsHint[i] = false;
        }
    }

    private void UpdateUI()
    {
        wordDisplay2.text = FormathiddenWord();
        remainingAttemptsText.text = "Remaining Attempts: " + remainingAttempts;
    }

    private string FormathiddenWord()
    {
        return string.Join(" ", hiddenWord.ToCharArray());
    }

    public void MakeGuess(char letter)
    {
        if (currentWord.Contains(letter.ToString()))
        {
            correctGuesses.Add(letter);
            UpdatehiddenWord();
            feedbackText.text = "Good guess!";
        }
        else
        {
            incorrectGuesses.Add(letter);
            feedbackText.text = "Try again!";

            if (score != 0)
            {
                Debug.Log("score != 0 checked");
                score -= 20;
                scoreText.text = "Score: " + score;
            }            
            else if (score == 0)
            {
                Debug.Log("score == 0 checked");
                score -= 10;
                scoreText.text = "Score: " + score;
            }
            
            remainingAttempts--;
        }

        if (remainingAttempts <= 0)
        {
            lastScoreLose.text = "Your Last score: " + score;
            OpenPanelLose();
        }


        if (IsWordComplete())
        {
            OpenPanelWin();
        }

        UpdateUI();
    }

    private void UpdatehiddenWord()
    {
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (correctGuesses.Contains(currentWord[i]))
            {
                hiddenWord = hiddenWord.Remove(i, 1).Insert(i, currentWord[i].ToString());
            }
        }
    }

    private bool IsWordComplete()
    {
        if (hiddenWord == currentWord)
        {
            // empty
            return true;
        }
        return false;
    }

    public void UseHint()
    {
        if (hintCount > 0)
        {
            int index = hiddenWord.IndexOf('_');
            if (index != -1)
            {
                char hintLetter = currentWord[index];
                hintCount--;
                hintCountText.text = "Hints: " + hintCount;
                MakeGuess(hintLetter);

                int letterIndex = hintLetter - 'a';
                if (letterIndex >= 0 && letterIndex < letterUsedAsHint.Length)
                {
                    letterUsedAsHint[letterIndex] = true;
                    letterButtons[letterIndex].interactable = false;
                    letterButtons[letterIndex].image.color = Color.black;
                }
            }
        }
    }

    public void OnLetterButtonClick(Button button)
    {
        char letter = button.GetComponentInChildren<Text>().text.ToLower()[0];
        int letterIndex = letter - 'a';

        if (!correctGuesses.Contains(letter) && !incorrectGuesses.Contains(letter) && letterUsedAsHint[letterIndex] == false)
        {
            MakeGuess(letter);
            button.interactable = false;

            if (letterIndex >= 0 && letterIndex < letterUsedAsHint.Length)
            {
                letterUsedAsHint[letterIndex] = true;
                button.image.color = Color.gray;
                score += 10;   
                scoreText.text = "Score: " + score;
                lastScoreLose.text = "Your Last score: " + score;
            }
        }
    }

    public void OnHintButtonClick()
    {
        UseHint();
        hintButton.interactable = hintCount > 0;
    }

    //Open Panel Function (can really combine but im lazy)

    public void OpenPanelWin()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    public void OpenPanelLose()
    {
        if (Panel2 != null)
        {
            Panel2.SetActive(true);
        }
    }

    public void OpenPanelQuit()
    {
        if (Panel3 != null)
        {
            Panel3.SetActive(true);
        }
    }

    //Debug menu for testers

    public void giveScoredebug()
    {
        score += 100;
        scoreText.text = "Score: " + score;
    }

    public void skipScenedebug(string sceneName)
    {
        ForceSaveGame();
        SceneManager.LoadScene(sceneName);
    }

    public void addHintdebug()
    {
        hintCount += 1;
        hintCountText.text = "Hints: " + hintCount;
    }

    public void addRemainAttempts()
    {
        remainingAttempts +=  1;
        remainingAttemptsText.text = "Remaining Attempts: " + remainingAttempts;
    }
}
