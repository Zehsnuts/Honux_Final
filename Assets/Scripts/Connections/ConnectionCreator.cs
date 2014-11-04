using UnityEngine;
using System.Collections;

public class ConnectionCreator : MonoBehaviour {

    public ConnectionEnum.ConnectionType Connection;

    public Transform Destination;

    CrystalConnection cc;

    void Start()
    {
        if (this.Destination != null)
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
        this.cc = gameObject.AddComponent<CrystalConnection>();         

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            cc.Connection = ConnectionEnum.ConnectionType.Fixed;
        else
            cc.Connection = ConnectionEnum.ConnectionType.Temporary;

        cc.Destination = this.Destination;
    }

    void CreateConnection()
    {   
        GameObject go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        go.transform.parent = transform;

        go.name = "Track: " + transform.name + " to " + this.Destination.name;

        go.GetComponent<ConnectorFunctions>().InitializeConnection(cc);

        GlobalFunctions.ConnectThisLineWithParent(transform.gameObject, go);

        transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(this.Destination.gameObject);

    }

    public void CreateConnectionAtRunTime(Transform destination, ConnectionEnum.ConnectionType type)
    {
        this.Destination = destination;
        this.Connection = type;       
    }
}
