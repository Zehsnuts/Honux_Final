using UnityEngine;
using System.Collections;

public class Screen_BluePrint : MonoBehaviour
{
    
    #region Singleton

    private static Screen_BluePrint _INSTANCE;

    public static Screen_BluePrint INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<Screen_BluePrint>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    #region Events
    void OnEnable()
    {
        EventManager.BLUEPRINTSTART += ChangeScreenDisplay;
        EventManager.BLUEPRINTEXIT += ChangeScreenDisplay;

    }

    void OnDisable()
    {
        EventManager.BLUEPRINTSTART -= ChangeScreenDisplay;
        EventManager.BLUEPRINTEXIT -= ChangeScreenDisplay;
    }
    #endregion

    private Vector3 _initialPosition;
    private Vector3 _shownPosition;

    private bool _isShowingScreen = false;

    void Start()
    {
        _initialPosition = transform.position;
        _shownPosition = new Vector3(_initialPosition.x, _initialPosition.y + 24, _initialPosition.z);
    }

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

    void HideScreen()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _initialPosition.x, "y", _initialPosition.y, "z", _initialPosition.z));
    }

    void ShowScreen()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _shownPosition.x, "y", _shownPosition.y, "z", _shownPosition.z));
    }

    public void AddMark(Transform t, Vector3 pos)
    {
        Debug.Log("Add Mark");
        GameObject b = Instantiate(Resources.Load("Prefabs/BluePrintMark"), pos, t.localRotation) as GameObject;
        b.transform.parent = t;
    }
}
