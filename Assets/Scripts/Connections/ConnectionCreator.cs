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
            if (Vector3.Distance(transform.position, Destination.position) < 10)
                CreateConnection();
            else
                CreateExtendedConnection();
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
        else if (Connection == ConnectionEnum.ConnectionType.Temporary)
            cc.Connection = ConnectionEnum.ConnectionType.Temporary;
        else if (Connection == ConnectionEnum.ConnectionType.ExtendedFixed)
            cc.Connection = ConnectionEnum.ConnectionType.ExtendedFixed;
        else if (Connection == ConnectionEnum.ConnectionType.ExtendedTemporary)
            cc.Connection = ConnectionEnum.ConnectionType.ExtendedTemporary;

        cc.Destination = Destination;
    }

    void CreateConnection()
    {   
        transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination.gameObject);

        GameObject go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;

        AssociateLineWithParent(go);
    }

    void CreateExtendedConnection()
    {
        transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(Destination.gameObject);


        //Criar 2 extensores. Posisionar o primeiro de acordo com Origin e o segundo de acordo com Destination. 3 frames pra fazer a conexão.
        GameObject go = new GameObject();
        go.name = transform.name + "'s extended connector to " + Destination.name;

        go.transform.position = Vector3.Lerp(transform.position, Destination.transform.position, 0.5f);
        go.transform.position = new Vector3(go.transform.position.x, transform.GetPositionY(), go.transform.position.z);

        GameObject line1 = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        AssociateLineWithParent(line1);


        GameObject line2 = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        AssociateLineWithParent(line2);

        line1.GetComponent<ConnectorFunctions>().InitializeExtendedConnection(go.transform, "Origin");
        go.transform.parent = line1.transform;

        line2.GetComponent<ConnectorFunctions>().InitializeExtendedConnection(go.transform, "");

        line2.transform.parent = line1.transform;
    }

    void AssociateLineWithParent(GameObject line)
    {
        line.transform.parent = transform;

        line.name = "Track: " + transform.name + " to " + Destination.name;

        if (Connection != ConnectionEnum.ConnectionType.ExtendedTemporary && Connection != ConnectionEnum.ConnectionType.ExtendedFixed)
        line.GetComponent<ConnectorFunctions>().InitializeConnection();

        GlobalFunctions.ConnectThisLineWithParent(transform.gameObject, line);
    }

    public void CreateConnectionAtRunTime(Transform destination, ConnectionEnum.ConnectionType type)
    {
        Destination = destination;
        Connection = type;       
    }
}
