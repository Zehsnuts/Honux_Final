using UnityEngine;
using System.Collections;

public class ConnectionCreator : MonoBehaviour {

    public ConnectionEnum.ConnectionType Connection;

    public Transform Destination;

    CrystalConnection cc;

    void Awake()
    {
        if (Destination != null)                  
            SetConnectionAttributes(); 
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

    public void CreateConnectionAtRunTime(Transform destination, ConnectionEnum.ConnectionType type, bool shouldTurnOnAfterCreating = false)
    {
        this.Destination = destination;
        this.Connection = type;

        SetConnectionAttributes();

        cc.CreateConnection(shouldTurnOnAfterCreating);

        Destroy(this);
    }

    public void ChangeFixedConnectionParent(GameObject track)
    {
        var trackCrystal = track.GetComponent<ConnectorFunctions>();

        this.Destination = trackCrystal.Origin;
        this.Connection = trackCrystal.Connection;

        GlobalFunctions.BreakThisLine(track);

        SetConnectionAttributes();

        cc.CreateConnection();
        Destroy(this);
    }
}
