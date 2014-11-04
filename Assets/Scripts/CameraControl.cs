using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    private GameObject _actualCamera;
    private GameObject _mainCamera;

    public static string cameraLookAtSide;

    void Start()
    {
        _actualCamera = GameObject.Find("ActualCamera");
        _mainCamera = Camera.main.gameObject;

        cameraLookAtSide = "Left";
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
        if (cameraLookAtSide == "Left")
        {
            iTween.RotateTo(_actualCamera, iTween.Hash("y", 90));
            iTween.RotateTo(_mainCamera, iTween.Hash("y", 90));

            cameraLookAtSide = "Right";
        }
        else            
            {
                iTween.RotateTo(_actualCamera, iTween.Hash("y", 0));
                iTween.RotateTo(_mainCamera, iTween.Hash("y", 0));

                cameraLookAtSide = "Left";
            }
    }
}
