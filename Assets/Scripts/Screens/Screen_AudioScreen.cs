using UnityEngine;
using System.Collections;

public class Screen_AudioScreen : MonoBehaviour {
    
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

    #region Events
    void OnEnable()
    {
        EventManager.CHANGECAMERA += ChangePosition;

        EventManager.AUDIOSCREENSTART += ChangeScreenDisplay;
        EventManager.AUDIOSCREENEXIT += ChangeScreenDisplay;

    }

    void OnDisable()
    {
        EventManager.CHANGECAMERA -= ChangePosition;

        EventManager.AUDIOSCREENSTART -= ChangeScreenDisplay;
        EventManager.AUDIOSCREENEXIT -= ChangeScreenDisplay;
    }   
    #endregion

    private Vector3 _initialPosition;
    private Vector3 _leftPosition;
    private Vector3 _rightPosition;
    private Vector3 _shownPosition;

    private bool _isLeft = true;
    private bool _isShowingScreen = false;


    void ChangeScreenDisplay()
    {
        if (_isShowingScreen)
        {
            _isShowingScreen = false;
            HideScreen();
        }
        else
        {
            _isShowingScreen = true;
            ShowScreen();
        }
    }

    void Start()
    {
        _initialPosition = new Vector3(-42.71157f, -25.022f, -27.80511f);
        _leftPosition = new Vector3(-6.5f, -0.9758148f, -23.08426f);
        _rightPosition = new Vector3(0.06050587f, -0.9758148f, -1.238088f);

        _shownPosition = _leftPosition;
    }

    void ChangePosition()
    {
        if (_isLeft)
        {
            _isLeft = false;
            _shownPosition = _rightPosition;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _isLeft = true;
            _shownPosition = _leftPosition;
            transform.rotation = Quaternion.Euler(0,-90,0);
        }
    }
    
    void HideScreen()
    {       
        iTween.MoveTo(gameObject, iTween.Hash("x", _initialPosition.x, "y", _initialPosition.y, "z", _initialPosition.z));
    }

    void ShowScreen()
    {        
        iTween.MoveTo(gameObject, iTween.Hash("x", _shownPosition.x, "y", _shownPosition.y, "z", _shownPosition.z));        
    }
	

}
