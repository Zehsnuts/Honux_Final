using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {

    #region Singleton

    private static LineManager _INSTANCE;
    public static LineManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
                _INSTANCE = GameObject.FindObjectOfType<LineManager>();

            return _INSTANCE;
        }
    }

    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;            
        }
        else
        {
            if (this != _INSTANCE)
                Destroy(this.gameObject);
        }
    }
    #endregion


    public LineRenderer MouseLineDrawer;

    public Camera ActualCamera;

    void Start()
    {
        ActualCamera = GameObject.Find("ActualCamera").GetComponent<Camera>();
        MouseLineDrawer = GameObject.Find("LineDrawerForMouse").GetComponent<LineRenderer>();
    }

    public void CreateLineDrawer(Transform a, Transform b, string str)
    {
        if (a == null && b == null)
            return;        

        var pf = GameObject.Find("Line");

        var go =  Instantiate(pf, a.position, a.rotation) as GameObject;

        go.transform.parent = a;

        go.name = "Track: " + a.name + " to " + b.name;
        go.GetComponent<LineDrawer>().DrawLineFromTo(a, b, str);
        GlobalFunctions.ConnectThisLineWithParent(a.gameObject, go);
    }

    public void LineDrawerForMouse(Transform a)
    {
        if (a == null)
            return;   

        if (MouseLineDrawer == null)
        {
            MouseLineDrawer = GameObject.Find("LineDrawerForMouse").GetComponent<LineRenderer>();            
        }

        MouseLineDrawer.SetWidth(.15f, .15f);
        MouseLineDrawer.SetPosition(0, a.position);

        MouseLineDrawer.SetPosition(1, ActualCamera.ScreenToWorldPoint(Input.mousePosition));
    }

    public void CancelLineDrawerForMouse()
    {
        MouseLineDrawer.SetWidth(0, 0);
    }

   


}

