using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalConnection : MonoBehaviour 
{
    public enum ConnectionType
    { 
        Fixed, Temporary
    }

    public ConnectionType Connection;

    private GameObject Origin;
    public GameObject Destination;

    void Awake()
    { 
        Origin = gameObject;
        transform.FindChild("OriginPoint").transform.position = Origin.transform.position;

        if (Origin != null && Destination != null)
            LineCreatorByInput.INSTANCE.EditorModeLineCreator(Origin, Destination, Connection.ToString());
    }

    public void SetConnectionAttributes(GameObject destination)
    {       
        Destination = destination;
        transform.FindChild("DestinationPoint").transform.position = Destination.transform.position;

        transform.FindChild("Frame").transform.LookAt(Destination.transform.position);

        Connection = ConnectionType.Temporary;
    }
}
