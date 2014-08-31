using UnityEngine;
using System.Collections;

public class MenuSystem : MonoBehaviour
{
    public void SelectOption(string btnName)//Switch que depende do ActualMenuState
    {
        string[] name = btnName.Split("_"[0]);

        switch (name[1])
        { 
            case "Start":
                SetupGameStart();
                break;

            case "Options":
                break;

            case "Credits":
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

}
