﻿using UnityEngine;
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
            Destroy(this);
        }
        else
            Debug.LogError("Crystal connection doesn't have a destination.");
    }

    void SetConnectionAttributes()
    { 
        gameObject.AddComponent<CrystalConnection>();
        cc = gameObject.GetComponent<CrystalConnection>();

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            cc.Connection = ConnectionEnum.ConnectionType.Fixed;
        else
            cc.Connection = ConnectionEnum.ConnectionType.Temporary;

        cc.Destination = Destination;
    }

    void CreateConnection()
    {   
        transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination.gameObject);

        var go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;

        go.transform.parent = transform;

        go.name = "Track: " + transform.name + " to " + Destination.name;

        go.GetComponent<ConnectorFunctions>().InitializeConnection();
        //go.GetComponent<LineDrawer>().DrawLineFromTo(a, b, str);

        GlobalFunctions.ConnectThisLineWithParent(transform.gameObject, go);
    }

    public void CreateConnectionAtRunTime(Transform destination, ConnectionEnum.ConnectionType type)
    {
        Destination = destination;
        Connection = type;                
    }
}
