using UnityEngine;
using System.Collections;

public class ConnectionsManager : MonoBehaviour {

	
	void Start () 
    {
        CreateChildren();       
	}


    void CreateChildren()
    {
        if (!FindObjectOfType<ConnectorFunctions>())
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/Connection/Connector")) as GameObject;
            go.transform.parent = transform;
            go.name = "Connector";
            go.AddComponent<ConnectorFunctions>();
        }

        if (!FindObjectOfType<ConnectionCreator>())
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.name = "Connector";
            go.AddComponent<ConnectionCreator>();
        }
    }
}
