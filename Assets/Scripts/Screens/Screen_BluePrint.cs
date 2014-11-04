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

    private Texture _currentSelectedStampTexture;
    private GameObject _stampContainer;

    private GameObject _screenSideLeft;
    private GameObject _screenSideRight;

    public GameObject selectedStampIndicator;

    void Start()
    {
        _screenSideLeft = transform.FindChild("Blueprint Left").gameObject;
        _screenSideRight = transform.FindChild("Blueprint Right").gameObject;

        EventManager.CHANGECAMERA += EnableScreenSideAccordingToCamera;
    }

    void OnDisable()
    {
        _stampContainer = null;
        selectedStampIndicator.SetActive(false);

        if (FindObjectOfType<EventManager>())
            EventManager.INSTANCE.CallBluePrintExit();
    }

    void OnEnable()
    {
        if(FindObjectOfType<EventManager>())
            EventManager.INSTANCE.CallBluePrintStart();

        if (selectedStampIndicator == null)
            selectedStampIndicator = transform.FindChild("texture_stampsSelected").gameObject;
        _stampContainer = null;
        selectedStampIndicator.SetActive(false);

        _screenSideLeft = transform.FindChild("Blueprint Left").gameObject;
        _screenSideRight = transform.FindChild("Blueprint Right").gameObject;

        EnableScreenSideAccordingToCamera();
    }

    public void AddMark(Transform t)
    {
        if (_stampContainer == null || UICamera.currentTouchID == -2)
            return;

        GameObject b = Instantiate(_stampContainer, UICamera.lastHit.point, t.localRotation) as GameObject;
        b.GetComponent<UITexture>().width = 58;
        b.GetComponent<UITexture>().height = 58;        
        b.transform.parent = t;
        b.transform.localScale = new Vector3(1, 1, 1);
    }

    public void RemoveMark(GameObject go)
    {
        if ( UICamera.currentTouchID == -2)
            Destroy(go);
    }

    public void ChangeSelectedStamp(Transform stamp)
    {
        selectedStampIndicator.SetActive(true);
        selectedStampIndicator.transform.position = stamp.transform.position;

        if(_stampContainer == null)
            _stampContainer = transform.FindChild("texture_stampContainer").gameObject;

        _stampContainer.GetComponent<UITexture>().mainTexture = _currentSelectedStampTexture = stamp.GetComponent<UITexture>().mainTexture;
    }

    void EnableScreenSideAccordingToCamera()
    {
        selectedStampIndicator.transform.position = new Vector3(-2000,180,0);
        _stampContainer = null;

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
