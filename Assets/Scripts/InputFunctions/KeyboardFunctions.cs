using UnityEngine;
using System.Collections;

public class KeyboardFunctions : MonoBehaviour 
{

    #region Singleton

    private static KeyboardFunctions _INSTANCE;

    public static KeyboardFunctions INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<KeyboardFunctions>();
            }
            return _INSTANCE;
        }
    }


    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _INSTANCE)
                Destroy(this.gameObject);
        }
    }
    #endregion

	void Update () 
    {
        switch (GameStateManager.INSTANCE.SpecialGameState)
        {
            case GameStateManager.SpecialGameStates.FreeInput:
                FreeInput();
                break;

            case GameStateManager.SpecialGameStates.OnPause:
                OnPauseInput();
                break;

            case GameStateManager.SpecialGameStates.OnMenu:
                OnStartMenuInput();
                break;

            case GameStateManager.SpecialGameStates.OnCutscene:
                OnCutsceneInput();
                break;

            case GameStateManager.SpecialGameStates.OnBluePrint:
                OnBluePrintInput();
                break;

            case GameStateManager.SpecialGameStates.OnAudioScreen:
                OnAudioScreenInput();
                break;
        }
	}

    void FreeInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            EventManager.INSTANCE.CallGamePauseStart();

        if (Input.GetKeyUp(KeyCode.Space))
            EventManager.INSTANCE.CallChangeCamera();
    }

    void OnPauseInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            EventManager.INSTANCE.CallGamePauseExit();
    }

    void OnStartMenuInput()
    {

    }

    void OnCutsceneInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            EventManager.INSTANCE.CallCutsceneEnd();
    }

    void OnBluePrintInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            EventManager.INSTANCE.CallBluePrintExit();

        if (Input.GetKeyUp(KeyCode.Space))
            EventManager.INSTANCE.CallChangeCamera();
    }

    void OnAudioScreenInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            EventManager.INSTANCE.CallAudioScreenExit();
    }
}
