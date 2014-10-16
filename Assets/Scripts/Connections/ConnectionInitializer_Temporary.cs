using UnityEngine;
using System.Collections;

public class ConnectionInitializer_Temporary : ConnectorFunctions {

    /*
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
        if (Vector3.Distance(Origin.position, Destination.position) > 10)
        {
            GameObject go = GameObject.Find("MidPiece");

            _framePosition.position = Vector3.Lerp(_originPoint.position, go.transform.position, 0.5f);
            _framePosition.position = new Vector3(_framePosition.position.x, (_destinationPoint.position.y + _originPoint.position.y) / 2, _framePosition.position.z);
        }
        else
            _framePosition.position = Vector3.Lerp(_originPoint.position, _destinationPoint.position, 0.5f);


        _frame = Instantiate(Resources.Load("Prefabs/Connection/Frame"), _framePosition.transform.position, _framePosition.transform.rotation) as GameObject;
        _frame.transform.parent = _framePosition;
        _frame.name = "Frame";
        _frame.transform.LookAt(Destination.position);

        _audioSourceOff.name = "AudioSource_off";
        _audioPointSourceOff = _audioSourceOff.GetComponent<SECTR_PointSource>();
        _audioSourceOn.name = "AudioSource_on";
        _audioPointSourceOn = _audioSourceOn.GetComponent<SECTR_PointSource>();

        _frame.renderer.material = _lineOnMat;

        _frameLight = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Origin.transform.position, Origin.transform.rotation) as GameObject;
        _frameLight.transform.parent = _framePosition;

        _frameLight.GetComponent<LightningBolt>().target = _frame.transform.FindChild("OriginEnd");
        StartCoroutine(GetLightDestination(_frame.transform.FindChild("OriginEnd")));

        _frameLight2 = Instantiate(Resources.Load("Prefabs/Connection/ConnectionLight"), Destination.transform.position, Destination.transform.rotation) as GameObject;
        _frameLight2.transform.parent = _framePosition;

        _frameLight2.GetComponent<LightningBolt>().target = _frame.transform.FindChild("DestinationBegin");
        StartCoroutine(GetLightDestination(_frame.transform.FindChild("DestinationBegin")));

        if (Origin.GetComponent<CrystalsUnit>().isThisSystemOn)
            TurnTrackOn();
        else
            TurnTrackOff();
    }

    IEnumerator GetLightDestination(Transform dest)
    {
        while (_frameLight.GetComponent<LightningBolt>().target == null)
        {
            _frameLight.GetComponent<LightningBolt>().target = dest;
            yield return 0;
        }
    }
     * */
}
