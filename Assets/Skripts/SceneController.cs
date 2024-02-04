using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
using TMPro;
using System;

public enum Difficulty { NoHelp = 0, SmallHelp = 1, BigHelp = 2 };

public struct SavedBicyclePosition
{
    public int bicyclePartArrayPos;
    public Transform trainingPosition;

    //Constructor
    public SavedBicyclePosition(int pos, Transform tPosition)
    {
        this.bicyclePartArrayPos = pos;
        this.trainingPosition = tPosition;
    }
}

public class SceneController : MonoBehaviour
{
    private List<Difficulty> sceneDifficulty;
    public PressableButton noHelpToggle;

    public PressableButton[] missionToggles;

    public List<GameObject> bicycleParts;

    public Transform[] bicycleTransforms;
    private Transform[] startPositions;
    private List<SavedBicyclePosition> savedPositions;

    private bool timerActive;
    private float currentTimeInTimer;
    public TMP_Text textfield;

    // Start is called before the first frame update
    void Start()
    {
        savedPositions = new List<SavedBicyclePosition>();
        startPositions = bicycleTransforms;
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

        //test if saved transforms change after moving the actual objectw
        Debug.Log("Transform 0 is:" + savedPositions[0].trainingPosition.position.x + "," + savedPositions[0].trainingPosition.position.y + "," + savedPositions[0].trainingPosition.position.z);

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
        //clear savedPositions
        savedPositions.Clear();
        //ordne die Fahrradteile Zufällig neu an 
        RandomizeObjectPositions();
        //setze die Aufgaben Toggles zurück
        ResetToggles();
    }

    public void resetTraining()
    {
        //timer zurücksetzen
        StopTimer();
        ResetTimer();
        //Aufgaben Toggles zurück setzen
        ResetToggles();
        //Fahrradteile auf startposition zurücksetzen
        ResetObjectPositions();
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

    private void ResetObjectPositions()
    {
        foreach (var bicyclepart in savedPositions)
        {
            bicycleParts[bicyclepart.bicyclePartArrayPos].transform.position = bicyclepart.trainingPosition.position;
            bicycleParts[bicyclepart.bicyclePartArrayPos].transform.rotation = bicyclepart.trainingPosition.rotation;
            bicycleParts[bicyclepart.bicyclePartArrayPos].transform.localScale = bicyclepart.trainingPosition.localScale;
        }
    }

    public void RandomizeObjectPositions()
    {
        List<Transform> tempTrans = new List<Transform>();
        foreach(var pos in startPositions)
        {
            tempTrans.Add(pos);
        }
        // for each bicyclepart get a random position from the startposition array
        for(int x = 0;x < bicycleParts.Count; x++)
        {
            int tempnr = UnityEngine.Random.Range(0, tempTrans.Count);
            SavedBicyclePosition trainingsItem = new SavedBicyclePosition(x, tempTrans[tempnr]);
            tempTrans.Remove(tempTrans[tempnr]);
            savedPositions.Add(trainingsItem);
        }

        PositionObjects();
    }

    private void PositionObjects()
    {
        for (int x = 0; x < bicycleParts.Count; x++)
        {
            foreach(var pos in savedPositions)
            {
                if(pos.bicyclePartArrayPos == x)
                {
                    bicycleParts[x].transform.position = pos.trainingPosition.position;
                    bicycleParts[x].transform.rotation = pos.trainingPosition.rotation;
                    bicycleParts[x].transform.localScale = pos.trainingPosition.localScale;
                }
            }
            
        }
    }

    private void ResetToggles()
    {
        foreach(var toggle in missionToggles)
        {
            toggle.ForceSetToggled(false, false);
        }
    }
}
