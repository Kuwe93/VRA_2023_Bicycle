using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using MixedReality.Toolkit.UX;
using TMPro;
using System;

public enum Difficulty { NoHelp = 0, SmallHelp = 1, BigHelp = 2 };

public struct SavedBicyclePosition
{
    public int bicyclePartArrayPos;
    public Vector3 trainingPosition;

    //Constructor
    public SavedBicyclePosition(int pos, Vector3 tPosition)
    {
        this.bicyclePartArrayPos = pos;
        this.trainingPosition = tPosition;
    }
}

public class SceneController : MonoBehaviour
{
    private List<Difficulty> sceneDifficulty;
    public PressableButton noHelpToggle;

    public HelpScreen helpscreencontroller;

    public PressableButton[] missionToggles;

    public List<GameObject> bicycleParts;
    public List<GameObject> bicyclePartsBig;

    public Transform[] bicycleTransforms;
    public Transform[] bicycleTransformsBig;
    private List<Vector3> startPositions;
    private List<Vector3> startPositionsBig;
    private List<SavedBicyclePosition> savedPositions;
    private List<SavedBicyclePosition> savedPositionsBig;

    public List<XRSocketInteractor> ReifensockelChecks;
    public List<XRSocketInteractor> BremsensockelChecks; //6Sockel 
    public List<XRSocketInteractor> LeitungensockelChecks;
    public List<XRSocketInteractor> RestsockelChecks;

    public XRSocketInteractor RahmenCheck;
    public XRSocketInteractor PedaleCheck;
    public XRSocketInteractor SattelCheck;

    private bool timerActive;
    private float currentTimeInTimer;
    public TMP_Text textfield;

    // Start is called before the first frame update
    void Start()
    {
        savedPositions = new List<SavedBicyclePosition>();
        savedPositionsBig = new List<SavedBicyclePosition>();
        startPositions = new List<Vector3>();
        foreach(var obj in bicycleTransforms)
        {
            startPositions.Add(new Vector3(obj.position.x, obj.position.y, obj.position.z));
        }
        startPositionsBig = new List<Vector3>();
        foreach (var obj in bicycleTransformsBig)
        {
            startPositionsBig.Add(new Vector3(obj.position.x, obj.position.y, obj.position.z));
        }
        currentTimeInTimer = 0;
        sceneDifficulty = new List<Difficulty>();
        this.AddDifficulty(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSockel();
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

        if (sceneDifficulty.Contains(Difficulty.BigHelp))
        {
            helpscreencontroller.ToggleState(true);
        } else
        {
            helpscreencontroller.ToggleState(false);
        }

        //object check and back transport if it falls outside the room
        /*foreach(var obj in bicycleParts) {
            if(obj.transform.position.y <= -1)
            {
                obj.transform.position = obj.transform.position + new Vector3(0, 2, 0);
            }
            if (obj.transform.position.z <= -5.2)
            {
                obj.transform.position = obj.transform.position + new Vector3(0, 0, 2);
            }
            if (obj.transform.position.z >= 0.65)
            {
                obj.transform.position = obj.transform.position + new Vector3(0, 0, -2);
            }
            if (obj.transform.position.x <= -7.25)
            {
                obj.transform.position = obj.transform.position + new Vector3(2, 0, 0);
            }
            if (obj.transform.position.x >= 7.25)
            {
                obj.transform.position = obj.transform.position + new Vector3(-2, 0, 0);
            }
        }*/
        /*foreach(var obj in bicyclePartsBig) {
            if (obj.transform.position.y <= -1)
            {
                obj.transform.position = obj.transform.position + new Vector3(0, 2, 0);
            }
            if (obj.transform.position.z <= -5.2)
            {
                obj.transform.position = obj.transform.position + new Vector3(0, 0, 2);
            }
            if (obj.transform.position.z >= 0.65)
            {
                obj.transform.position = obj.transform.position + new Vector3(0, 0, -2);
            }
            if (obj.transform.position.x <= -7.25)
            {
                obj.transform.position = obj.transform.position + new Vector3(2, 0, 0);
            }
            if (obj.transform.position.x >= 7.25)
            {
                obj.transform.position = obj.transform.position + new Vector3(-2, 0, 0);
            }
        }*/

        //test if saved transforms change after moving the actual objectw
        //Debug.Log("Transform 0 is:" + startPositions[0].x + "," + startPositions[0].y + "," + startPositions[0].z);

        /*foreach (var diff in sceneDifficulty)
        {
            int c = 0;
            Debug.Log(c+": Actual Difficulty: " + diff);
            c++;
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
                    //Debug.Log("Entferne Difficulty BigHelp.");
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
        savedPositionsBig.Clear();
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
        for (int x = 0; x < bicycleParts.Count; x++)
        {
            foreach (var pos in savedPositions)
            {
                if (pos.bicyclePartArrayPos == x)
                {
                    bicycleParts[pos.bicyclePartArrayPos].transform.position = pos.trainingPosition;
                }
            }
        }

        for (int x = 0; x < bicyclePartsBig.Count; x++)
        {
            foreach (var pos in savedPositionsBig)
            {
                if (pos.bicyclePartArrayPos == x)
                {
                    bicyclePartsBig[pos.bicyclePartArrayPos].transform.position = pos.trainingPosition;
                    //bicyclePartsBig[bicyclepart.bicyclePartArrayPos].transform.rotation = bicyclepart.trainingPosition.rotation;
                    //bicyclePartsBig[bicyclepart.bicyclePartArrayPos].transform.localScale = bicyclepart.trainingPosition.localScale;
                }
            }
        }
    }

    public void RandomizeObjectPositions()
    {
        List<Vector3> tempTrans = new List<Vector3>();
        foreach(var pos in startPositions)
        {
            Vector3 tempV3 = new Vector3(pos.x, pos.y + 0.25f, pos.z);
            tempTrans.Add(tempV3);
        }
        // for each bicyclepart get a random position from the startposition array
        for(int x = 0;x < bicycleParts.Count; x++)
        {
            int tempnr = UnityEngine.Random.Range(0, tempTrans.Count);
            SavedBicyclePosition trainingsItem = new SavedBicyclePosition(x, tempTrans[tempnr]);
            savedPositions.Add(trainingsItem);
            tempTrans.Remove(tempTrans[tempnr]);
        }

        List<Vector3> tempTransBig = new List<Vector3>();
        foreach (var pos in startPositionsBig)
        {
            Vector3 tempV3 = new Vector3(pos.x, pos.y + 0.25f, pos.z);
            tempTransBig.Add(tempV3);
        }
        // for each bicyclepart get a random position from the startposition array
        for (int x = 0; x < bicyclePartsBig.Count; x++)
        {
            int tempnr = UnityEngine.Random.Range(0, tempTransBig.Count);
            //Debug.Log("x = "+x+" und TempNR = "+tempnr);
            SavedBicyclePosition trainingsItem = new SavedBicyclePosition(x, tempTransBig[tempnr]);
            savedPositionsBig.Add(trainingsItem);
            tempTransBig.Remove(tempTransBig[tempnr]);
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
                    bicycleParts[x].transform.position = pos.trainingPosition;
                    //bicycleParts[x].transform.rotation = pos.trainingPosition.rotation;
                    //bicycleParts[x].transform.localScale = pos.trainingPosition.localScale;
                }
            }
            
        }
        for (int x = 0; x < bicyclePartsBig.Count; x++)
        {
            foreach (var pos in savedPositionsBig)
            {
                if (pos.bicyclePartArrayPos == x)
                {
                    bicyclePartsBig[x].transform.position = pos.trainingPosition;
                    //bicyclePartsBig[x].transform.rotation = pos.trainingPosition.rotation;
                    //bicyclePartsBig[x].transform.localScale = pos.trainingPosition.localScale;
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

    private void CheckSockel()
    {
        if (RahmenCheck.hasSelection)
        {
            missionToggles[0].ForceSetToggled(true, false);
        }
        else
        {
            missionToggles[0].ForceSetToggled(false, false);
        }

        if (PedaleCheck.hasSelection)
        {
            missionToggles[6].ForceSetToggled(true, false);
        }
        else
        {
            missionToggles[6].ForceSetToggled(false, false);
        }

        if (SattelCheck.hasSelection)
        {
            missionToggles[4].ForceSetToggled(true, false);
        }
        else
        {
            missionToggles[4].ForceSetToggled(false, false);
        }
        bool allChecked = false;
        bool check1 = true;
        bool check2 = true;
        bool check3 = true;
        bool check4 = true;
        foreach (var obj in ReifensockelChecks)
        {
            if (!obj.hasSelection) {
                check1 = false;
            }
        }
        if (check1)
        {
            missionToggles[1].ForceSetToggled(true, false);
        }
        else
        {
            missionToggles[1].ForceSetToggled(false, false);
        }
        foreach (var obj in BremsensockelChecks)
        {
            if (!obj.hasSelection)
            {
                check2 = false;
            }
        }
        if (check2)
        {
            missionToggles[2].ForceSetToggled(true, false);
        }
        else
        {
            missionToggles[2].ForceSetToggled(false, false);
        }
        foreach (var obj in LeitungensockelChecks)
        {
            if (!obj.hasSelection)
            {
                check3 = false;
            }
        }
        if (check3)
        {
            missionToggles[3].ForceSetToggled(true, false);
        }
        else
        {
            missionToggles[3].ForceSetToggled(false, false);
        }
        foreach (var obj in RestsockelChecks)
        {
            if (!obj.hasSelection)
            {
                check4 = false;
            }
        }
        if (check4)
        {
            missionToggles[5].ForceSetToggled(true, false);
        } else
        {
            missionToggles[5].ForceSetToggled(false, false);
        }

        if (check1 && check2 && check3 && check4 && RahmenCheck.hasSelection && PedaleCheck.hasSelection && SattelCheck.hasSelection)
        {
            allChecked = true;
        }

        if (allChecked)
        {
            StopTimer();
        }
    }
}
