using UnityEngine;
using System.Collections;

public class MenuSystem : MonoBehaviour
{

    public GameObject mainAnchor;
    public GameObject creditsAnchor;
    public GameObject optionsAnchor;

    public void SelectOption(string btnName)//Switch que depende do ActualMenuState
    {
        string[] name = btnName.Split("_"[0]);

        switch (name[1])
        { 
            case "Start":
                SetupGameStart();
                break;

            case "Options":
                ShowOptions();
                break;

            case "Credits":
                ShowCredits();
                break;  
           
            case "Back":
                ShowMain();
                break;
        }
    }

    void SetupGameStart()
    {
        StartGame();
    }

    void StartGame()
    {
        CameraFade.StartAlphaFade(Color.black, false, 2, 0, () => { Application.LoadLevel("Stage_HUB"); });
    }

    void ShowCredits()
    {
        iTween.RotateTo(mainAnchor, iTween.Hash("y", 90, "time", 3));
        iTween.RotateTo(creditsAnchor, iTween.Hash("y", 0, "time", 3));
    }

    void ShowOptions()
    {
        iTween.RotateTo(mainAnchor, iTween.Hash("y", -90, "time", 3));
        iTween.RotateTo(optionsAnchor, iTween.Hash("y", 0, "time", 3));
    }

    void ShowMain()
    {
        iTween.RotateTo(mainAnchor, iTween.Hash("y", 0, "time", 3));
        iTween.RotateTo(creditsAnchor, iTween.Hash("y", -90, "time", 3));
        iTween.RotateTo(optionsAnchor, iTween.Hash("y", 90, "time", 3));
    
    }

}
