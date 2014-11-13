using UnityEngine;
using System.Collections;

public class Connection_Temporary: ConnectorFunctions
{
    public ConnectionEnum.ConnectionType Connection;

    public Transform _originPoint;
    public Transform _destinationPoint;

    public Transform _framePosition;
    public GameObject _frame;
    public GameObject _centerPiece;

    public CrystalConnection cc;

    public GameObject _frameLight;
    public GameObject _frameLight2;

    public Transform _audioSourceOff;
    public Transform _audioSourceOn;

    public SECTR_PointSource _audioPointSourceOff;
    public SECTR_PointSource _audioPointSourceOn;

    public void InitializeConnection(CrystalConnection cristalConnection, bool shouldTurnOnAfterCreating = false)
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

        _frame = Instantiate(Resources.Load("Prefabs/Connection/FrameTemp"), _framePosition.transform.position, _framePosition.transform.rotation) as GameObject;
        _frame.transform.parent = _framePosition;

        _frame.name = "Frame";
        _frame.transform.LookAt(Destination.position);

        _centerPiece = _frame.transform.FindChild("trilhoUVcorrigida").gameObject;

        if (Vector3.Distance(Origin.position, Destination.position) > 10)
        {
            GameObject go = GameObject.Find("MidPiece");

            _framePosition.position = Vector3.Lerp(_originPoint.position, go.transform.position, 0.5f);
            _framePosition.position = new Vector3(_framePosition.position.x, (_destinationPoint.position.y + _originPoint.position.y) / 2, _framePosition.position.z);
        }
        else
            _framePosition.position = Vector3.Lerp(_originPoint.position, _destinationPoint.position, 0.5f);


        _frame.transform.localEulerAngles = new Vector3(_frame.transform.localEulerAngles.x, _frame.transform.localEulerAngles.y, 0);

        _audioSourceOff = _frame.transform.FindChild("AudioSource_off");
        _audioSourceOn = _frame.transform.FindChild("AudioSource_on");
        _audioPointSourceOff = _audioSourceOff.GetComponent<SECTR_PointSource>();
        _audioPointSourceOn = _audioSourceOn.GetComponent<SECTR_PointSource>();

        _frameLight = InstantiateNewLight(_originPoint);
        _frameLight.transform.parent = _framePosition;

        _frameLight2 = InstantiateNewLight(_frame.transform.FindChild("FromDestination"));
        _frameLight2.transform.parent = _framePosition;

        if (!shouldTurnOnAfterCreating)
            TurnTrackOff();
        else
            TurnTrackOn();
    }

    public void TurnTrackOn()
    {
        StartCoroutine(GetLightDestination(_frameLight, Origin, _frame.transform.FindChild("FromOrigin")));
        StartCoroutine(GetLightDestination(_frameLight2, _frame.transform.FindChild("FromDestination"), _frame.transform.FindChild("ToDestination"), 0.5f, Destination, true));

        _centerPiece.GetComponent<MeshRenderer>().enabled = true;
        _frameLight.GetComponent<ParticleRenderer>().enabled = true;
        _frameLight2.GetComponent<ParticleRenderer>().enabled = true;

        _audioSourceOff.active = false;
        _audioSourceOn.active = true;
        _audioPointSourceOn.Play();
    }

    public void TurnTrackOff()
    {
        isTrackOn = false;  

        if (Application.loadedLevelName != "Stage_Null")
        {
            _centerPiece.GetComponent<MeshRenderer>().enabled = false;
        }

        _frameLight.GetComponent<ParticleRenderer>().enabled = false;
        _frameLight2.GetComponent<ParticleRenderer>().enabled = false;

        _audioSourceOff.active = true;
        _audioPointSourceOff.Play();
        _audioSourceOn.active = false;
    }

    public void BreakLine()
    {
        ResourcesManager.INSTANCE.AddTrack();
        GlobalFunctions.BreakThisConnection(gameObject, transform.parent, Destination);

        Destroy(cc);

        Destroy(gameObject);
    }
}
