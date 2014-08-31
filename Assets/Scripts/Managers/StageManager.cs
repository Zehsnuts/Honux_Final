using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    #region Events
    void OnEnable()
    {
        EventManager.LOADINGEND += GrabLevelName;
    }

    void OnDisable()
    {
        EventManager.LOADINGEND -= GrabLevelName;
    }
    #endregion
    #region Singleton

    private static StageManager _INSTANCE;
    public static StageManager INTANCE
    {
        get 
        {
            if (_INSTANCE == null)
                _INSTANCE = GameObject.FindObjectOfType<StageManager>();

            return _INSTANCE;
        }
    }

    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _INSTANCE)
                Destroy(this.gameObject);
        }
    }

    #endregion

    public string currentLevel;
    public List<string> lastLevel;

    private List<string> _unlockedLevels = new List<string>();

    void Start()
    {
        _unlockedLevels.Add("Stage_tut_1");        

        EnableUnlockedLevels();

        currentLevel = Application.loadedLevelName;
    }

    void GrabLevelName()
    {
        currentLevel = Application.loadedLevelName; 
    }

    public void RetryCurrentLevel()
    {
        LoadingManager.INSTANCE.LoadLevelByName(currentLevel);
    }

    public void ChangeLevelTo(string level)
    {
        lastLevel.Add(currentLevel);
        LoadingManager.INSTANCE.LoadLevelByName(level);
    }

    public void ChangeLevelBySelection(string buttonName)
    {
        ChangeLevelTo(buttonName);
    }

    public void ReturnToStageSelect()
    {
        LoadingManager.INSTANCE.LoadLevelByName("Stage_HUB");
    }

    public void StageCompleted()
    {
        Debug.Log(currentLevel + "is completed!");
        StartCoroutine(WaitForStageToEnd());
    }

    IEnumerator WaitForStageToEnd()
    {
        Debug.Log("Endind level in 5");
        yield return new WaitForSeconds(1.0F);
        Debug.Log("Endind level in 4");
        yield return new WaitForSeconds(1.0F);
        Debug.Log("Endind level in 3");
        yield return new WaitForSeconds(1.0F);
        Debug.Log("Endind level in 2");
        yield return new WaitForSeconds(1.0F);
        Debug.Log("Endind level in 1");
        yield return new WaitForSeconds(1.0F);
  
    }

    public void ProvisoriSolution()
    {
        GrabLevelName();
        switch (currentLevel)
        { 
            case "Stage_1_1":
                LoadingManager.INSTANCE.LoadLevelByName("Stage_2_1");
                break;

            case "Stage_2_1":
                LoadingManager.INSTANCE.LoadLevelByName("Stage_3_1");
                break;

            case "Stage_3_1":
                LoadingManager.INSTANCE.LoadLevelByName("Stage_4_1");
                break;

            case "Stage_4_1":
                Application.Quit();
                break;
        }
    }
    
    void EnableUnlockedLevels()
    {
        foreach (string stage in _unlockedLevels)
            if (GameObject.Find(stage))
                GameObject.Find(stage).SetActiveRecursively(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void EnableAllLevels()
    {
        FillCheatList();

        EnableUnlockedLevels();
    }
    void FillCheatList()
    {
        _unlockedLevels.Add("Stage_tut_1");
        _unlockedLevels.Add("Stage_tut_2");
        _unlockedLevels.Add("Stage_tut_3");
        _unlockedLevels.Add("Stage_tut_4");
        _unlockedLevels.Add("Stage_tut_5");
        _unlockedLevels.Add("Stage_tut_6");
        _unlockedLevels.Add("Stage_tut_7");
        _unlockedLevels.Add("Stage_tut_8");

        _unlockedLevels.Add("Stage_1_1");
        _unlockedLevels.Add("Stage_1_2");
        _unlockedLevels.Add("Stage_1_3");
        _unlockedLevels.Add("Stage_1_4");
        _unlockedLevels.Add("Stage_1_5");
        _unlockedLevels.Add("Stage_1_6");

        _unlockedLevels.Add("Stage_2_1");
        _unlockedLevels.Add("Stage_2_2");
        _unlockedLevels.Add("Stage_2_3");
        _unlockedLevels.Add("Stage_2_4");
        _unlockedLevels.Add("Stage_2_5");
        _unlockedLevels.Add("Stage_2_6");

        _unlockedLevels.Add("Stage_3_1");
        _unlockedLevels.Add("Stage_3_2");
        _unlockedLevels.Add("Stage_3_3");
        _unlockedLevels.Add("Stage_3_4");
        _unlockedLevels.Add("Stage_3_5");
        _unlockedLevels.Add("Stage_3_6");

        _unlockedLevels.Add("Stage_4_1");
        _unlockedLevels.Add("Stage_4_2");
        _unlockedLevels.Add("Stage_4_3");
        _unlockedLevels.Add("Stage_4_4");
        _unlockedLevels.Add("Stage_4_5");
        _unlockedLevels.Add("Stage_4_6");

        _unlockedLevels.Add("Stage_5_1");
        _unlockedLevels.Add("Stage_5_2");
        _unlockedLevels.Add("Stage_5_3");
        _unlockedLevels.Add("Stage_5_4");
        _unlockedLevels.Add("Stage_5_5");
        _unlockedLevels.Add("Stage_5_6");

    }
}
