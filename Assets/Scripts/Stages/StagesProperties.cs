using UnityEngine;
using System.Collections;
public class StagesProperties : MonoBehaviour
{
    #region Events
    void OnEnable()
    {
        EventManager.PINTURNON_XOR += PinXorTurnedOn;
        EventManager.PINTURNON_OR += PinOrTurnedOn;
        EventManager.PINTURNON_AND += PinAndTurnedOn;

        EventManager.PINTURNOFF_XOR += PinXorTurnedOff;
        EventManager.PINTURNOFF_OR += PinOrTurnedOff;
        EventManager.PINTURNOFF_AND += PinAndTurnedOff;

        EventManager.UNSTABLETURNON += TurnRobotOn;

        EventManager.UNSTABLETURNON += AddToUnstableCount;
        EventManager.UNSTABLETURNOFF += RemoveFromUnstableCount;

        EventManager.STAGESUCESS += TurnRobotOff;
        EventManager.STAGEFAIL += TurnRobotOff;
    }

    void OnDisable()
    {
        EventManager.PINTURNON_XOR -= PinXorTurnedOn;
        EventManager.PINTURNON_OR -= PinOrTurnedOn;
        EventManager.PINTURNON_AND -= PinAndTurnedOn;

        EventManager.PINTURNOFF_XOR -= PinXorTurnedOff;
        EventManager.PINTURNOFF_OR -= PinOrTurnedOff;
        EventManager.PINTURNOFF_AND -= PinAndTurnedOff;

        EventManager.UNSTABLETURNON -= TurnRobotOn;

        EventManager.UNSTABLETURNON -= AddToUnstableCount;
        EventManager.UNSTABLETURNOFF -= RemoveFromUnstableCount;

        EventManager.STAGESUCESS -= TurnRobotOff;
        EventManager.STAGEFAIL -= TurnRobotOff;
    }
    #endregion

    public int NumberOfInitialTracks = 0;
    public int TimeAvailableInMinutes;
    public int TimeAvailableInSeconds;

    public int MaxUnstableUnitsPowered;
    private int _actualUnstableUnitsPowered=0;
    private bool _isCoutingDownToStageFail = false;
    private bool _isRobotOn = false;
    private bool _isStageSucceded = false;


    public int NumberOfPinsAND=0;
    public int NumberOfPinsOR=0;
    public int NumberOfPinsXOR=0;

    private int _pinsAnd = 0;
    private int _pinsOr = 0;
    private int _pinsXor = 0;

    private bool _and = false;
    private bool _or = false;
    private bool _xor = false;


    void Start()
    {
        HUDManager.INSTANCE.SetCountDownTimer(TimeAvailableInMinutes, TimeAvailableInSeconds);

        ResourcesManager.INSTANCE.SetNumberOfInitialTrack(NumberOfInitialTracks);  

        if (NumberOfPinsAND == 0 && NumberOfPinsOR == 0 && NumberOfPinsXOR == 0)
        {

            Object[] list = FindObjectsOfType<CrystalsUnit_pinAND>();
            NumberOfPinsAND = list.Length;

            list = FindObjectsOfType<CrystalsUnit_pinOR>();
            NumberOfPinsOR = list.Length;

            list = FindObjectsOfType<CrystalsUnit_pinXOR>();
            NumberOfPinsXOR = list.Length;

            if (NumberOfPinsAND == 0 && NumberOfPinsOR == 0 && NumberOfPinsXOR == 0)
                Debug.LogError("You need to set at least one pin needed to complete this level in the Stage Holder. \nNumber Of Pins AND = " + NumberOfPinsAND + "\nNumber Of Pins OR = " + NumberOfPinsOR + "\nNumber Of Pins XOR = " + NumberOfPinsXOR);
        }
    }
    
    #region Pins

    void PinAndTurnedOn()
    {
        StopCoroutine("WaitBeforeCheckingIfStageEnded");

        _pinsAnd++;
        StartCoroutine("WaitBeforeCheckingIfStageEnded");
    }

    void PinAndTurnedOff()
    {
        StopCoroutine("WaitBeforeCheckingIfStageEnded");

        _pinsAnd--;
        StartCoroutine("WaitBeforeCheckingIfStageEnded");

    }

    void PinOrTurnedOn()
    {
        StopCoroutine("WaitBeforeCheckingIfStageEnded");

        _pinsOr++;
        StartCoroutine("WaitBeforeCheckingIfStageEnded");

    }

    void PinOrTurnedOff()
    {
        StopCoroutine("WaitBeforeCheckingIfStageEnded");

        _pinsOr--;
        StartCoroutine("WaitBeforeCheckingIfStageEnded");

    }

    void PinXorTurnedOn()
    {
        StopCoroutine("WaitBeforeCheckingIfStageEnded");
        _pinsXor++;
        StartCoroutine("WaitBeforeCheckingIfStageEnded");

    }

    void PinXorTurnedOff()
    {
        StopCoroutine("WaitBeforeCheckingIfStageEnded");
        _pinsXor--;
        StartCoroutine("WaitBeforeCheckingIfStageEnded");
    }
    #endregion

    void AddToUnstableCount()
    {
        _actualUnstableUnitsPowered++;

        if (_actualUnstableUnitsPowered >= MaxUnstableUnitsPowered && !_isStageSucceded)
        {
            //Debug.Log("Unstable");
            _isCoutingDownToStageFail = true;
            EventManager.INSTANCE.CallStageFailSequenceInitiated();
            StartCoroutine("StageFailCountDown");
        }
    }

    void RemoveFromUnstableCount()
    {
        _actualUnstableUnitsPowered--;

        ImpossibilityCheck();

        if (_actualUnstableUnitsPowered == 0)
        {
            TurnRobotOff();
            EventManager.INSTANCE.CallRobotCreationSequenceStop();
        }

        if (_isCoutingDownToStageFail && _actualUnstableUnitsPowered < MaxUnstableUnitsPowered || _actualUnstableUnitsPowered == 0)
        {
            StopCoroutine("StageFailCountDown");
            _isCoutingDownToStageFail = false;
            EventManager.INSTANCE.CallStageFailSequenceStop();
        }
    }

    IEnumerator StageFailCountDown()
    {
        yield return new WaitForSeconds(5);

        if(_isCoutingDownToStageFail)
            EventManager.INSTANCE.CallStageFail();
    }

    #region Robot

    void TurnRobotOn()
    {
        if (_isRobotOn)
            return;
        EventManager.INSTANCE.CallRobotCreationSequence();
        StartCoroutine("CreateRobot");    
    }

    IEnumerator CreateRobot()
    {
        Debug.Log("Robot turning on event");
        yield return new WaitForSeconds(5);        
            EventManager.INSTANCE.CallRobotTurnOn();
            _isRobotOn = true;
    }

    void TurnRobotOff()
    {
        if (_isRobotOn)
            return;
        Debug.Log("Stop robot creation");
        StopCoroutine("CreateRobot");
        _isRobotOn = false;
        EventManager.INSTANCE.CallRobotCreationSequenceStop();
    }

    #endregion

    void CheckIfStageEnded()
    {
        ImpossibilityCheck();

        if (_pinsAnd == NumberOfPinsAND)
            _and = true;
        else
            _and = false;

        if (_pinsOr == NumberOfPinsOR)
            _or = true;
        else
            _or = false;


        if (_pinsXor == NumberOfPinsXOR)
            _xor = true;
        else
            _xor = false;        

        Debug.Log(_pinsAnd + " And " + _pinsOr + " Or " + _pinsXor + " Xor");

        if (_and && _xor && _or )
        {
            _isStageSucceded = true;
            StopCoroutine("StageFailCountDown");
            _isCoutingDownToStageFail = false;
            EventManager.INSTANCE.CallStageFailSequenceStop();

            EventManager.INSTANCE.CallStageSucces();
        }
    }

    void ImpossibilityCheck()
    { 
        if(_pinsAnd<=0)
            _pinsAnd=0;
        if (_pinsOr <= 0)
            _pinsOr = 0;
        if (_pinsXor <= 0)
            _pinsXor = 0;

        if (_actualUnstableUnitsPowered <= 0)
            _actualUnstableUnitsPowered = 0;
    }

    //As vezes as checagens de conexão acontecem muito rapido e não da tempo dos pinos saberem que existem mais peças conectadas à eles
    //alem da que está passando energia naquele frame. 
    //No caso do pino XOR, quando uma peça conectada à ele for a unica a ser ligada e transferir energia, ele será ligado naquele frame. 
    IEnumerator WaitBeforeCheckingIfStageEnded()
    {
        yield return new WaitForSeconds(2);
        CheckIfStageEnded();     
    }
}
