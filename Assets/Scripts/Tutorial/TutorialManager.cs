using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TutorialManager : MonoBehaviour {

    #region Singleton

    private static TutorialManager _INSTANCE;

    public static TutorialManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<TutorialManager>();
            }
            return _INSTANCE;
        }
    }
    #endregion

     public List<string> _steps;
     public int _currentStep = 0;

     public List<KeyCode> _inputKeyList;
     public int _currentInput = 0;

     public List<Material> _materialsList;
     public int _currentMaterial = 0;

     public List<GameObject> _clickTargetList;
     public int _currentTarget = 0;

     public bool _isWaitingInput = false;

     public GameObject _tutorialPanel;

     public GameObject _target;
     public KeyCode _inputKey;

     public Camera ActualCamera;

    public List<string> Steps = new List<string>();
    public List<KeyCode> Inputs = new List<KeyCode>();
    public List<Material> Mats = new List<Material>();
    public List<GameObject> ClickTarget = new List<GameObject>();


    void Awake()
    {
        if (_tutorialPanel == null)
            _tutorialPanel = GameObject.Find("Tutorial Panel");

        ActualCamera = GameObject.Find("ActualCamera").GetComponent<Camera>();

        _tutorialPanel.SetActive(false);

    }

    public void ReceiveSteps(List<string> list, List<KeyCode> keycodes, List<Material> mats, List<GameObject> targets)
    {
        if (list.Count <= 0)
            return;

        _steps = list;

        if (mats.Count > 0)
            _materialsList = mats;

        if (keycodes.Count > 0)
            _inputKeyList = keycodes;

        if(targets.Count >0)
            _clickTargetList = targets;

        ExecuteNextStep();
    }

    void ExecuteNextStep()
    {
        if (_currentStep < _steps.Count)
            StartCoroutine(WaitAndGoToNextStep(_steps[_currentStep]));
        else
            FinishTutorial();
        
    }

    void ShowTutorialPanel()
    {
        _tutorialPanel.renderer.material = _materialsList[_currentMaterial];
        _currentMaterial++;
        _tutorialPanel.SetActive(true);
        ExecuteNextStep();
    }

    void FinishTutorial()
    {
        EventManager.INSTANCE.CallStageSucces();
    }

    IEnumerator WaitAndGoToNextStep(string str)
    {
        _currentStep++;
        yield return new WaitForSeconds(0.2f);

        switch (str)
        { 
            case "ShowTutorialPanel":
                ShowTutorialPanel();
                break;

            case "WaitForKeyboardInput":
                _isWaitingInput = true;
                StartCoroutine(WaitKeyboardInput());
                break;

            case "WaitForMouseInput":
                _isWaitingInput = true;
                StartCoroutine(WaitMouseInput());
                break;
        }
    }

    IEnumerator WaitKeyboardInput()
    {
        while (_isWaitingInput)
        {
            CheckKeyboardInput();
            yield return 0;
        }

        _currentInput++;
        ExecuteNextStep();
    }

    public virtual void CheckKeyboardInput()
    {
        if (Input.GetKey(_inputKeyList[_currentInput]))
        {
            _isWaitingInput = false;
        }
    
	}


    IEnumerator WaitMouseInput()
    {
        while (_isWaitingInput)
        {
            CheckMouseInput();
            yield return 0;
        }
        
        ExecuteNextStep();
    }

    public virtual void CheckMouseInput()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        
    }

  


}
