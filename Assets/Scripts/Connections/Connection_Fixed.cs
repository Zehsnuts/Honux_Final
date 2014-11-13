using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection_Fixed : ConnectorFunctions{

    public ConnectionEnum.ConnectionType Connection;

    public Transform _originPoint;
    public Transform _destinationPoint;

    public Transform _framePosition;
    public GameObject _frame;

    public CrystalConnection cc;

    public GameObject _frameLight;
    public GameObject _frameLight2;
    public GameObject _frameLight3;

    public List<GameObject> _frameLighList;

    public Transform _audioSourceOff;
    public Transform _audioSourceOn;

    public SECTR_PointSource _audioPointSourceOff;
    public SECTR_PointSource _audioPointSourceOn;
    private GameObject _fixedPiece1;
    private GameObject _fixedPiece2;

    public void InitializeConnection(CrystalConnection cristalConnection)
    {        
        Origin = transform.parent;
        cc = cristalConnection;
        Destination = cc.Destination;
        Connection = cc.Connection;

        _originPoint = transform.FindChild("OriginPoint");
        _originPoint.position = Origin.position;

        _destinationPoint = transform.FindChild("DestinationPoint");
        _destinationPoint.position = Destination.position;

        _framePosition = transform.FindChild("Frame");
        
        _frame = Instantiate(Resources.Load("Prefabs/Connection/FrameFixo"), _framePosition.transform.position, _framePosition.transform.rotation) as GameObject;
        _frame.transform.parent = _framePosition;

        _frame.name = "Frame";
        _fixedPiece1 = _frame.transform.FindChild("TrilhoFixo_1").gameObject;
        _fixedPiece2 = _frame.transform.FindChild("TrilhoFixo_2").gameObject;

        _frame.transform.LookAt(Destination.position);
        
        if (Vector3.Distance(Origin.position, Destination.position) > 10)
        {
            GameObject go = GameObject.Find("MidPiece");

            _framePosition.position = Vector3.Lerp(_originPoint.position, go.transform.position, 0.5f);
            _framePosition.position = new Vector3(_framePosition.position.x, (_destinationPoint.position.y + _originPoint.position.y) / 2, _framePosition.position.z);

            _fixedPiece1.transform.position = Vector3.Lerp(_originPoint.position, go.transform.position, 0.5f);
            _fixedPiece2.transform.position = Vector3.Lerp(_destinationPoint.position, go.transform.position, 0.5f);
        }
        else
        {
            _framePosition.position = Vector3.Lerp(_originPoint.position, _destinationPoint.position, 0.5f);

            _fixedPiece1.transform.position = Vector3.Lerp(_originPoint.position, _destinationPoint.position, 0.35f);
            _fixedPiece2.transform.position = Vector3.Lerp(_destinationPoint.position, _originPoint.position, 0.35f);
        }

        _frame.transform.localEulerAngles = new Vector3(_frame.transform.localEulerAngles.x, _frame.transform.localEulerAngles.y, 90);

        _audioSourceOff = _frame.transform.FindChild("AudioSource_off");
        _audioSourceOn = _frame.transform.FindChild("AudioSource_on");
        _audioPointSourceOff = _audioSourceOff.GetComponent<SECTR_PointSource>();
        _audioPointSourceOn = _audioSourceOn.GetComponent<SECTR_PointSource>();

        _frameLight = InstantiateNewLight(Origin);
        _frameLight.transform.parent = _framePosition;

        _frameLight2 = InstantiateNewLight(_fixedPiece2.transform.FindChild("FromDestination"));
        _frameLight2.transform.parent = _framePosition;

        _frameLight3 = InstantiateNewLight(_fixedPiece1.transform.FindChild("ToPiece"));
        _frameLight3.transform.parent = _framePosition;

        TurnTrackOff();
    }

    public void TurnTrackOn()
    {
        StartCoroutine(GetLightDestination(_frameLight, Origin.transform, _fixedPiece1.transform.FindChild("FromOrigin")));
        StartCoroutine(GetLightDestination(_frameLight2, _fixedPiece2.transform.FindChild("FromDestination"), _fixedPiece2.transform.FindChild("ToDestination"), 1, Destination));
        StartCoroutine(GetLightDestination(_frameLight3, _fixedPiece1.transform.FindChild("ToPiece"), _fixedPiece2.transform.FindChild("ToPiece"), 0.5f, null, true));

        _fixedPiece1.active = true;
        _fixedPiece2.active = true;

        foreach (Transform item in _fixedPiece1.transform)
        {
            if (item.GetComponent<MeshRenderer>())
                item.GetComponent<MeshRenderer>().enabled = true;
        }

        foreach (Transform item in _fixedPiece2.transform)
        {
            if (item.GetComponent<MeshRenderer>())
                item.GetComponent<MeshRenderer>().enabled = true;
        }

        _audioSourceOff.active = false;
        _audioSourceOn.active = true;
        _audioPointSourceOn.Play();
    }

    public void TurnTrackOff()
    {
        isTrackOn = false;

        _fixedPiece1.active = false;
        _fixedPiece2.active = false;

        if (Application.loadedLevelName != "Stage_Null")
        {
            foreach (Transform item in _fixedPiece1.transform)
            {
                if(item.GetComponent<MeshRenderer>())
                    item.GetComponent<MeshRenderer>().enabled = false;
            }

            foreach (Transform item in _fixedPiece2.transform)
            {
                if (item.GetComponent<MeshRenderer>())
                    item.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        TurnLightOff(_frameLight);
        TurnLightOff(_frameLight2);
        TurnLightOff(_frameLight3);

        _audioSourceOff.active = true;
        _audioPointSourceOff.Play();
        _audioSourceOn.active = false;
    }

    public void BreakLine()
    {  
        return;
    }
}
