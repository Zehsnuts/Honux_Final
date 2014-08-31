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

    void OnEnable()
    {
        EventManager.GAMEPAUSESTART += StartCountDown;
        EventManager.GAMEPAUSEEXIT += StopCountDown;

        EventManager.STAGESUCESS += StopCountDown;
        EventManager.STAGEFAIL += StopCountDown;
    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= StopCountDown;
        EventManager.GAMEPAUSEEXIT -= StopCountDown;

        EventManager.STAGESUCESS -= StopCountDown;
        EventManager.STAGEFAIL -= StopCountDown;
    }

    private TextMesh _leftClock;
    private TextMesh _rightClock;
    private string _timeText;
    private float _minutes = 5;
    private float _seconds = 59;
    private bool _shouldCountDown = false;

    public List<Transform> _leftTracks = new List<Transform>();
    public List<Transform> _rightTracks = new List<Transform>();

    private int _tracksCount;

    void Start()
    {
        Transform holder = transform.FindChild("Left");

        _leftClock = holder.FindChild("Clock").GetComponent<TextMesh>();

        Transform track = holder.FindChild("Tracks");

        foreach (Transform t in track)
        {

            _leftTracks.Add(t);
            t.gameObject.active = false;
        }

        holder = transform.FindChild("Right");

        _rightClock = holder.FindChild("Clock").GetComponent<TextMesh>();

        track = holder.FindChild("Tracks");

        foreach (Transform t in track)
        {

            _rightTracks.Add(t);
            t.gameObject.active = false;
        }

        TracksCounter();

        ShowNumberOfTracks();

    }

    void Update()
    {
        if (_shouldCountDown)
            Cronometer();

        TracksCounter();
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

        _rightClock.text = _leftClock.text = _timeText;

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
    void TracksCounter()
    {
        if (_tracksCount == ResourcesManager.INSTANCE.GetTracksNumber())
            return;

            _tracksCount = ResourcesManager.INSTANCE.GetTracksNumber();
            ShowNumberOfTracks();
    }

    void ShowNumberOfTracks()
    {
        for (int i = 0; i < _leftTracks.Count; i++)
        {
            if (i < _tracksCount)
                _leftTracks[i].active = _rightTracks[i].active = true; 
            else
                _leftTracks[i].active = _rightTracks[i].active = false;    
        }
    }

    public void SetCountDownTimer(float min, float sec)
    {
        _minutes = min;

        if (_minutes < 0)
            _minutes = 0;

        _seconds = sec;
        _shouldCountDown = true;
    }
}

