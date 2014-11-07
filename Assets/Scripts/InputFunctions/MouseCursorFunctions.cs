using UnityEngine;
using System.Collections;

public class MouseCursorFunctions : MonoBehaviour
{
    public Camera nguiCamera;

    private UITexture _cursorUITexture;

    private Texture _cursorCurrentTexture;
    private Texture _cursorNormal;
    private Texture _cursorPressed;

    public Texture cursorNormal1;
    public Texture cursorPressed1;

    void Start()
    {
        Screen.showCursor = false;

        _cursorUITexture = transform.GetComponent<UITexture>();

        _cursorNormal = cursorNormal1;
        _cursorPressed = cursorPressed1;
    }

    void ChangeMouseCursor(string mouseInput)
    {
        if(mouseInput == "Up")
            _cursorCurrentTexture = _cursorNormal;
        else if(mouseInput == "Down")
            _cursorCurrentTexture = _cursorPressed;


        _cursorUITexture.mainTexture = _cursorCurrentTexture;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ChangeMouseCursor("Down");
        if (Input.GetMouseButtonUp(0))
            ChangeMouseCursor("Up");

        Vector2 mousePos = Input.mousePosition;

        var wantedPos = nguiCamera.ScreenToWorldPoint(Input.mousePosition);

        transform.position = wantedPos;

    }

}
