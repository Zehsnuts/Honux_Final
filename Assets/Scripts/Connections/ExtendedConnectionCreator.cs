using UnityEngine;
using System.Collections;

public class ExtendedConnectionCreator : MonoBehaviour {

    public ConnectionEnum.ConnectionType Connection;

    public Transform Destination;

    CrystalConnectionExtended cc;

    GameObject Go;
    GameObject Go2;
    GameObject Go3;
    GameObject Frame1;
    GameObject Frame2;
    GameObject Frame3;

    GameObject frameLight1;
    GameObject frameLight2;
    GameObject frameLight3;
    GameObject frameLight4;
    GameObject frameLight5;

    void Start()
    {
        if (Destination != null)
        {
            SetConnectionAttributes();
            CreateConnection();
            Destroy(this);
        }
        else
            Debug.LogError("Extended crystal connection doesn't have a destination. " + gameObject.name);
    }

    void SetConnectionAttributes()
    {       
        cc = gameObject.AddComponent<CrystalConnectionExtended>();       

        if (Connection == ConnectionEnum.ConnectionType.ExtendedFixed)
            cc.Connection = ConnectionEnum.ConnectionType.ExtendedFixed;
        else if (Connection == ConnectionEnum.ConnectionType.ExtendedTemporary)
            cc.Connection = ConnectionEnum.ConnectionType.ExtendedTemporary;

        cc.Destination = Destination;
    }

    void CreateConnection()
    {
        GameObject extendedAux = new GameObject();
        extendedAux.transform.position = new Vector3(30, 0, 6);

        Go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        Go.transform.parent = transform;
        Go.name = "Track: " + transform.name + " to " + Destination.name + "_Line_2";
        Go.transform.position = Vector3.Lerp(transform.position, Destination.position, 0.5f);


        Frame1 = Instantiate(Resources.Load("Prefabs/Connection/Frame"), Go.transform.position, Go.transform.rotation) as GameObject;
        Frame1.transform.parent = Go.transform;
        Frame1.name = "ExtendedFrame";
        Frame1.AddComponent<ExtendedConnectionDestroyer>().Parent = Go.transform;
        Frame1.transform.LookAt(Destination.position);
        Go.GetComponent<ConnectorFunctions>().Destination = Destination;
        Go.GetComponent<ConnectorFunctions>().Connection = ConnectionEnum.ConnectionType.Temporary;

        if (cc.Connection == ConnectionEnum.ConnectionType.Fixed || cc.Connection == ConnectionEnum.ConnectionType.ExtendedFixed)
            Frame1.renderer.material = Resources.Load("Materials/Line") as Material;
        else
            Frame1.renderer.material = Resources.Load("Materials/LineTemp") as Material;
        

        Go2 = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        Go2.name = "Track: " + transform.name + " to " + Destination.name + "_Line_1";
        extendedAux.transform.SetPositionY(transform.position.y);
        Go2.transform.position = Vector3.Lerp(transform.position,  extendedAux.transform.position, 0.5f);
        Go2.transform.parent = Go.transform;
        Frame2 = Instantiate(Resources.Load("Prefabs/Connection/Frame"), Go2.transform.position, Go2.transform.rotation) as GameObject;
        Frame2.transform.parent = Go2.transform;
        Frame2.name = "ExtendedFrame";
        Frame2.AddComponent<ExtendedConnectionDestroyer>().Parent = Go.transform;
        Frame2.transform.LookAt(Go.transform.position);
        if (cc.Connection == ConnectionEnum.ConnectionType.Fixed || cc.Connection == ConnectionEnum.ConnectionType.ExtendedFixed)
            Frame2.renderer.material = Resources.Load("Materials/Line") as Material;
        else
            Frame2.renderer.material = Resources.Load("Materials/LineTemp") as Material;


        Go3 = Instantiate(Resources.Load("Prefabs/Connection/Connector"), transform.position, transform.rotation) as GameObject;
        Go3.name = "Track: " + transform.name + " to " + Destination.name + "_Line_3";
        extendedAux.transform.SetPositionY(Destination.transform.position.y);
        Go3.transform.position = Vector3.Lerp(Destination.transform.position, extendedAux.transform.position, 0.5f);
        Go3.transform.parent = Go.transform;

        Frame3 = Instantiate(Resources.Load("Prefabs/Connection/Frame"), Go3.transform.position, Go3.transform.rotation) as GameObject;
        Frame3.transform.parent = Go3.transform;
        Frame3.name = "ExtendedFrame";
        Frame3.AddComponent<ExtendedConnectionDestroyer>().Parent = Go.transform;
        Frame3.transform.LookAt(Go.transform.position);        
        if (cc.Connection == ConnectionEnum.ConnectionType.Fixed || cc.Connection == ConnectionEnum.ConnectionType.ExtendedFixed)
            Frame3.renderer.material = Resources.Load("Materials/Line") as Material;
        else
            Frame3.renderer.material = Resources.Load("Materials/LineTemp") as Material;

        cc.ExtendedConnectionComponents.Add(Go);
        cc.ExtendedConnectionComponents.Add(Go2);
        cc.ExtendedConnectionComponents.Add(Go3);

        GlobalFunctions.ConnectThisLineWithParent(transform.gameObject, Go);

        frameLight1 = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Frame1.transform.FindChild("OriginEnd").transform.position, Frame1.transform.rotation) as GameObject;
        frameLight1.transform.parent = Go.transform;
        frameLight1.GetComponent<LightningBolt>().target = Frame2.transform.FindChild("DestinationBegin");
        StartCoroutine(GetLightDestination(frameLight1.GetComponent<LightningBolt>(), Frame2.transform.FindChild("DestinationBegin")));

        frameLight2 = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Frame1.transform.FindChild("DestinationBegin").transform.position, Frame1.transform.rotation) as GameObject;
        frameLight2.transform.parent = Go.transform;
        frameLight2.GetComponent<LightningBolt>().target = Frame3.transform.FindChild("DestinationBegin");
        StartCoroutine(GetLightDestination(frameLight2.GetComponent<LightningBolt>(), Frame3.transform.FindChild("DestinationBegin")));

        frameLight3 = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Frame2.transform.FindChild("OriginEnd").transform.position, Frame2.transform.rotation) as GameObject;
        frameLight3.transform.parent = Go2.transform;
        frameLight3.GetComponent<LightningBolt>().target = transform;
        StartCoroutine(GetLightDestination(frameLight3.GetComponent<LightningBolt>(), transform));

        frameLight4 = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Frame3.transform.FindChild("OriginEnd").transform.position, Frame3.transform.rotation) as GameObject;
        frameLight4.transform.parent = Go3.transform;
        frameLight4.GetComponent<LightningBolt>().target = Destination;
        StartCoroutine(GetLightDestination(frameLight4.GetComponent<LightningBolt>(), Destination));

        if (transform.GetComponent<CrystalsUnit>().isThisSystemOn)
            TurnTrackOn();
        else
            TurnTrackOff();
    }

    IEnumerator GetLightDestination(LightningBolt dis, Transform desti)
    {
        while (dis.target == null)
        {
            dis.target = desti;
            yield return 0;
        }

    }

    public void TurnTrackOn()
    {
        Frame1.GetComponent<MeshRenderer>().enabled = true;
        Frame2.GetComponent<MeshRenderer>().enabled = true;
        Frame3.GetComponent<MeshRenderer>().enabled = true;
        frameLight1.GetComponent<ParticleRenderer>().enabled = true;
        frameLight2.GetComponent<ParticleRenderer>().enabled = true;
        frameLight3.GetComponent<ParticleRenderer>().enabled = true;
        frameLight4.GetComponent<ParticleRenderer>().enabled = true;
    }

    public void TurnTrackOff()
    {
        Frame1.GetComponent<MeshRenderer>().enabled = false;
        Frame2.GetComponent<MeshRenderer>().enabled = false;
        Frame3.GetComponent<MeshRenderer>().enabled = false;
        frameLight1.GetComponent<ParticleRenderer>().enabled = false;
        frameLight2.GetComponent<ParticleRenderer>().enabled = false;
        frameLight3.GetComponent<ParticleRenderer>().enabled = false;
        frameLight4.GetComponent<ParticleRenderer>().enabled = false;
    }

    public void CreateConnectionAtRunTime(Transform destination, ConnectionEnum.ConnectionType type)
    {
        this.Destination = destination;
        Connection = type;
    }
}
