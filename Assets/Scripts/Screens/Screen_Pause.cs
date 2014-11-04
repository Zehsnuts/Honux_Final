using UnityEngine;
using System.Collections;

public class Screen_Pause : MonoBehaviour {

    #region Singleton

    private static Screen_Pause _INSTANCE;

    public static Screen_Pause INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<Screen_Pause>();
            }
            return _INSTANCE;
        }
    }

    #endregion

    #region Events
    void OnEnable()
    {
        //if (FindObjectOfType<EventManager>())
           // EventManager.INSTANCE.CallGamePauseStart();
    }

    void OnDisable()
    {
        if(FindObjectOfType<EventManager>())
            EventManager.INSTANCE.CallGamePauseExit();
    }
    #endregion

    public void OpenConfigurationScreen(string name)
    {
        switch (name)
        {
            case "texture_configurationButton":
                FindObjectOfType<ScreensManager>().OpenScreenByName("call_pauseScreen");
                break;
        }
    }

    public void TryToRestartLevel(string name)
    {
        switch (name)
        {
            case "texture_restartButton":
                FindObjectOfType<ScreensManager>().OpenScreenByName("call_confirmation_retry");
                break;

            case "texture_yesButton":
                StageManager.INTANCE.RetryCurrentLevel();
                break;
        }
    }

    public void TryToExitLevel(string name)
    {
        switch (name)
        {
            case "texture_exitButton":
                FindObjectOfType<ScreensManager>().OpenScreenByName("call_confirmation_exit");
                break;

            case "texture_yesButton":
                Application.Quit();
                break;
        }
    }

    public void TryToExitToLevelSelect(string name)
    {
        switch (name)
        {
            case "texture_menuButton":
                FindObjectOfType<ScreensManager>().OpenScreenByName("call_confirmation_levelSelect");
                break;

            case "texture_yesButton":
                StageManager.INTANCE.ReturnToStageSelect();
                break;
        }
    }

}
