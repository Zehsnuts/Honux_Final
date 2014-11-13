using UnityEngine;
using System.Collections;

public class ConnectorFunctions : MonoBehaviour
{
    public ConnectionEnum.ConnectionType Connection;

    public Transform Origin;
    public Transform Destination;

    public bool isTrackOn = false;

    public void InitializeConnection(CrystalConnection cristalConnection, bool shouldTurnOnAfterCreating = false)
    {
        Connection = cristalConnection.Connection;

        Origin = cristalConnection.transform;
        Destination = cristalConnection.Destination;

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.AddComponent<Connection_Fixed>().InitializeConnection(cristalConnection);
        else
            gameObject.AddComponent<Connection_Temporary>().InitializeConnection(cristalConnection, shouldTurnOnAfterCreating);
    }

    public void BreakLine()
    {
        if (Connection == ConnectionEnum.ConnectionType.Temporary)
            gameObject.GetComponent<Connection_Temporary>().BreakLine();
    }

    public void TurnTrackOn()
    {
        if (isTrackOn)
            return;

        Debug.Log("Something is turning track on.");

        isTrackOn = true;

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOn();
        else
            gameObject.GetComponent<Connection_Temporary>().TurnTrackOn();
    }

    public void TurnTrackOff()
    {
        isTrackOn = false;
        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            gameObject.GetComponent<Connection_Fixed>().TurnTrackOff();
        else
            gameObject.GetComponent<Connection_Temporary>().TurnTrackOff();           
    }

    public GameObject InstantiateNewLight(Transform lightOrigin)
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), lightOrigin.position, lightOrigin.rotation) as GameObject;
        go.GetComponent<LightningBolt>().target = go.transform;
        TurnLightOff(go);
        return go;
    }

    public IEnumerator GetLightDestination(GameObject light, Transform lightOrigin, Transform lightDestination, float animationDelay = 0, Transform destination = null, bool shouldTransferEnergy = false)
    {
        while (light.GetComponent<LightningBolt>().target != lightDestination)
        {
            light.GetComponent<LightningBolt>().target = lightDestination;
            yield return 0;
        }

        TurnLightOff(light);

        Vector3 lightDestinationFinalPosition = lightDestination.position;

        if (destination != null)
            lightDestinationFinalPosition = destination.position;

        lightDestination.position = lightOrigin.position;

        if(!shouldTransferEnergy)
            iTween.MoveTo(lightDestination.gameObject, iTween.Hash("position", lightDestinationFinalPosition, "delay", animationDelay,
                                                                "onstart", "TurnLightOn", "onstarttarget", gameObject, "onstartparams", light));
        else
            iTween.MoveTo(lightDestination.gameObject, iTween.Hash("position", lightDestinationFinalPosition, "delay", animationDelay,
                                                                "onstart", "TurnLightOn", "onstarttarget", gameObject, "onstartparams", light,
                                                                "oncomplete", "TransferEnergy", "oncompletetarget", gameObject));
    }

    public void TransferEnergy()
    {
        Destination.GetComponent<CrystalUnitFunctions>().AddEnergy(Origin.gameObject);
    }

    public void TurnLightOn(GameObject light)
    {
        light.GetComponent<ParticleRenderer>().enabled = true;
    }

    public void TurnLightOff(GameObject light)
    {
        light.GetComponent<ParticleRenderer>().enabled = false;
    }
}
