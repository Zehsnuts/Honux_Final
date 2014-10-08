using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    private GameObject _actualCamera;
    private GameObject _mainCamera;

    private string _camPosition;

    void Start()
    {
        _actualCamera = GameObject.Find("ActualCamera");
        _mainCamera = Camera.main.gameObject;

        _camPosition = "Left";
    }

    void OnEnable()
    {
        EventManager.CHANGECAMERA += ChangeCamera;
    }

    void OnDisable()
    {
        EventManager.CHANGECAMERA -= ChangeCamera;
    }

    void ChangeCamera()
    {
        if (_camPosition == "Left")
        {
            iTween.RotateTo(_actualCamera, iTween.Hash("y", 90));
            iTween.RotateTo(_mainCamera, iTween.Hash("y", 90));

            _camPosition = "Right";
        }
        else            
            {
                iTween.RotateTo(_actualCamera, iTween.Hash("y", 0));
                iTween.RotateTo(_mainCamera, iTween.Hash("y", 0));

                _camPosition = "Left";
            }

        //Debug.Log("Changing Camera");
    }
}
