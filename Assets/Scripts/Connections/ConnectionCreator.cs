using UnityEngine;
using System.Collections;

public class ConnectionCreator : MonoBehaviour {

    #region Singleton

    private static ConnectionCreator _INSTANCE;

    public static ConnectionCreator INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<ConnectionCreator>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    public void CreateConnection(Transform a, Transform b, string str)
    {
        if (a == null && b == null)
            return;

        var go = Instantiate(Resources.Load("Prefabs/Connection/Connector"), a.position, a.rotation) as GameObject;

        go.transform.parent = a;

        go.name = "Track: " + a.name + " to " + b.name;

        //go.GetComponent<LineDrawer>().DrawLineFromTo(a, b, str);

        GlobalFunctions.ConnectThisLineWithParent(a.gameObject, go);
    }
}
