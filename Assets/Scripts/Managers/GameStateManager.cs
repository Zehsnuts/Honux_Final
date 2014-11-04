using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    #region Singleton

    private static GameStateManager _INSTANCE;

    public static GameStateManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<GameStateManager>();
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

        EventManager.MAINMENUSTART += MenuState;

        EventManager.LOADINGSTART += LoadState;

        EventManager.CUTSCENESTART += CutsceneState;
        EventManager.CUTSCENEEND += NoneState;

        EventManager.STAGESTART += NoneState;
        EventManager.STAGEFAIL += FeedbackState;
        EventManager.STAGESUCESS += FeedbackState;    

        EventManager.BLUEPRINTSTART += BluePrintState;
        EventManager.BLUEPRINTEXIT += NoneState;

        EventManager.AUDIOSCREENSTART += AudioScreenState;
        EventManager.AUDIOSCREENEXIT += NoneState;

        EventManager.STAGESELECTSTART += StageSelectState;
        EventManager.STAGESELECTEXIT += NoneState;
    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= PauseState;
        EventManager.GAMEPAUSEEXIT -= NoneState;

        EventManager.MAINMENUSTART -= MenuState;

        EventManager.LOADINGSTART -= LoadState;

        EventManager.CUTSCENESTART -= CutsceneState;
        EventManager.CUTSCENEEND -= NoneState;

        EventManager.STAGESTART -= NoneState;
        EventManager.STAGEFAIL -= FeedbackState;
        EventManager.STAGESTART -= FeedbackState;

        EventManager.BLUEPRINTSTART -= BluePrintState;
        EventManager.BLUEPRINTEXIT -= NoneState;        
    }
    #endregion

    public enum SpecialGameStates 
    {
        FreeInput,
        OnPause,
        OnCutscene,
        OnMenu,
        OnStageSelect,
        OnLoad,
        OnBluePrint,
        OnAudioScreen,
        OnFeedBack,
        OnTutorial
    }

    public SpecialGameStates SpecialGameState;    

    void NoneState()
    {
        SpecialGameState = SpecialGameStates.FreeInput;
    }

    void PauseState()
    {
        SpecialGameState = SpecialGameStates.OnPause;
    }

    void LoadState()
    {
        SpecialGameState = SpecialGameStates.OnLoad;        
    }

    void MenuState()
    {
        SpecialGameState = SpecialGameStates.OnMenu;
    }

    void CutsceneState()
    {
        SpecialGameState = SpecialGameStates.OnCutscene;
    }

    void BluePrintState()
    {
        SpecialGameState = SpecialGameStates.OnBluePrint;
    }

    void AudioScreenState()
    {
        SpecialGameState = SpecialGameStates.OnAudioScreen;
    }

    void FeedbackState()
    {
        SpecialGameState = SpecialGameStates.OnFeedBack;
    }

    void StageSelectState()
    {
        SpecialGameState = SpecialGameStates.OnStageSelect;
    }

    void TutorialState()
    {
        SpecialGameState = SpecialGameStates.OnTutorial;
    }
    

}
