using UnityEngine;
using System.Collections;

public class Screen_Feedback : MonoBehaviour {

    #region Singleton

    private static Screen_Feedback _INSTANCE;

    public static Screen_Feedback INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<Screen_Feedback>();
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
        EventManager.STAGESUCESS += SucceedFeedback;
        EventManager.STAGEFAIL += FailFeedback;
    }

    void OnDisable()
    {
        EventManager.STAGESUCESS -= SucceedFeedback;
        EventManager.STAGEFAIL -= FailFeedback;
    }
    #endregion

    private bool _isFeedbackPositive;

    private Transform _failScreen;
    private Transform _succesScreen;

    void Start()
    {
        _failScreen = transform.FindChild("Fail");
        _succesScreen = transform.FindChild("Succeed");

        _failScreen.gameObject.SetActiveRecursively(false);
        _succesScreen.gameObject.SetActiveRecursively(false);
    }


    void SucceedFeedback()
    {
        _isFeedbackPositive = true;
        StartCoroutine(Wait()); 
    }

    void FailFeedback()
    {
        _isFeedbackPositive = false;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5F);
        GiveFeedback();           
    }

    void GiveFeedback()
    { 
        if(_isFeedbackPositive)
            _succesScreen.gameObject.SetActiveRecursively(true);
        else
            _failScreen.gameObject.SetActiveRecursively(true);

    }

    public void ClickedThisButton(string name)
    {

        switch (name)
        {
            case "btn_Yes":
                StageManager.INTANCE.RetryCurrentLevel();
                break;
            case "btn_Retry":
                StageManager.INTANCE.RetryCurrentLevel();
                break;

            case "btn_No":
                StageManager.INTANCE.ReturnToStageSelect();
                break;
            case "btn_Quit":
                StageManager.INTANCE.ReturnToStageSelect();
                break;
        }
    }

}
