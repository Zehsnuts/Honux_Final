using UnityEngine;
using System.Collections;

public static class Extensions
{

    #region PositionGetters

    public static float GetPositionX(this Transform t)
    {
        return t.position.x;
    }

    public static float GetPositionY(this Transform t)
    {
        return t.position.y;
    }

    public static float GetPositionZ(this Transform t)
    {
        return t.position.z;
    }
    #endregion

    #region PositionsSetters

    

    public static void SetPositionX(this Transform t, float newX)
    {
        t.position = new Vector3(newX, t.position.y, t.position.z);
    }

    public static void SetPositionY(this Transform t, float newY)
    {
        t.position = new Vector3(t.position.x, newY, t.position.z);
    }

    public static void SetPositionZ(this Transform t, float newZ)
    {
        t.position = new Vector3(t.position.x, t.position.y, newZ);
    }

    public static void EqualPositionAndRotation(this Transform t, GameObject l)
    {
        t.position = l.transform.position;
        t.rotation= l.transform.rotation;
    }

    public static void EqualPosition(this Transform t, GameObject l)
    {
        t.position = l.transform.position;
    }

    public static void EqualRotation(this Transform t, GameObject l)
    {
        t.rotation = l.transform.rotation;
    }

    #endregion
}
