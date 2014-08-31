using UnityEngine;
using System.Collections;

public class ConnectionFunctions : MonoBehaviour
{

    #region Singleton

    private static ConnectionFunctions _INSTANCE;

    public static ConnectionFunctions INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<ConnectionFunctions>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    public GameObject Origin;
    public GameObject Destination;

    private bool _isCreatingConnection = false;

    private Transform _lineFrame;

    private Vector3 _initialPosition;

    void Awake()
    {
        _lineFrame = transform.FindChild("Frame");
        _initialPosition = transform.position;
    }

    void InitialState()
    {

    }

    void Update()
    {
        if (_isCreatingConnection)
            PointFrameToMouse();
    }

    void PointFrameToMouse()
    {
        _lineFrame.LookAt(MouseFunctions.INSTANCE.MoveMouseCursor());
        if (Input.GetKeyUp(KeyCode.Escape))
            CancelLineCreation();
    }

    public bool ReadyToCreateLine()
    {
        if (Origin == null || Destination == null)
            return true;

        else
            return false;
    }

    public void AssignOriginAndDestination(GameObject go)
    {
        if (Origin == null)
        {
            _isCreatingConnection = true;
            Origin = go;
            transform.position = Origin.transform.position;
        }
        else
            Destination = go;

        if (Origin != null && Destination != null && Origin != Destination)
        {
            if (!Origin.GetComponent<CrystalsUnit>().isThisSystemOn)
            {
                var aux = Origin;
                Origin = Destination;
                Destination = aux;
            }

            if (GlobalFunctions.CheckIfConnectionIsPossible(Origin.transform, Destination.transform))
                StartLineCreation();
            else
                CancelLineCreation();
        }
    }

    void StartLineCreation()
    {
        _isCreatingConnection = false;

        Origin.transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination);

        //ConnectionCreator.INSTANCE.CreateConnection(Origin.transform, Destination.transform, "Temp");

        //LineManager.INSTANCE.CreateLineDrawer(Origin.transform, Destination.transform, "Temp");

        ResourcesManager.INSTANCE.RemoveTrack();

        CancelLineCreation();
    }

    public void EditorModeLineCreator(GameObject origin, GameObject destination, string type)
    {
        if (origin != null && destination != null)
        {
            origin.transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(destination);
            LineManager.INSTANCE.CreateLineDrawer(origin.transform, destination.transform, type);
        }
    }

    public void CancelLineCreation()
    {
        _isCreatingConnection = false;
        Origin = null;
        Destination = null;

        transform.position = _initialPosition;
        //LineManager.INSTANCE.CancelLineDrawerForMouse();
    }
}
