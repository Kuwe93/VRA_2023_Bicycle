using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
using TMPro;
using System;

public enum Difficulty { NoHelp = 0, SmallHelp = 1, BigHelp = 2 };

public class SceneController : MonoBehaviour
{
    private List<Difficulty> sceneDifficulty;
    public PressableButton noHelpToggle;

    public PressableButton[] missionToggles;

    private bool timerActive;
    private float currentTimeInTimer;
    public TMP_Text textfield;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeInTimer = 0;
        sceneDifficulty = new List<Difficulty>();
        this.AddDifficulty(0);
    }

    // Update is called once per frame
    void Update()
    {
        //update Time of Stopwatch
        if (timerActive)
        {
            currentTimeInTimer = currentTimeInTimer + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTimeInTimer);

        textfield.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();

        if(sceneDifficulty.Count < 1)
        {
            this.AddDifficulty(0);
            noHelpToggle.ForceSetToggled(true, false);
        }
        /*foreach (var diff in sceneDifficulty)
        {
            Debug.Log("Actual Difficulty: " + diff);
        }*/
    }

    public void AddDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                if (!SearchDifficulty(Difficulty.NoHelp))
                {
                    sceneDifficulty.Add(Difficulty.NoHelp);
                }                
                break;
            case 1:
                if (!SearchDifficulty(Difficulty.SmallHelp))
                {
                    sceneDifficulty.Add(Difficulty.SmallHelp);
                }
                break;
            case 2:
                if (!SearchDifficulty(Difficulty.BigHelp))
                {
                    sceneDifficulty.Add(Difficulty.BigHelp);
                }
                break;
            default:
                print("Incorrect difficulty level.");
                break;
        }
        
    }

    private bool SearchDifficulty(Difficulty difficulty)
    {
        bool isFound = false;
        foreach (var diff in sceneDifficulty)
        {
            if (diff == difficulty)
            {
                isFound = true;
                break;
            }
        }
        return isFound;
    }

    public int GetDifficultyNumber()
    {
        return sceneDifficulty.Count;
    }

    public Difficulty GetDifficultyByNumber(int nr)
    {
        return sceneDifficulty[nr];
    }

    public void RemoveDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                if (SearchDifficulty(Difficulty.NoHelp))
                {
                    sceneDifficulty.Remove(Difficulty.NoHelp);
                }
                break;
            case 1:
                if (SearchDifficulty(Difficulty.SmallHelp))
                {
                    sceneDifficulty.Remove(Difficulty.SmallHelp);
                }
                break;
            case 2:
                if (SearchDifficulty(Difficulty.BigHelp))
                {
                    sceneDifficulty.Remove(Difficulty.BigHelp);
                }
                break;
            default:
                print("Incorrect difficulty level.");
                break;
        }
    }

    public void StartTraining()
    {
        
        //starte Timer
        StartTimer();
        //ordne die Fahrradteile Zufällig neu an 
        //setze die Aufgaben Toggles zurück
        
    }

    public void resetTraining()
    {
        //timer zurücksetzen
        StopTimer();
        ResetTimer();
        //Aufgaben Toggles zurück setzen
        //Fahrradteile auf startposition zurücksetzen
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public void ResetTimer()
    {
        currentTimeInTimer = 0.0f;
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
