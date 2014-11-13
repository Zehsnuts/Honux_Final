using UnityEngine;
using System.Collections;

public static class ConnectionsUtils  
{
    public static void CreateConnection(this CrystalConnection crystalConnection, bool shouldTurnTrackOn = false)
    {
        Utils utils = new Utils();

        GameObject go = utils.InstantiateConnection(crystalConnection.transform);

        go.transform.parent = crystalConnection.transform;

        go.name = "Track: " + crystalConnection.transform.name + " to " + crystalConnection.Destination.name;

        go.GetComponent<ConnectorFunctions>().InitializeConnection(crystalConnection, shouldTurnTrackOn);

        GlobalFunctions.ConnectThisLineWithParent(crystalConnection, go);

        crystalConnection.transform.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(crystalConnection.Destination.gameObject);
        crystalConnection.Destination.GetComponent<CrystalUnitFunctions>().ConnectSingleUnit(crystalConnection.gameObject);
    }

    public static void Connection_CheckIfConnectionsNeedToChangeParent(this CrystalsUnit crystalUnit, ConnectorFunctions trackConnector)
    {
        if (trackConnector.Origin.GetComponent<CrystalsUnit>().isThisSystemOn || trackConnector.isTrackOn)
            return;

        GlobalFunctions.BreakThisLine(trackConnector.gameObject);

        crystalUnit.gameObject.AddComponent<ConnectionCreator>().ChangeFixedConnectionParent(trackConnector.gameObject);
    }


    class Utils : MonoBehaviour
    {
        public GameObject InstantiateConnection(Transform crystalConnectionTransform)
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), crystalConnectionTransform.position, crystalConnectionTransform.rotation) as GameObject;

            return go;
        }
    }
}
