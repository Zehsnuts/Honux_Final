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
    private bool _canCreate = false;

    private GameObject _lineFrame;
    private Transform _lightFollow;

    private Vector3 _initialPosition;

    void Awake()
    {  
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (_isCreatingConnection)
            PointFrameToMouse();
    }

    void PointFrameToMouse()
    {
        var dist = Vector3.Distance(MouseFunctions.INSTANCE.MoveMouseCursor(), Origin.transform.position);

        if (dist < 20)
        {
            _lightFollow.position = MouseFunctions.INSTANCE.MoveMouseCursor();
            _canCreate = true;
        }
        else
            _canCreate = false;

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

            LightTracking();
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

    void LightTracking()
    {
        if (_lineFrame == null)
        {
            _lineFrame = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Origin.transform.position, Origin.transform.rotation) as GameObject;
            _lineFrame.transform.parent = transform;

            var go = new GameObject();
            go.name = "LightFollow";
            _lightFollow = go.transform;
            _lightFollow.parent = transform;

            _lineFrame.GetComponent<LightningBolt>().target = _lightFollow;
        }
    }

    void StartLineCreation()
    {
        _isCreatingConnection = false;

        Origin.transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination);

        Origin.AddComponent<ConnectionCreator>().CreateConnectionAtRunTime(Destination.transform, ConnectionEnum.ConnectionType.Temporary);
        //ConnectionCreator.INSTANCE.CreateConnection(Origin.transform, Destination.transform, "Temp");

        ResourcesManager.INSTANCE.RemoveTrack();

        CancelLineCreation();
    }

    public void EditorModeLineCreator(GameObject origin, GameObject destination, string type)
    {
        if (origin != null && destination != null)
        {
            origin.transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(destination);
        }
    }

    public void CancelLineCreation()
    {
        _isCreatingConnection = false;
        Origin = null;
        Destination = null;

        transform.position = _initialPosition;

        Destroy(_lineFrame);
        if(_lightFollow != null)
        Destroy(_lightFollow.gameObject);
        //LineManager.INSTANCE.CancelLineDrawerForMouse();
    }
}
