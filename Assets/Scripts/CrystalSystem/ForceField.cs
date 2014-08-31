using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

    #region Events

    void OnEnable()
    {
        EventManager.GAMEPAUSESTART += PauseUnit;
        EventManager.GAMEPAUSEEXIT += UnpauseUnit;

        EventManager.BLUEPRINTSTART += PauseUnit;
        EventManager.BLUEPRINTEXIT += UnpauseUnit;

        EventManager.AUDIOSCREENSTART += PauseUnit;
        EventManager.AUDIOSCREENEXIT += UnpauseUnit;

        EventManager.STAGEFAIL += PauseUnit;
        EventManager.STAGESUCESS += PauseUnit;

    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= PauseUnit;
        EventManager.GAMEPAUSEEXIT -= UnpauseUnit;

        EventManager.BLUEPRINTSTART -= PauseUnit;
        EventManager.BLUEPRINTEXIT -= UnpauseUnit;

        EventManager.AUDIOSCREENSTART -= PauseUnit;
        EventManager.AUDIOSCREENEXIT -= UnpauseUnit;

        EventManager.STAGEFAIL -= PauseUnit;
        EventManager.STAGESUCESS -= PauseUnit;
    }
    #endregion



    public enum ForceFieldStatus
    { 
        Inactive, 
        Active,
        Solved,
        Paused
    }

    public ForceFieldStatus FieldStatus;
    private ForceFieldStatus _lastState;

    public GameObject RhythmButton;
    private ForceField_Button _myRhythmButtonScript;

    private GameObject _light;
    private GameObject _sphere;
    private bool _isVunerable;
    private bool _isRhythmActive;

    private float _forceFieldLife = 3;

    private AudioSource _audioSource;
    private AudioClip _rhythmAudioClip;

    void Start()
    { 
        if(RhythmButton !=null)
        {
            _myRhythmButtonScript = RhythmButton.GetComponent<ForceField_Button>();
            _myRhythmButtonScript.InitializeButton(gameObject);
        }
        else 
            Debug.LogError("Force field has no button assigned.");

        _rhythmAudioClip = Resources.Load("Sounds/Rhythm") as AudioClip;
        _audioSource = transform.GetComponent<AudioSource>();

        _light = transform.FindChild("Light").gameObject;
        _sphere = transform.FindChild("ForceFieldSphere").gameObject;
         
    }


    public void StopRhythmSequence()
    {
        FieldStatus = ForceFieldStatus.Inactive;

        //_light.renderer.material = Resources.Load("Materials/ForceField") as Material;

        _isRhythmActive = false;
    }

    public void BeginRhythmSequence()
    {
        FieldStatus = ForceFieldStatus.Active;

        _light.renderer.material = Resources.Load("Materials/Firula") as Material;

        _isRhythmActive = true;

        StartCoroutine(RhythmSequence());
    }

    public void ClickedForceField()
    {
        if (FieldStatus == ForceFieldStatus.Active)
        {
            if (_isVunerable)
                _forceFieldLife--;
            else
                _forceFieldLife++;

            if (_forceFieldLife > 3)
                _forceFieldLife = 3;

            Debug.Log(_forceFieldLife);
        }        
    }    

    void DestroyForceField()
    {
        _myRhythmButtonScript.ButtonSolved();

        Destroy(_sphere);
        Destroy(_light);

    }

    IEnumerator RhythmSequence()
    {
        while (_isRhythmActive && _forceFieldLife > 0)
        {
            _isVunerable = true;
            yield return StartCoroutine(Wait(_rhythmAudioClip.length));
        }

        _isRhythmActive = false;
        DestroyForceField();
    }

    IEnumerator Wait(float duration)
    {
        _audioSource.PlayOneShot(_rhythmAudioClip);
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            if (timer > duration / 4)
                _isVunerable = false;

            if (_forceFieldLife < 1)
                DestroyForceField();

            yield return 0;
        }
    }


    void PauseUnit()
    {
        _lastState = FieldStatus;

        FieldStatus = ForceFieldStatus.Paused;

        StopRhythmSequence();

        StopAllCoroutines();
    }

    void UnpauseUnit()
    {
        FieldStatus = _lastState;
    }
}
