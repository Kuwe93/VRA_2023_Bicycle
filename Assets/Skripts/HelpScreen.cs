using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public struct ImageForBicyclePart
{
    public string bicyclePartName;
    public Image bicycleImage;

    //Constructor
    public ImageForBicyclePart(string desc, Image img)
    {
        this.bicyclePartName = desc;
        this.bicycleImage = img;
    }
}

public class HelpScreen : MonoBehaviour
{
    public Difficulty thisDifficulty;
    public SceneController sceneController;
    //public Image[] imagesInScene;
    private List<ImageForBicyclePart> activeImages;
    private ImageForBicyclePart activeImg;
    

    public TMP_Text textfield;
    public GameObject nextImg;
    public GameObject prevImg;

    public Image startImg;
    public string startString = "Rahmen";

    // Start is called before the first frame update
    void Start()
    {
        activeImages = new List<ImageForBicyclePart>();
        AddActiveImage(new ImageForBicyclePart(startString, startImg));
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach(var obj in imagesInScene)
        {
            if (obj.enabled)
            {
                activeImages.Add(obj);
            } else
            {
                activeImages.Remove(obj);
            }
        }*/

        if(activeImages.Count == 1)
        {
            activeImg = activeImages[0];
            activeImg.bicycleImage.enabled = true;
            textfield.text = "Aktuell gesucht: " + activeImg.bicyclePartName;
        }

        if (activeImages.Count == 0)
        {
            textfield.text = "Alle Teile eingebaut.";
        }

        if (activeImages.Count > 1)
        {
            //Debug.Log("Aktuell sind " + activeImages.Count + "Imgs aktiv.");
            if (!nextImg.activeSelf)
            {
                nextImg.SetActive(true);
                prevImg.SetActive(true);
            }
            
        } else
        {
            if (nextImg.activeSelf)
            {
                nextImg.SetActive(false);
                prevImg.SetActive(false);
            }
        }

        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(Vector3.up, 180);
    }

    public void ToggleState(bool value)
    {
        int actualDifficultys = sceneController.GetDifficultyNumber();

        for (int x = 0; x < actualDifficultys; x++)
        {
            Difficulty diff = sceneController.GetDifficultyByNumber(x);
            if (thisDifficulty == diff)
            {
                //Debug.Log("Setze den Status des GameObjects.");
                this.gameObject.SetActive(value);
                break;
            } else
            {
                this.gameObject.SetActive(value);
                break;
            }

        }

    }

    public void AddActiveImage(ImageForBicyclePart obj)
    {
        if (!activeImages.Contains(obj))
        {
            Debug.Log("HELPSCREEN:Es wurde ein Img hinzugefügt.");
            activeImages.Add(obj);
        }
    }

    public void RemActiveImage(ImageForBicyclePart obj)
    {
        if (activeImages.Contains(obj))
        {
            Debug.Log("HELPSCREEN:Es wurde ein Img entfernt.");
            activeImages.Remove(obj);
        }
        
    }

    public bool CheckIfContains(ImageForBicyclePart objToCheck)
    {
        bool check = false;
        foreach(var obj in activeImages)
        {
            if(obj.bicyclePartName.Equals(objToCheck.bicyclePartName))
            {
                check = true;
            }
        }
        return check;
    }

    private int searchImg(ImageForBicyclePart currentImg)
    {
        return activeImages.IndexOf(currentImg);
    }

    public void ShowNextImg()
    {
        activeImg.bicycleImage.enabled = false;
        textfield.text = "Aktuell gesucht: ";
        int newIndex = searchImg(activeImg) + 1;
        if (newIndex >= activeImages.Count)
        {
            activeImg = activeImages[0];
        } else
        {
            activeImg = activeImages[newIndex];
        }
        activeImg.bicycleImage.enabled = true;
        textfield.text = "Aktuell gesucht: " + activeImg.bicyclePartName;
    }

    public void ShowPrevImg()
    {
        activeImg.bicycleImage.enabled = false;
        textfield.text = "Aktuell gesucht: ";
        int newIndex = searchImg(activeImg) - 1;
        if (newIndex < 0)
        {
            activeImg = activeImages[activeImages.Count - 1];
        }
        else
        {
            activeImg = activeImages[newIndex];
        }
        activeImg.bicycleImage.enabled = true;
        textfield.text = "Aktuell gesucht: " + activeImg.bicyclePartName;
    }
}
