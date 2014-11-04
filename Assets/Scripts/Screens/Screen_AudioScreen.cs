using UnityEngine;
using System.Collections;

public class Screen_AudioScreen : MonoBehaviour
{

    #region Singleton

    private static Screen_AudioScreen _INSTANCE;

    public static Screen_AudioScreen INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<Screen_AudioScreen>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    private GameObject _screenSideLeft;
    private GameObject _screenSideRight;


    void Start()
    {
        _screenSideLeft = transform.FindChild("Audio Left").gameObject;
        _screenSideRight = transform.FindChild("Audio Right").gameObject;

        EventManager.CHANGECAMERA += EnableScreenSideAccordingToCamera;
    }

    void OnDisable()
    {

        if (FindObjectOfType<EventManager>())
            EventManager.INSTANCE.CallAudioScreenExit();
    }

    void OnEnable()
    {
        if (FindObjectOfType<EventManager>())
            EventManager.INSTANCE.CallAudioScreenStart();

        _screenSideLeft = transform.FindChild("Audio Left").gameObject;
        _screenSideRight = transform.FindChild("Audio Right").gameObject;

        EnableScreenSideAccordingToCamera();
    }


    void EnableScreenSideAccordingToCamera()
    { 
        if (CameraControl.cameraLookAtSide == "Left")
        {
            _screenSideLeft.SetActive(true);
            _screenSideRight.SetActive(false);
        }
        else
        {
            _screenSideRight.SetActive(true);
            _screenSideLeft.SetActive(false);
        }

    }
}
