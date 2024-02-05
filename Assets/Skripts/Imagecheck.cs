using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.UI;

public class Imagecheck : MonoBehaviour
{
    public List<XRSocketInteractor> SockelToCheck;
    private List<bool> isSet;
    private bool isActive = false;
    public HelpScreen myController;
    //public Image myImage;
    public string desc;
    private ImageForBicyclePart img;

    // Start is called before the first frame update
    void Start()
    {
        isSet = new List<bool>();
        img = new ImageForBicyclePart(desc, this.GetComponent<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        for(int x = 0; x<SockelToCheck.Count;x++)
        {
            if (SockelToCheck[x].enabled)
            {
                if (SockelToCheck[x].hasSelection)
                {
                    if(isSet.Count > 0)
                    {
                        isSet.RemoveAt(0);
                    }
                } else {
                    if (isSet.Count < SockelToCheck.Count)
                    {
                        isSet.Add(false);
                    }
                }
            } 
        }

        if(isSet.Count > 0)
        {
            isActive = true;
        } else
        {
            isActive = false;
        }

        if (isActive)
        {
            myController.AddActiveImage(img);
        } else
        {
            if (myController.CheckIfContains(img))
            {
                myController.RemActiveImage(img);
            }
        }
    }
}
