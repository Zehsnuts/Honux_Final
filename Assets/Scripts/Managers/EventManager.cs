#region Header
/*
 * Event manager serve para fazer a "call" dos eventos do jogo.
 * Não é ele que vai chamar a execução do evento mas ele é dono dos eventos então, os outros scripts precisam chamar por esse, exemplo:
 *  Quando o LoadingManager termina de carregar a fase, ele precisa avisar todos os outros scripts que ele acabou usando:
 *  
 *         EventManager.INSTANCE.CallFinishLoading();
 *         
 * Para que os outros scripts que vão ouvir a chamada do evento, é preciso adicionar um listener daquele evento usando:
 * 
 *           EventManager.(Nome do Evento) += (Nome da Função que será executada);
 * 
 * Exemplo:
 *  GUIManager tem uma função chamada "TurnGUIComponentsOn()" que precisa ser chamado quando termina o loading da fase.
 *  A linha fica:
 *          EventManager.FINISHLOADING += TurnGUIComponentsOn; //Sem () para funcionar
 *          
 * É boa pratica incluir essa linha nas funções OnEnable e OnDisable da classe, para que o listener seja adicionado e removido:
 * 
         * public class GUIManager : Monobehavior
         * {
 * 
         *   void OnEnable()
            {
                EventManager.FINISHLOADING += TurnGUIComponentsOn;
            }    
    
            void OnDisable()
            {
                EventManager.FINISHLOADING -= TurnGUIComponentsOn;
            }    
    
            void TurnGUIComponentsOn()
            {                
            }
 * }
 * 
 * 
 * */

#endregion

using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    #region Singleton

    private static EventManager _INSTANCE;

    public static EventManager INSTANCE
    { 
        get
        {
            if(_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<EventManager>();
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

    public delegate void StageStart();
    public static event StageStart STAGESTART;
    public  void CallStageStart()
    {
        if(STAGESTART!=null)
            STAGESTART();
    }

    public delegate void StageSucces();
    public static event StageSucces STAGESUCESS;
    public void CallStageSucces()
    {
        if (STAGESUCESS != null)
            STAGESUCESS();
    } 

    public delegate void StageFail();
    public static event StageFail STAGEFAIL;
    public void CallStageFail()
    {
        if (STAGEFAIL != null)
            STAGEFAIL();
    }

    public delegate void StageFailSequenceInitiated();
    public static event StageFailSequenceInitiated STAGEFAILSEQUENCE;
    public void CallStageFailSequenceInitiated()
    {
        if (STAGEFAILSEQUENCE != null)
            STAGEFAILSEQUENCE();
    }

    public delegate void StageFailSequenceStop();
    public static event StageFailSequenceStop STAGEFAILSEQUENCESTOP;
    public void CallStageFailSequenceStop()
    {
        if (STAGEFAILSEQUENCESTOP != null)
            STAGEFAILSEQUENCESTOP();
    } 

    public delegate void MainMenu();
    public static event MainMenu MAINMENUSTART;
    public void CallMainMenu()
    {
        if (MAINMENUSTART != null)
            MAINMENUSTART();
    }

    public delegate void StageEnd();
    public static event StageEnd STAGEEND;
    public void CallStageEnd()
    {
        if (STAGEEND != null)
            STAGEEND();
    }    

    public delegate void LoadingStart();
    public static event LoadingStart LOADINGSTART;
    public void CallLoadingStart()
    {
        if (LOADINGSTART != null)
            LOADINGSTART();
    }

    public delegate void LoadingEnd();
    public static event LoadingEnd LOADINGEND;
    public void CallLoadingEnd()
    {
        if (LOADINGEND != null)
            LOADINGEND();
    }

    public delegate void GamePauseStart();
    public static event GamePauseStart GAMEPAUSESTART;
    public void CallGamePauseStart()
    {
        if (GAMEPAUSESTART != null)
            GAMEPAUSESTART();
    }

    public delegate void GamePauseExit();
    public static event GamePauseExit GAMEPAUSEEXIT;
    public void CallGamePauseExit()
    {
        if (GAMEPAUSEEXIT != null)
            GAMEPAUSEEXIT();
    }

    public delegate void StageSelectStart();
    public static event StageSelectStart STAGESELECTSTART;
    public void CallStageSelectStart()
    {
        if (STAGESELECTSTART != null)
            STAGESELECTSTART();
    }

    public delegate void StageSelectExit();
    public static event StageSelectExit STAGESELECTEXIT;
    public void CallStageSelectExit()
    {
        if (STAGESELECTEXIT != null)
            STAGESELECTEXIT();
    }

    public delegate void CutsceneStart();
    public static event CutsceneStart CUTSCENESTART;
    public void CallCutsceneStart()
    {
        if (CUTSCENESTART != null)
            CUTSCENESTART();
    }

    public delegate void CutsceneEnd();
    public static event CutsceneEnd CUTSCENEEND;
    public void CallCutsceneEnd()
    {
        if (CUTSCENEEND != null)
            CUTSCENEEND();
    }

    public delegate void ChangeCamera();
    public static event ChangeCamera CHANGECAMERA;
    public void CallChangeCamera()
    {       
        if (CHANGECAMERA != null)
            CHANGECAMERA();
    }

    public delegate void BluePrintStart();
    public static event BluePrintStart BLUEPRINTSTART;
    public void CallBluePrintStart()
    {
        if (BLUEPRINTSTART != null)
            BLUEPRINTSTART();
    }

    public delegate void BluePrintExit();
    public static event BluePrintExit BLUEPRINTEXIT;
    public void CallBluePrintExit()
    {
        if (BLUEPRINTEXIT != null)
            BLUEPRINTEXIT();
    }

    public delegate void AudioScreenStart();
    public static event AudioScreenStart AUDIOSCREENSTART;
    public void CallAudioScreenStart()
    {
        if (AUDIOSCREENSTART != null)
            AUDIOSCREENSTART();
    }

    public delegate void AudioScreenExit();
    public static event AudioScreenExit AUDIOSCREENEXIT;
    public void CallAudioScreenExit()
    {
        if (AUDIOSCREENEXIT != null)
            AUDIOSCREENEXIT();
    }

    public delegate void PinTurnOn_And();
    public static event PinTurnOn_And PINTURNON_AND;
    public void CallPinTurnOn_And()
    {
        if (PINTURNON_AND != null)
            PINTURNON_AND();
    }

    public delegate void PinTurnOff_And();
    public static event PinTurnOff_And PINTURNOFF_AND;
    public void CallPinTurnOff_And()
    {
        if (PINTURNOFF_AND != null)
            PINTURNOFF_AND();
    }

    public delegate void PinTurnOn_Or();
    public static event PinTurnOn_Or PINTURNON_OR;
    public void CallPinTurnOn_Or()
    {
        if (PINTURNON_OR != null)
            PINTURNON_OR();
    }

    public delegate void PinTurnOff_Or();
    public static event PinTurnOff_Or PINTURNOFF_OR;
    public void CallPinTurnOff_Or()
    {
        if (PINTURNOFF_OR != null)
            PINTURNOFF_OR();
    }

    public delegate void PinTurnOn_Xor();
    public static event PinTurnOn_Xor PINTURNON_XOR;
    public void CallPinTurnOn_Xor()
    {
        if (PINTURNON_XOR != null)
            PINTURNON_XOR();
    }

    public delegate void PinTurnOff_Xor();
    public static event PinTurnOff_Xor PINTURNOFF_XOR;
    public void CallPinTurnOff_Xor()
    {
        if (PINTURNOFF_XOR != null)
            PINTURNOFF_XOR();
    }

    public delegate void UnstableTurnOn();
    public static event UnstableTurnOn UNSTABLETURNON;
    public void CallUnstableTurnOn()
    {
        if (UNSTABLETURNON != null)
            UNSTABLETURNON();
    }

    public delegate void UnstableTurnOff();
    public static event UnstableTurnOff UNSTABLETURNOFF;
    public void CallUnstableTurnOff()
    {
        if (UNSTABLETURNOFF != null)
            UNSTABLETURNOFF();
    }


    public delegate void RobotCreationSequence();
    public static event RobotCreationSequence ROBOTCREATION;
    public void CallRobotCreationSequence()
    {
        if (ROBOTCREATION != null)
            ROBOTCREATION();
    }

    public delegate void RobotCreationSequenceStop();
    public static event RobotCreationSequenceStop ROBOTCREATIONSTOP;
    public void CallRobotCreationSequenceStop()
    {
        if (ROBOTCREATIONSTOP != null)
            ROBOTCREATIONSTOP();
    }

    public delegate void RobotTurnOn();
    public static event RobotTurnOn ROBOTURNON;
    public void CallRobotTurnOn()
    {
        if (ROBOTURNON != null)
            ROBOTURNON();
    }

    public delegate void RobotTurnOff();
    public static event RobotTurnOff ROBOTURNOFF;
    public void CallRobotTurnOff()
    {
        if (ROBOTURNOFF != null)
            ROBOTURNOFF();
    }

    
}
