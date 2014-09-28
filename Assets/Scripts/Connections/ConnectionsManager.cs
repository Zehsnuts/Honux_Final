using UnityEngine;
using System.Collections;

public class ConnectionsManager : MonoBehaviour {

    #region Singleton

    private static ConnectionsManager _INSTANCE;
    public static ConnectionsManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
                _INSTANCE = GameObject.FindObjectOfType<ConnectionsManager>();

            return _INSTANCE;
        }
    }
    #endregion

	
	void Start () 
    {
        CreateChildren();       
	}

    void CreateChildren()
    {
        /*if (!FindObjectOfType<ConnectorFunctions>())
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/Connection/Connector")) as GameObject;
            go.transform.parent = transform;
            go.name = "Connector";
            go.AddComponent<ConnectorFunctions>();
        }
         * */

        /*
        if (!FindObjectOfType<ConnectionCreator>())
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.name = "Connector";
            go.AddComponent<ConnectionCreator>();
        }
         * */
    }

}
