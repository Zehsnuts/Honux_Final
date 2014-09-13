using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour
{

    #region Singleton
    private static SoundManager _INSTANCE;
    public static SoundManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<SoundManager>();
                //DontDestroyOnLoad(_INSTANCE.gameObject);                
            }
            return _INSTANCE;
        }
    }

    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            DontDestroyOnLoad(this);
            //LoadAudioResources();
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
        EventManager.STAGESUCESS += PositiveFeedBackSound;
        EventManager.STAGEFAIL += NegativeFeedBackSound;

        EventManager.ROBOTCREATION += RobotWarning;
        EventManager.ROBOTCREATIONSTOP += RobotWarningStop;

        EventManager.STAGEFAILSEQUENCE += StageFailWarningBegin;
        EventManager.STAGEFAILSEQUENCESTOP += StageFailWarningStop;

        EventManager.STAGEFAIL += StopAllSounds;
    }

    void OnDisable()
    {
        EventManager.STAGESUCESS -= PositiveFeedBackSound;
        EventManager.STAGEFAIL -= NegativeFeedBackSound;

        EventManager.ROBOTCREATION -= RobotWarning;
        EventManager.ROBOTCREATIONSTOP -= RobotWarningStop;

        EventManager.STAGEFAILSEQUENCE -= StageFailWarningBegin;
        EventManager.STAGEFAILSEQUENCESTOP -= StageFailWarningStop;

        EventManager.STAGEFAIL -= StopAllSounds;
    }   

    #endregion

    private static string AUDIOPATH = "Sounds/";

    private GameObject _mouseObject;

    private AudioSource _globalSounds;
    private SECTR_AudioSource _stunSfx;
    private SECTR_AudioSource _alarmSfx;
    private SECTR_AudioSource _robotAlarmSfx;
    private SECTR_AudioSource _robotCollectSfx;

    private SECTR_AudioBus _masterBus;
    private SECTR_AudioBus _sfxBus;
    private SECTR_AudioBus _musicBus;

    private AudioClip _feedbackSucced;
    private AudioClip _feedbackFail;   

    void Start()
    {        
        GrabAudioSources();
        LoadAudioFiles();        
    }

    void GrabAudioSources()
    {
        _globalSounds = Camera.main.GetComponent<AudioSource>();

        _globalSounds = GameObject.Find("ActualCamera").GetComponent<AudioSource>();


        if (GameObject.Find("StunSFX"))
            _stunSfx = GameObject.Find("StunSFX").GetComponent<SECTR_AudioSource>();

        if (GameObject.Find("AlarmSFX"))
            _alarmSfx = GameObject.Find("AlarmSFX").GetComponent<SECTR_AudioSource>();

        if (GameObject.Find("RobotAlarmSFX"))
            _robotAlarmSfx = GameObject.Find("RobotAlarmSFX").GetComponent<SECTR_AudioSource>();

        if (GameObject.Find("RobotCollectSFX"))
            _robotCollectSfx = GameObject.Find("RobotCollectSFX").GetComponent<SECTR_AudioSource>();        
        
    }

    void LoadAudioFiles()
    {
        _feedbackSucced = Resources.Load(AUDIOPATH + "Tada") as AudioClip;
        _feedbackFail = Resources.Load(AUDIOPATH + "Fail") as AudioClip;

    }

    void PlaySingleFile(AudioClip clip)
    {
        _globalSounds.PlayOneShot(clip);
    }

    void PositiveFeedBackSound()
    {
        PlaySingleFile(_feedbackSucced);
    }

    void NegativeFeedBackSound()
    {
        PlaySingleFile(_feedbackFail);
    }

    void RobotWarning()
    {
        _robotAlarmSfx.Stop(true);
        _robotAlarmSfx.Play();
    }

    void RobotWarningStop()
    {        
        _robotAlarmSfx.Stop(true);
    }

    void StageFailWarningBegin()
    {
        _alarmSfx.Stop(true);
        _alarmSfx.Play();
    }

    void StageFailWarningStop()
    {
        _alarmSfx.Stop(true);
    }

    public void PlaysSingleFileByName(string soundname, Vector3 pos)
    {
        switch (soundname)
        {
            case "Stun":
                _stunSfx.transform.position = pos;
                _stunSfx.Stop(true);
                _stunSfx.Play();
                break;
        }
    }

    void ResumeAllSounds()
    {
        
    }

    void StopAllSounds()
    {
        _stunSfx.Stop(true);
        _alarmSfx.Stop(true);
        _robotCollectSfx.Stop(true);
        _robotAlarmSfx.Stop(true);
    }

}
