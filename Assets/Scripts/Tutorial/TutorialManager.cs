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

    private List<string> _currentStageDescriptions;

    private int _currentStep;
    private int _totalStageSteps;

    public UITexture tutorialPanel;
    public UILabel tutorialLabel;

    public GameObject arrow;
    public GameObject arrowTarget;

    void Awake()
    {
        //ChangeState(false);
        string[] lines = tutorialStepsTextFile.ToString().Split("\n"[0]);
        Debug.Log(lines.Length);

        for (int i = 0; i < lines.Length-1; i++)
        {
            string[] itemSplit = lines[i].Split(";"[0]);
            _tutorialLevels.Add(itemSplit[0]);
            _tutorialDescriptions.Add(itemSplit[1]);
        }

        DontDestroyOnLoad(gameObject);

        transform.parent = GameObject.Find("_Manager").transform;
    }

    void OnLevelWasLoaded(int lvlNumber)
    {
        _currentStep = 0;
        if (_tutorialLevels.Contains(Application.loadedLevelName))
            foreach (var item in _tutorialLevels)            
                if (item == Application.loadedLevelName)
                    _currentStageDescriptions.Add(_tutorialDescriptions[ _tutorialLevels.IndexOf(item)]);             
    }

    void ChangeState(bool state)
    {
        arrow.active = state;
        tutorialPanel.active = state;
        tutorialLabel.active = state;
    }

    void NextStep()
    {
        _currentStep++;
    }
}
