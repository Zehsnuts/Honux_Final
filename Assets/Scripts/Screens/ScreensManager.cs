using UnityEngine;
using System.Collections;

public class ScreensManager : MonoBehaviour
{

    #region Events
    void OnEnable()
    {
        EventManager.GAMEPAUSESTART += CallPauseScreen;
        EventManager.GAMEPAUSEEXIT += CloseScreens;
        EventManager.BLUEPRINTEXIT += CloseScreens;
        EventManager.AUDIOSCREENEXIT += CloseScreens;

        EventManager.STAGESUCESS += CallWinFeedbackScreen;
        EventManager.STAGEFAIL += CallLoseFeedbackScreen;
    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= CallPauseScreen;
        EventManager.GAMEPAUSEEXIT -= CloseScreens;
        EventManager.BLUEPRINTEXIT -= CloseScreens;
        EventManager.AUDIOSCREENEXIT -= CloseScreens;

        EventManager.STAGESUCESS -= CallWinFeedbackScreen;
        EventManager.STAGEFAIL -= CallLoseFeedbackScreen;
    }
    #endregion

    public GameObject configurationScreen;
    public GameObject bluePrintScreen;
    public GameObject audioListScreen;
    public GameObject pauseScreen;
    public GameObject feedbackScreen;
    public GameObject confirmationScreen;
    public GameObject blockingBackGround;
    
    // Use this for initialization
	void Start () 
    {
        gameObject.SetActive(true);

        if (configurationScreen == null ||
            bluePrintScreen == null ||
            audioListScreen == null ||
            pauseScreen == null ||
            feedbackScreen == null ||
            blockingBackGround == null)
        {
            configurationScreen = transform.FindChild("Configuration").gameObject;
            bluePrintScreen = transform.FindChild("Blueprint").gameObject;
            audioListScreen = transform.FindChild("Audio").gameObject;
            pauseScreen = transform.FindChild("Pause").gameObject;
            feedbackScreen = transform.FindChild("Feedback").gameObject;
            blockingBackGround = transform.FindChild("BlockingBG").gameObject;
            confirmationScreen = transform.FindChild("Confirmation").gameObject;
        }

        CloseScreens();
	}
	
	// Update is called once per frame
	public void OpenScreenByButtonName(Transform name) 
    {

        if (GameStateManager.INSTANCE.SpecialGameState == GameStateManager.SpecialGameStates.OnTutorial)
            return;

        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        Debug.Log(name);

        string[] nameSplit = name.name.Split("_"[0]);

        switch (nameSplit[1])
        {
            case "bluePrintButton":
                bluePrintScreen.SetActive(true);
                break;

            case "audioListButton":
                audioListScreen.SetActive(true);
                break;

            case "restartButton":
                confirmationScreen.SetActive(true);
                break;

            case "pauseScreenButton":
                EventManager.INSTANCE.CallGamePauseStart();
                break;

            case "configurationButton":
                configurationScreen.SetActive(true);
                break;
        }

        blockingBackGround.SetActive(true);
	}

    public void OpenScreenByName(string name)
    {
        if (name == "call_confirmation_retry")
        {
            confirmationScreen.SetActive(true);
            confirmationScreen.GetComponent<Screen_Confirmation>().ConfirmRetryLevel();
            return;
        }
        else if (name == "call_confirmation_exit")
        {
            confirmationScreen.SetActive(true);
            confirmationScreen.GetComponent<Screen_Confirmation>().ConfirmExitGame();
            return;
        }
        else if (name == "call_confirmation_levelSelect")
        {
            confirmationScreen.SetActive(true);
            confirmationScreen.GetComponent<Screen_Confirmation>().ConfirmExitToLevelSelect();
            return;
        }

        else if (name == "call_pauseScreen")
        {
            configurationScreen.SetActive(true);
            return;
        }

        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        string[] nameSplit = name.Split("_"[0]);

        switch (nameSplit[1])
        {
            case "pause":
                pauseScreen.SetActive(true);
                break;

            case "bluePrintButton":
                bluePrintScreen.SetActive(true);
                break;

            case "audioListButton":
                audioListScreen.SetActive(true);
                break;

            case "restartButton":
                confirmationScreen.SetActive(true);
                break;

            case "feedbackLose":
                feedbackScreen.SetActive(true);
                feedbackScreen.GetComponent<Screen_Feedback>().FailFeedback();
                break;

            case "feedbackWin":
                feedbackScreen.SetActive(true);
                feedbackScreen.GetComponent<Screen_Feedback>().SucceedFeedback();
                break;
        }
        blockingBackGround.SetActive(true);
    }

    void CallPauseScreen()
    {
        CloseScreens();
        OpenScreenByName("call_pause");
    }

    void CallLoseFeedbackScreen()
    {
        OpenScreenByName("call_feedbackLose");
    }

    void CallWinFeedbackScreen()
    {
        OpenScreenByName("call_feedbackWin");
    }

    public void CloseScreens()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        blockingBackGround.SetActive(false);
    }

}
