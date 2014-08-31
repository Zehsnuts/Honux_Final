using UnityEngine;
using System.Collections;

public class ConnectorFunctions : MonoBehaviour
{
    public ConnectionEnum.ConnectionType Connection;

    public Transform Origin;
    public Transform Destination;

    private Transform _originPoint;
    private Transform _destinationPoint;
    private Transform _frame;

    private CrystalConnection cc;

    public void InitializeConnection()
    {  
        Origin = transform.parent;
        cc = Origin.GetComponent<CrystalConnection>();
        Destination = cc.Destination;

        _originPoint = transform.FindChild("OriginPoint");
        _originPoint.position = Origin.position;
        
        _destinationPoint = transform.FindChild("DestinationPoint");
        _destinationPoint.position = Destination.position;

        _frame = transform.FindChild("Frame");
        _frame.position = Vector3.Lerp(_originPoint.position, _destinationPoint.position, 0.5f);

        GameObject b = Instantiate(Resources.Load("Prefabs/Connection/Frame"), _frame.transform.position, _frame.transform.rotation) as GameObject;
        b.transform.parent = _frame;
        b.name = "Frame";
        b.transform.LookAt(Destination.position);
    }
}
