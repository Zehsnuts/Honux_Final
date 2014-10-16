using UnityEngine;
using System.Collections;

public class ConnectorFunctions : MonoBehaviour
{
    public ConnectionEnum.ConnectionType Connection;

    public Transform Origin;
    public Transform Destination;

    public void InitializeConnection(CrystalConnection cristalConnection)
    {

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.AddComponent<Connection_Fixed>().InitializeConnection(cristalConnection);
        else
            gameObject.AddComponent<ConnectionInitializer_Temporary>().InitializeConnection(cristalConnection);

        //if(Origin.GetComponent<CrystalsUnit>().isThisSystemOn)
          //  TurnTrackOn();
        //else
          //  TurnTrackOff();
    }

    public void BreakLine()
    {
        /*
        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            return;

        Debug.Log("Destroy");

        ResourcesManager.INSTANCE.AddTrack();
        GlobalFunctions.BreakThisConnection(gameObject, transform.parent, Destination);

        Destroy(cc);

        Destroy(gameObject);
         * */
    }

    public void TurnTrackOn()
    {
        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOn();
        else
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOn();
    }

    public void TurnTrackOff()
    {
        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOff();
        else
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOff();
           
    }
}
