using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSystem_Options : MonoBehaviour 
{

    #region Singleton

    private static MenuSystem_Options _INSTANCE;

    public static MenuSystem_Options INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<MenuSystem_Options>();
            }
            return _INSTANCE;
        }
    }
   

    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
        }
        else
        {
            if (this != _INSTANCE)
                Destroy(this.gameObject);
        }
    }
    #endregion

    Material White;
    Material Black;

    private GameObject _btnDisplayResolutionNext;
    private GameObject _btnDisplayResolutionPrev;
    private TextMesh _txtDisplayResolution;
    private GameObject _btnApply;
    private GameObject _btnReturn;


    private int _resolutionIndex;
    private List<Resolution> _availableResolutions = new List<Resolution>();
    

    void Start()
    {
        GetOptionsComponents();
        GetAvailableResolutions();
    }

    void GetOptionsComponents()
    {
        White = Resources.Load("Materials/White") as Material;
        Black = Resources.Load("Materials/Black") as Material;

        _btnDisplayResolutionNext = transform.FindChild("ResolutionButtonNext").gameObject;
        _btnDisplayResolutionPrev = transform.FindChild("ResolutionButtonPrev").gameObject;

        _txtDisplayResolution = transform.FindChild("DisplayResolution").GetComponent<TextMesh>();
        //_txtDisplayResolution.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;

        _btnApply = transform.FindChild("btn_Apply").gameObject;
        _btnApply = transform.FindChild("btn_Return").gameObject;

    }

    void GetAvailableResolutions()
    {
        _availableResolutions = ResolutionManager.GetCompatibleResolutions();

        for (int i = 0; i<_availableResolutions.Count;i++)
        {
            if (_availableResolutions[i].width == Screen.currentResolution.width)
            {
                if(_availableResolutions[i].height == Screen.currentResolution.height)
                _txtDisplayResolution.text = _availableResolutions[i].width + "x" + _availableResolutions[i].height;
                _resolutionIndex = i;
            }
        }
        
    }

    public void HoveringOption(RaycastHit hit)
    { 
        switch(hit.transform.name)
        {
            case "ResolutionButtonNext":
                _btnDisplayResolutionNext.renderer.material = White;
                HoveringNextResolutionOptions();
                break;

            case "ResolutionButtonPrev":
                _btnDisplayResolutionPrev.renderer.material = White;
                HoveringPrevResolutionOptions();
                break;  

            case "btn_Apply":
                _btnApply.renderer.material = White;
                if(Input.GetMouseButtonUp(0))
                    EffectChanges();
                break;

            case "btn_Return":
                _btnApply.renderer.material = White;
                break;

           default:
                _btnDisplayResolutionNext.renderer.material = Black;
                _btnDisplayResolutionPrev.renderer.material = Black;
                _btnApply.renderer.material = Black;
                break;
        }
    }

    void HoveringNextResolutionOptions()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _resolutionIndex++;
            if (_resolutionIndex > _availableResolutions.Count -1)
                _resolutionIndex = 0;

            ChangeDisplayedResolution();
        }
    }

    void HoveringPrevResolutionOptions()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _resolutionIndex--;
            if (_resolutionIndex < 0)
                _resolutionIndex = _availableResolutions.Count-1;

            ChangeDisplayedResolution();
        }
    }

    void ChangeDisplayedResolution()
    {
        _txtDisplayResolution.text = _availableResolutions[_resolutionIndex].width + "x" + _availableResolutions[_resolutionIndex].height;
    }

    void EffectChanges()
    {
        if (_availableResolutions[_resolutionIndex].width != Screen.currentResolution.width)      
            if (_availableResolutions[_resolutionIndex].height != Screen.currentResolution.height)      
                ResolutionManager.ChangeResolution(_availableResolutions[_resolutionIndex].width, _availableResolutions[_resolutionIndex].height, Screen.fullScreen);

    }

    void oi()
    {
        var sec = 0;
        var min= 0;



    }

    string Madisca()
    { 
        string HorasPronta ="";



        return HorasPronta;
    }
}
