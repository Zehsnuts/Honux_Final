using UnityEngine;
using System.Collections;

public class Stage_HUB : MonoBehaviour
{
    #region Singletone

    private static Stage_HUB _INSTANCE;

    public static Stage_HUB INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<Stage_HUB>();
            }
            return _INSTANCE;
        }
    }
    #endregion

   
}
