using UnityEngine;
using System.Collections;

public class ConnectorFunctions : MonoBehaviour
{
    public ConnectionEnum.ConnectionType Connection;

    public Transform Origin;
    public Transform Destination;

    private Transform _originPoint;
    private Transform _destinationPoint;

    private Transform _framePosition;
    private GameObject _frame;
    private Material _lineOnMat;

    private CrystalConnection cc;

    private GameObject _frameLight;

    private SECTR_AudioSource _audioSource;

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
        _framePosition.position = Vector3.Lerp(_originPoint.position, _destinationPoint.position, 0.5f);

        if (Connection == ConnectionEnum.ConnectionType.Fixed)
            _lineOnMat = Resources.Load("Materials/Line") as Material;
        else
            _lineOnMat = Resources.Load("Materials/LineTemp") as Material;

        _frame = Instantiate(Resources.Load("Prefabs/Connection/Frame"), _framePosition.transform.position, _framePosition.transform.rotation) as GameObject;
        _frame.transform.parent = _framePosition;
        _frame.name = "Frame";
        _frame.transform.LookAt(Destination.position);
        _frame.renderer.material = _lineOnMat;

        _frameLight = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Origin.transform.position, Origin.transform.rotation) as GameObject;
        _frameLight.transform.parent = _framePosition;

        _frameLight.GetComponent<LightningBolt>().target = Destination;
        StartCoroutine(GetLightDestination());

        if(Origin.GetComponent<CrystalsUnit>().isThisSystemOn)
            TurnTrackOn();
        else
            TurnTrackOff();
    }

    IEnumerator GetLightDestination()
    {
        while (_frameLight.GetComponent<LightningBolt>().target == null)
        {
            _frameLight.GetComponent<LightningBolt>().target = Destination;
            yield return 0;
        }        
    }

    public void BreakLine()
    {
        if (Connection == ConnectionEnum.ConnectionType.Fixed || Connection == ConnectionEnum.ConnectionType.ExtendedFixed)
            return;

        GlobalFunctions.BreakThisConnection(gameObject, transform.parent, Destination);
        ResourcesManager.INSTANCE.AddTrack();

        Destroy(cc);

        Destroy(gameObject);
    }

    public void TurnTrackOn()
    {   
        _frame.GetComponent<MeshRenderer>().enabled = true;
        _frameLight.GetComponent<ParticleRenderer>().enabled = true;

        if (_audioSource == null)
            _audioSource = transform.FindChild("Frame").FindChild("Frame").FindChild("AudioSource").GetComponent<SECTR_AudioSource>();

        _audioSource.Play();
    }

    public void TurnTrackOff()
    {
        if (_frame == null)
            return;
        
        if (_frame.GetComponent<MeshRenderer>())
            _frame.GetComponent<MeshRenderer>().enabled = false;
        if (_frameLight.GetComponent<ParticleRenderer>())        
            _frameLight.GetComponent<ParticleRenderer>().enabled = false;

        if (_audioSource == null)
            _audioSource = transform.FindChild("Frame").FindChild("Frame").FindChild("AudioSource").GetComponent<SECTR_AudioSource>();

        _audioSource.gameObject.active = true;

        _audioSource.Play();     
    }
}
