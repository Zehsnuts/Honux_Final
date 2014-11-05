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

    public TextAsset tutorialStepsTextFile;
    private List<string> _tutorialLevels = new List<string>();
    private List<string> _tutorialDescriptions = new List<string>();
    private List<Transform> tutorialObjects;
    public List<string> _currentStageDescriptions;

    public int _currentStep;
    public int _totalStageSteps;

    private TutorialObject _currentTutorialObject;

    public UITexture tutorialPanel;
    public UILabel tutorialLabel;

    public GameObject arrow;
    public GameObject arrowTarget;

    void Awake()
    {
        ChangeState(false);

        string[] lines = tutorialStepsTextFile.ToString().Split("\n"[0]);

        for (int i = 0; i < lines.Length-1; i++)
        {
            string[] itemSplit = lines[i].Split(";"[0]);
            _tutorialLevels.Add(itemSplit[0]);
            _tutorialDescriptions.Add(itemSplit[1]);
        }

        DontDestroyOnLoad(gameObject);

        transform.parent = GameObject.Find("_Manager").transform;

        LoadDescriptons();

        NextStep();
    }

    void LoadDescriptons()
    {
        _currentStep = 0;
        if (_tutorialLevels.Contains(Application.loadedLevelName))
        {
            for (int i = 0; i < _tutorialLevels.Count; i++)
                if (_tutorialLevels[i] == Application.loadedLevelName)
                    _currentStageDescriptions.Add(_tutorialDescriptions[i]);            
        }                
    }

    bool GrabTutorialStep()
    {
        ChangeState(false);

        string loadAtPath = "TutorialObjects/TO_" + _currentStep.ToString() + "_" + Application.loadedLevelName;

        GameObject go = Resources.Load<GameObject>(loadAtPath);

        if (go == null)
            return false;

        go = Instantiate(Resources.Load("TutorialObjects/TO_" + _currentStep.ToString() + "_" + Application.loadedLevelName)) as GameObject;

        go.transform.parent = transform;
        _currentTutorialObject = go.GetComponent<TutorialObject>();
        
        go = null;

        return true;        
    }

    void OnLevelWasLoaded(int lvlNumber)
    {
        LoadDescriptons();                  
    }

    void ChangeState(bool state)
    {
        arrow.active = state;
        tutorialPanel.active = state;
        tutorialLabel.active = state;
    }

    IEnumerator WaitBeforNextStep()
    {
        ChangeState(false);
        yield return new WaitForSeconds(0.2f);

        NextDescription();
        _currentStep++;        

        if (GrabTutorialStep())
        {
            _currentTutorialObject.BeginStep();
            _currentTutorialObject.TUTORIALSTEPCOMPLETED += NextStep;
            ChangeState(true);

            if (_currentTutorialObject.tutorialSteps == TutorialObject.TutorialSteps.TargetClick)
                PointArrowToObject();

            if (_currentTutorialObject.tutorialSteps == TutorialObject.TutorialSteps.KeyPress && _currentTutorialObject.clickTarget != null)
                PointArrowToObject();

            else if (_currentTutorialObject.tutorialSteps == TutorialObject.TutorialSteps.KeyPress && _currentTutorialObject.clickTarget == null)
                arrow.active = false;

            else if (_currentTutorialObject.tutorialSteps == TutorialObject.TutorialSteps.WaitForRobotSteal)
                PointArrowToObject();     
        }
    }

    void NextDescription()
    {
        tutorialLabel.text = _currentStageDescriptions[_currentStep];
    }

    void NextStep()
    {
        StartCoroutine(WaitBeforNextStep());
    }

    void PointArrowToObject()
    {
        if (_currentTutorialObject.clickTarget == null)
        {
            arrow.transform.position = GameObject.Find(_currentTutorialObject.clickTargetName).transform.position;
            arrow.transform.rotation = GameObject.Find(_currentTutorialObject.clickTargetName).transform.rotation;

        }
        else
        {
            arrow.transform.position = _currentTutorialObject.clickTarget.transform.position;
            arrow.transform.rotation = _currentTutorialObject.clickTarget.transform.rotation;
        }

        if (_currentTutorialObject.clickTarget.name.Contains("Track:"))
            arrow.transform.position = _currentTutorialObject.clickTarget.transform.FindChild("Frame").transform.position;
    }
}
