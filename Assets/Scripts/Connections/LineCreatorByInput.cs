using UnityEngine;
using System.Collections;

public class LineCreatorByInput : MonoBehaviour {

    #region Singleton

    private static LineCreatorByInput _INSTANCE;

    public static LineCreatorByInput INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<LineCreatorByInput>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    public GameObject Origin;
    public GameObject Destination;

    private bool _isCreatingLine = false;

    void Update()
    {
        if (_isCreatingLine)
            LineToMouseLocation();
    }

    void LineToMouseLocation()
    {
        LineManager.INSTANCE.LineDrawerForMouse(Origin.transform);
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
            _isCreatingLine = true;
            Origin = go;
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

            if(GlobalFunctions.CheckIfConnectionIsPossible(Origin.transform,Destination.transform))
                StartLineCreation();
            else 
                CancelLineCreation();
        }
    }

    void StartLineCreation()
    {
        _isCreatingLine = false;
        Origin.transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination);

        LineManager.INSTANCE.CreateLineDrawer(Origin.transform, Destination.transform, "Temp");

        ResourcesManager.INSTANCE.RemoveTrack();         

        CancelLineCreation();
    }

    public void EditorModeLineCreator(GameObject origin, GameObject destination, string type)
    {
        

        //LineManager.INSTANCE.CreateLineDrawer(origin.transform, destination.transform, type);        
    }    

    public void CancelLineCreation()
    {
        _isCreatingLine = false;
        Origin = null;
        Destination = null;
        LineManager.INSTANCE.CancelLineDrawerForMouse();
    }
}
