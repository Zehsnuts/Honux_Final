using UnityEngine;
using System.Collections;

public class Screen_Confirmation : MonoBehaviour 
{

    public GameObject retryLevelConfirmationScreen;
    public GameObject exitGameConfirmationScreen;
    public GameObject levelSelectConfirmationScreen;


    void CheckIfNull()
    {
        if (retryLevelConfirmationScreen == null ||
           exitGameConfirmationScreen == null )
        {
            retryLevelConfirmationScreen = transform.FindChild("RetryLevel").gameObject;
            exitGameConfirmationScreen = transform.FindChild("ExitGame").gameObject;
            levelSelectConfirmationScreen = transform.FindChild("ExitToLevelSelect").gameObject;
        }
    }

    public void ConfirmRetryLevel()
    {
        CheckIfNull();

        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        retryLevelConfirmationScreen.SetActive(true);
    }

    public void ConfirmExitGame()
    {
        CheckIfNull();

        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        exitGameConfirmationScreen.SetActive(true);
    }

    public void ConfirmExitToLevelSelect()
    {
        CheckIfNull();

        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        levelSelectConfirmationScreen.SetActive(true);
    }
}
