using UnityEngine;
using System.Collections;

public class ConnectionCreator : MonoBehaviour {

    public ConnectionEnum.ConnectionType Connection;

    public Transform Destination;

    CrystalConnection cc;

    void Start()
    {
        if (Destination != null)
        {            
            SetConnectionAttributes();
            CreateConnection();          
        }
        else
            Debug.LogError("Crystal connection doesn't have a destination.");

        Destroy(this);
    }

    void SetConnectionAttributes()
    { 
       cc= gameObject.AddComponent<CrystalConnection>();         

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            cc.Connection = ConnectionEnum.ConnectionType.Fixed;
        else if (Connection == ConnectionEnum.ConnectionType.Temporary)
            cc.Connection = ConnectionEnum.ConnectionType.Temporary;

        cc.Destination = Destination;
    }

    void CreateConnection()
    {   
        GameObject go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        go.transform.parent = transform;

        go.name = "Track: " + transform.name + " to " + Destination.name;

        go.GetComponent<ConnectorFunctions>().InitializeConnection();

        GlobalFunctions.ConnectThisLineWithParent(transform.gameObject, go);

        transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination.gameObject);
    }

    public void CreateConnectionAtRunTime(Transform destination, ConnectionEnum.ConnectionType type)
    {
        Destination = destination;
        Connection = type;       
    }
}
