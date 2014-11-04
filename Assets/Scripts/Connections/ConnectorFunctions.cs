using UnityEngine;
using System.Collections;

public class ConnectorFunctions : MonoBehaviour
{
    public ConnectionEnum.ConnectionType Connection;

    public Transform Origin;
    public Transform Destination;

    public void InitializeConnection(CrystalConnection cristalConnection)
    {
        Connection = cristalConnection.Connection;

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.AddComponent<Connection_Fixed>().InitializeConnection(cristalConnection);
        else
            gameObject.AddComponent<Connection_Temporary>().InitializeConnection(cristalConnection);
    }

    public void BreakLine()
    {
        if (Connection == ConnectionEnum.ConnectionType.Temporary)
            gameObject.GetComponent<Connection_Temporary>().BreakLine();
    }

    public void TurnTrackOn()
    {
        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOn();
        else
            gameObject.GetComponent<Connection_Temporary>().TurnTrackOn();
    }

    public void TurnTrackOff()
    {
        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOff();
        else
            gameObject.GetComponent<Connection_Temporary>().TurnTrackOff();           
    }
}
