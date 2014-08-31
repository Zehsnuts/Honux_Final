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

    #region Events
    void OnEnable()
    {
        EventManager.GAMEPAUSESTART += PauseState;
        EventManager.GAMEPAUSEEXIT += NoneState;      
    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= PauseState;
        EventManager.GAMEPAUSEEXIT -= NoneState;
    }
    #endregion

    public enum PauseScreenState 
    {
        Index,
        ConfirmRetry,
        ConfirmQuit,
        NotPaused
    }
    private PauseScreenState _pauseState;


    private Transform _screen;
    private Transform _confirmScreen;

    private Transform _txtQuit;
    private Transform _txtRetry;

    void Start()
    {
        _screen = transform.FindChild("Screen");
        _confirmScreen = _screen.FindChild("Confirm");
        
        _txtRetry = _confirmScreen.FindChild("Texts").FindChild("Deseja_recomeçar");
        _txtQuit = _confirmScreen.FindChild("Texts").FindChild("Deseja_sair");

    }

    public void ClickedThisButton(string name)
    {
        switch (name)
        {
            case "btn_Continue":
                EventManager.INSTANCE.CallGamePauseExit();
                break;

            case "btn_Retry":
                _pauseState = PauseScreenState.ConfirmRetry;
                CallConfirmScreen();
                break;

            case "btn_Quit":
                _pauseState = PauseScreenState.ConfirmQuit;
                CallConfirmScreen();
                break;

            case "btn_Yes":
                DecidedYes();
                break;

            case "btn_No":
                CloseConfirmScreen();
                break;
        }
    }

    void PauseState()
    {
        _pauseState = PauseScreenState.Index;

        _screen.gameObject.SetActiveRecursively(true);

        _confirmScreen.gameObject.SetActiveRecursively(false);
    }

    void NoneState()
    {
        _screen.gameObject.SetActiveRecursively(false);
    }

    void CallConfirmScreen()
    {
        _confirmScreen.gameObject.SetActiveRecursively(true);

        switch(_pauseState)
        {
            case PauseScreenState.ConfirmQuit:
                _txtQuit.active = true;
                _txtRetry.active = false;
                break;

            case PauseScreenState.ConfirmRetry:
                _txtQuit.active = false;
                _txtRetry.active = true;
                break;
        }
    }

    void CloseConfirmScreen()
    {
        _confirmScreen.gameObject.SetActiveRecursively(false);
        
    }

    void DecidedYes()
    {
        switch (_pauseState)
        {
            case PauseScreenState.ConfirmQuit:
                StageManager.INTANCE.ReturnToStageSelect();
                break;

            case PauseScreenState.ConfirmRetry:
                StageManager.INTANCE.RetryCurrentLevel();
                break;
        }
    }

}
