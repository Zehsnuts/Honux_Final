using UnityEngine;
using System.Collections;

public class ExtendedConnectionDestroyer : MonoBehaviour {

    public Transform Parent;

    public void BreakLine()
    {
        Parent.GetComponent<ConnectorFunctions>().BreakLine();
    }
}
