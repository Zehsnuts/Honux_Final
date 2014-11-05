using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{

    #region Singleton

    private static HUDManager _INSTANCE;

    public static HUDManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<HUDManager>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    #region Events
    void OnEnable()
    {
        EventManager.GAMEPAUSESTART += StopCountDown;
        EventManager.GAMEPAUSEEXIT += StartCountDown;

        EventManager.STAGESUCESS += StopCountDown;
        EventManager.STAGEFAIL += StopCountDown;

        EventManager.CHANGECAMERA += EnableScreenSideAccordingToCamera;

    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= StopCountDown;
        EventManager.GAMEPAUSEEXIT -= StartCountDown;

        EventManager.STAGESUCESS -= StopCountDown;
        EventManager.STAGEFAIL -= StopCountDown;
        EventManager.CHANGECAMERA -= EnableScreenSideAccordingToCamera;
    }

    #endregion

    public UILabel leftClockLabel;
    public UILabel rightClockLabel;
    public UILabel leftTracksLabel;
    public UILabel rightTracksLabel;

    private string _timeText;
    private float _minutes = 5;
    private float _seconds = 59;
    public bool _shouldCountDown = false;

    private int _tracksCount;

    private Transform _hudContainerLeft;
    private Transform _hudContainerRight;

    void OnLevelWasLoaded(int level)
    {
        GrabElements();
    }


    void Start()
    {
        GrabElements();
    }

    void GrabElements()
    {
        _hudContainerLeft = transform.FindChild("InGame HUD Left");
        _hudContainerLeft.gameObject.SetActive(true);
        _hudContainerRight = transform.FindChild("InGame HUD Right");

        //EnableScreenSideAccordingToCamera();


        if (leftClockLabel == null || 
            leftTracksLabel == null ||
            rightTracksLabel == null ||
            rightClockLabel == null)
        {
            leftClockLabel = transform.FindChild("InGame HUD Left/texture_tempoBox/label_tempo").GetComponent<UILabel>();
            leftTracksLabel = transform.FindChild("InGame HUD Left/texture_trilhos/label_trilhos").GetComponent<UILabel>();

            rightClockLabel = transform.FindChild("InGame HUD Right/texture_tempoBox/label_tempo").GetComponent<UILabel>();
            rightTracksLabel = transform.FindChild("InGame HUD Right/texture_trilhos/label_trilhos").GetComponent<UILabel>();
        }
        _hudContainerRight.gameObject.SetActive(false);

        UpdateTracksCounter();
    }

    void Update()
    {
        if (_shouldCountDown)
            Cronometer();

        UpdateTracksCounter();
    }


    void StartCountDown()
    {
        _shouldCountDown = true;
    }
    void StopCountDown()
    {
        _shouldCountDown = false;
    }

    void Cronometer()
    {
        _seconds -= Time.deltaTime;

        if (_seconds < 0)
        {
            _minutes--;
            _seconds = 59;
        }

        _timeText = "0" + _minutes.ToString() + ":" + Mathf.Round(_seconds).ToString();

        if (_seconds < 9.5f)
            _timeText = "0" + _minutes.ToString() + ":" + "0" + Mathf.Round(_seconds).ToString();

        rightClockLabel.text = leftClockLabel.text = _timeText;

        CheckIfTimeIsOver();
    }

    void CheckIfTimeIsOver()
    {
        if (_minutes == 0 && _seconds <0.5f)
        {
            _shouldCountDown = false;

            Debug.Log("Hello");

            EventManager.INSTANCE.CallStageFail();
        }           
    }
    void UpdateTracksCounter()
    {
        if (_tracksCount == ResourcesManager.INSTANCE.GetTracksNumber())
            return;

        _tracksCount = ResourcesManager.INSTANCE.GetTracksNumber();

        leftTracksLabel.text = rightTracksLabel.text = _tracksCount.ToString();
    }


    public void SetCountDownTimer(float min, float sec)
    {
        _minutes = min;

        if (_minutes < 0)
            _minutes = 0;

        _seconds = sec;
        _shouldCountDown = true;
    }

    void EnableScreenSideAccordingToCamera()
    {
        if (_hudContainerLeft == null || _hudContainerRight == null)
        {
            _hudContainerLeft = transform.FindChild("InGame HUD Left");
            _hudContainerRight = transform.FindChild("InGame HUD Right");
        }

        if (CameraControl.cameraLookAtSide == "Left")
        {
            _hudContainerLeft.gameObject.SetActive(false);
            _hudContainerRight.gameObject.SetActive(true);
        }
        else
        {
            _hudContainerRight.gameObject.SetActive(false);
            _hudContainerLeft.gameObject.SetActive(true);
        }

    }
}

