using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour {

    public string LineType;

    private Transform _lineFrame;
    private Transform _lineTip;

    private float _dist;

    private bool _isDrawningLine = false;

    public Transform Origin;
    public Transform Destination;

    private Material _lineOnMat;
    private Material _lineOffMat;

    private SECTR_AudioSource _audioSource;

    void Update()
    {
        if (!_isDrawningLine)
            return;

        if (_dist > 1)
        {
            _dist = Vector3.Distance(_lineTip.position, Destination.position);
            transform.LookAt(Destination);
            transform.localScale += new Vector3(0, 0, 0.5f);
        }
        else
            _isDrawningLine = false;
    }

    public void DrawLineFromTo(Transform a, Transform b , string str)
    {       
        Origin = a;
        Destination = b;

        LineType = str;
        if (LineType == "Fixed")
            _lineOnMat = Resources.Load("Materials/Line") as Material; 

        else if (LineType == "Temporary")
            _lineOnMat = Resources.Load("Materials/LineTemp") as Material;

        _lineOffMat = Resources.Load("Materials/Black") as Material;

        transform.LookAt(Destination);

        _lineFrame = transform.FindChild("Frame");
        _lineTip = _lineFrame.FindChild("Tip");

        _dist = Vector3.Distance(_lineTip.position, Destination.position);

        _lineFrame.renderer.material = _lineOnMat;
        _isDrawningLine = true;        
    }


    public void TurnTrackOn()
    {
        if (_audioSource == null)
            _audioSource = transform.FindChild("Frame").FindChild("AudioSource").GetComponent<SECTR_AudioSource>();

        _audioSource.Play();

        _lineFrame.GetComponent<MeshRenderer>().enabled = true;
    }

    public void TurnTrackOff()
    {
        if (_audioSource == null)
            _audioSource = transform.FindChild("Frame").FindChild("AudioSource").GetComponent<SECTR_AudioSource>();

        _audioSource.gameObject.active = true;

        _audioSource.Play();

        _lineFrame.GetComponent<MeshRenderer>().enabled = false;
    }

    public void BreakLine()
    {
        if (LineType == "Fixed")
            return;

        ResourcesManager.INSTANCE.AddTrack();

        Debug.Log("This line: "+ gameObject + "\n has " +transform.parent+ "\n and " + Destination );
        GlobalFunctions.BreakThisConnection(gameObject, transform.parent, Destination);

        Destroy(gameObject);
    }
}
