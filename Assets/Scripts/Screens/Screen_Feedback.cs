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

    private bool _isFeedbackPositive;

    public GameObject winScreen;
    public GameObject loseScreen;

    void OnEnable()
    {
        if (winScreen == null || loseScreen == null)
        {
            winScreen = transform.FindChild("Win").gameObject;
            loseScreen = transform.FindChild("Lose").gameObject;
        }
    }

    public void SucceedFeedback()
    {
        _isFeedbackPositive = true;
        StartCoroutine(Wait()); 
    }

    public void FailFeedback()
    {
        _isFeedbackPositive = false;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        yield return new WaitForSeconds(1F);
        GiveFeedback();           
    }

    void GiveFeedback()
    {
        if (_isFeedbackPositive)
        {
            winScreen.SetActive(true);
            loseScreen.SetActive(false);
        }
        else
        {
            winScreen.SetActive(false);
            loseScreen.SetActive(true);
        }
    }

    public void ClickedThisButton(string name)
    {
        switch (name)
        {
            case "texture_retryButton":
                StageManager.INTANCE.RetryCurrentLevel();
                break;

            case "texture_backButton":
                StageManager.INTANCE.ReturnToStageSelect();
                break;
        }
    }

}
