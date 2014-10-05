using UnityEngine;
using System.Collections;

public class GlobalFunctions : MonoBehaviour {


    public static void CheckIfMyConnectionsHaveMe(GameObject go)
    {
        var connectionsList = go.GetComponent<CrystalsUnit>().ConnectedToMe;

        //Ele não pode se auto-conter na lista de conexões; Evita erro humano;
        if (connectionsList.Contains(go))
            connectionsList.Remove(go);

        //Para cada elemento que está conectado neste, verifica se este está conectado no elemento. Apenas para preencher a lista de conexões.
        for (int i = 0; i < connectionsList.Count; i++)
        {
            var connectedList = connectionsList[i].GetComponent<CrystalsUnit>().ConnectedToMe;
            if (!connectedList.Contains(go))
                connectedList.Add(go);
        }
    }

    public static void BreakThisConnection(GameObject line, Transform origin, Transform destination)
    {
        var originScript = origin.GetComponent<CrystalUnitFunctions>();
        var destinationScript = destination.GetComponent<CrystalUnitFunctions>();

        originScript.TracksOfDonatedEnergy.Remove(line);
        originScript.ConnectedToMe.Remove(destination.gameObject);
        originScript.SystemsThisDonatedEnergyTo.Remove(destination.gameObject);

        destinationScript.ConnectedToMe.Remove(origin.gameObject);    
        destinationScript.RemoveEnergy(origin.gameObject);
            
    }

    public static void BreakThisLine(GameObject line)
    {
        if (line.GetComponent<ConnectorFunctions>().Connection == ConnectionEnum.ConnectionType.Fixed)
            return;

        var origin = line.GetComponent<ConnectorFunctions>().Origin;
        var destination = line.GetComponent<ConnectorFunctions>().Destination;

        var originScript = origin.GetComponent<CrystalUnitFunctions>();
        var destinationScript = destination.GetComponent<CrystalUnitFunctions>();

        originScript.TracksOfDonatedEnergy.Remove(line);
        originScript.ConnectedToMe.Remove(destination.gameObject);
        originScript.SystemsThisDonatedEnergyTo.Remove(destination.gameObject);


        destinationScript.RemoveEnergy(origin.gameObject);
        destinationScript.SystemsThisReceivedEnergyFrom.Remove(origin.gameObject);
        destinationScript.ConnectedToMe.Remove(origin.gameObject);

        Destroy(line);
    }

    public static void ConnectThisLineWithParent(GameObject parent, GameObject line)
    {        
        parent.GetComponent<CrystalsUnit>().TracksOfDonatedEnergy.Add(line);
    }

    public static bool CheckIfConnectionIsPossible(Transform origin, Transform destination, float connectionDistance)
    {
        var tracks = origin.GetComponent<CrystalsUnit>().TracksOfDonatedEnergy;
        var tracks2 = destination.GetComponent<CrystalsUnit>().TracksOfDonatedEnergy;
        var dist = Vector3.Distance(origin.position, destination.position);

        Ray myRay = new Ray(Vector3.Lerp(origin.position, destination.position, 0.5f), destination.position);
        RaycastHit hitInfo;

        if (Physics.SphereCast(myRay,1, out hitInfo, 2))
            if (hitInfo.transform.name == "Frame")
                return false;

        for (int i = 0; i < tracks.Count; i++)
        {
            if (tracks[i].GetComponent<ConnectorFunctions>().Destination == destination)
                return false;
        }

        for (int j = 0; j < tracks2.Count; j++)
        {
            if (tracks2[j].GetComponent<ConnectorFunctions>().Destination == origin)
                return false;
        }
            return true;
    }
}
