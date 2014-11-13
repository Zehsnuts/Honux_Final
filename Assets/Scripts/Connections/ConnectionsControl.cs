using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectionsControl : MonoBehaviour
{
    #region Singleton

    private static ConnectionsControl _INSTANCE;

    public static ConnectionsControl INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<ConnectionsControl>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    private List<CrystalConnection> _crystalConnectionsList = new List<CrystalConnection>();

    public void GrabConnections(CrystalConnection crystalConnections)
    {
        _crystalConnectionsList.Add(crystalConnections);
    }

    void Start()
    {
        CrystalConnection[] ccs = FindObjectsOfType<CrystalConnection>();

        for (int i = 0; i < ccs.Length; i++)
        {
            ccs[i].CreateConnection();
            _crystalConnectionsList.Add(ccs[i]);
        }
    }
}
