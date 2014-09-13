using UnityEngine;
using System.Collections;

public class MouseFunctions : MonoBehaviour
{
    #region Singletone

    private static MouseFunctions _INSTANCE;
    public static MouseFunctions INSTANCE
    {
        get 
        {
            if (_INSTANCE == null)
                _INSTANCE = FindObjectOfType<MouseFunctions>();
            return _INSTANCE;
        }
    }

    #endregion

    public GameObject mouseCursor;
    public GameObject Ear;
    private Vector3 _earStartginPosition;
    public Camera ActualCamera;
    public float _earDistance;    

    void Start()
    {
       GrabStuff();        
    }

    void GrabStuff()
    {
        _earDistance = 23f;

        if (!GameObject.Find("MouseCursor"))
        {
            var go = new GameObject();
            go.name = "MouseCursor";
            mouseCursor = go;
        }

        //if (GameObject.Find("Ear"))
        //{
            Ear = GameObject.Find("Ear");
           // Destroy(Ear);
        //}


        //Ear = Instantiate(Resources.Load("Prefabs/Ear")) as GameObject;

        _earStartginPosition = Ear.transform.position;

        ActualCamera = GameObject.Find("ActualCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (ActualCamera == null)
            GrabStuff();

        MoveMouseCursor();

        Ray ray = ActualCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {           
                switch (GameStateManager.INSTANCE.SpecialGameState)
                {
                    case GameStateManager.SpecialGameStates.FreeInput:
                        FreeInput(hit);
                        break;

                    case GameStateManager.SpecialGameStates.OnPause:
                        OnPauseInput(hit);
                        break;

                    case GameStateManager.SpecialGameStates.OnStageSelect:
                        OnStageSelectInput(hit);
                        break;

                    case GameStateManager.SpecialGameStates.OnMenu:
                        OnStartMenuInput();
                        break;

                    case GameStateManager.SpecialGameStates.OnCutscene:
                        OnCutsceneInput();
                        break;

                    case GameStateManager.SpecialGameStates.OnBluePrint:
                        OnBluePrintInput(hit);
                        break;

                    case GameStateManager.SpecialGameStates.OnAudioScreen:
                        OnAudioScreenInput();
                        break;
                    case GameStateManager.SpecialGameStates.OnFeedBack:
                        OnFeedbackScreenInput(hit);
                        break;
                }  
        }
    }
    
    void FreeInput(RaycastHit hit)
    {
        _earDistance = 22f;

        MoveEar();

        Transform t = hit.transform;

        if (Input.GetMouseButtonUp(0))
        {            
            if (t.GetComponent<CrystalUnitFunctions>())
            {
                if (ResourcesManager.INSTANCE.GetTracksNumber() > 0)
                {
                    ConnectionFunctions.INSTANCE.AssignOriginAndDestination(t.gameObject);
                }
                /*if (LineCreatorByInput.INSTANCE.ReadyToCreateLine() && ResourcesManager.INSTANCE.GetTracksNumber()>0)
                    LineCreatorByInput.INSTANCE.AssignOriginAndDestination(t.gameObject);    
                 * */
            }
            else
                ConnectionFunctions.INSTANCE.CancelLineCreation();

             if (t.name == "ForceFieldSphere")
                t.parent.GetComponent<ForceField>().ClickedForceField();

            else if (t.name == "ButtonCore")
                t.parent.GetComponent<ForceField_Button>().ClickForceFieldButton();

            else if (t.name == "Frame")
                t.parent.parent.GetComponent<ConnectorFunctions>().BreakLine();
             else if (t.name == "ExtendedFrame")
                 t.parent.GetComponent<ConnectorFunctions>().BreakLine();

            else if (t.name == "BluePrintButton")
                EventManager.INSTANCE.CallBluePrintStart();

            else if (t.name == "AudioScreenButton")
                EventManager.INSTANCE.CallAudioScreenStart();
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (t.name == "StageFloor")
                Skill_StunRay.INSTANCE.CastStunRay(hit.point);

        }
    }

    void OnPauseInput(RaycastHit hit)
    {
        Ear.transform.position = _earStartginPosition;

        if (hit.transform.name != "Plane")
        {
            if (Input.GetMouseButtonUp(0))
            {
                Screen_Pause.INSTANCE.ClickedThisButton(hit.transform.name);
            }
        }
    }

    void OnStartMenuInput()
    {

    }

    void OnStageSelectInput(RaycastHit hit)
    {
        if (Input.GetMouseButtonUp(0))
        {
            string[] str = hit.transform.name.Split("_"[0]);

            if (str[0] == "Stage")
                StageManager.INTANCE.ChangeLevelTo(hit.transform.name);

            else
                if (hit.transform.name == "btn_QuitGame")
                    Application.Quit();
        }
    }

    void OnCutsceneInput()
    {
        Ear.transform.position = _earStartginPosition;
    }

    void OnBluePrintInput(RaycastHit hit)
    {
        MoveEar();

        string[] name = hit.transform.name.Split("_"[0]);

        if (Input.GetMouseButtonUp(0))
        {
            //if (hit.transform.name == "BluePaper")
            if (name[0] == "BluePaper")

                Screen_BluePrint.INSTANCE.AddMark(hit.transform, hit.point);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (hit.transform.name == "MarkLeg")
                Destroy(hit.transform.parent.gameObject);
        }
    }

    void OnAudioScreenInput()
    {
        _earDistance = 15f;
        MoveEar();

    }

    void OnFeedbackScreenInput(RaycastHit hit)
    {
        Ear.transform.position = _earStartginPosition;

        if (hit.transform.name != "Plane")
        {
            if (Input.GetMouseButtonUp(0))
            {
                Screen_Feedback.INSTANCE.ClickedThisButton(hit.transform.name);
            }
        }
    }

    public Vector3 MoveMouseCursor()
    {
        var mousePos = Input.mousePosition;
        var wantedPos = ActualCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 23));

        mouseCursor.transform.position = wantedPos;

        return wantedPos;
    }

    void MoveEar()
    {
        var mousePos = Input.mousePosition;
        var wantedPos = ActualCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _earDistance));
        Ear.transform.position = wantedPos;
    }
}
