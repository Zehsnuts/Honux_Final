using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThievingRobot : MonoBehaviour {
    
    #region Events

    void OnEnable()
    {
        EventManager.GAMEPAUSESTART += PauseUnit;
        EventManager.GAMEPAUSEEXIT += UnpauseUnit;

        EventManager.BLUEPRINTSTART += PauseUnit;
        EventManager.BLUEPRINTEXIT += UnpauseUnit;

        EventManager.AUDIOSCREENSTART += PauseUnit;
        EventManager.AUDIOSCREENEXIT += UnpauseUnit;

        EventManager.STAGEFAIL += PauseUnit;
        EventManager.STAGESUCESS += PauseUnit;

        EventManager.ROBOTURNON += StartRobot;
        //EventManager.ROBOTURNOFF += PauseUnit;
    }

    void OnDisable()
    {
        EventManager.GAMEPAUSESTART -= PauseUnit;
        EventManager.GAMEPAUSEEXIT -= UnpauseUnit;

        EventManager.BLUEPRINTSTART -= PauseUnit;
        EventManager.BLUEPRINTEXIT -= UnpauseUnit;

        EventManager.AUDIOSCREENSTART -= PauseUnit;
        EventManager.AUDIOSCREENEXIT -= UnpauseUnit;

        EventManager.STAGEFAIL -= PauseUnit;
        EventManager.STAGESUCESS -= PauseUnit;

        EventManager.ROBOTURNON -= StartRobot;
        //EventManager.ROBOTURNOFF -= PauseUnit;
    }
    #endregion

    #region Singleton

    private static ThievingRobot _INSTANCE;
    public static ThievingRobot INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<ThievingRobot>();
            }
            return _INSTANCE;
        }
    }

    #endregion

    #region FSM Variables
    public enum States
    {
        StandBy,
        Patrol,
        Pause,
        RunAway,
        Stun,
        Thief
    }
    public States state = States.Patrol;
    public States lastKnownState = States.Patrol;
    #endregion
    
    private Transform _robot;
    private Transform _destination;
    private SECTR_AudioSource _audioSourceOn;
    private SECTR_AudioSource _audioSourceOff;

    #region Movement Variables
    public float speed = 0.5f;

    private Transform _waypointHolder;
    public List<Transform> _waypoints;
    private Transform _target;

    private int _currentWaypoint = 0;
    #endregion

    public void PauseUnit()
    {
        state = States.Pause;

        _audioSourceOn.Stop(true);
        _audioSourceOff.Stop(true);        
    }

    public void UnpauseUnit()
    {
        _audioSourceOn.Play();

        state = lastKnownState;
    }


    public void StartRobot()
    {
        state = States.Patrol;
    }


    void Start()
    {       
        _robot = transform.FindChild("Robot");
        lastKnownState = States.Patrol;

        _waypointHolder = transform.FindChild("Waypoints").transform;

        foreach (Transform t in _waypointHolder)
        {
            if(t!=null)
                _waypoints.Add(t);
        }

        _audioSourceOn = _robot.FindChild("AudioSource_on").GetComponent<SECTR_AudioSource>();
        _audioSourceOff = _robot.FindChild("AudioSource_off").GetComponent<SECTR_AudioSource>();


        _robot.transform.position = _waypoints[Random.Range(0, _waypoints.Count)].position;

        PauseUnit();
    }

    void Update()
    {        
        switch (state)
        {
            case States.Patrol:
                PatrolState();
                break;

            case States.Stun:
                StunState();
                break;

            case States.Thief:
                ThiefState();
                break;

            case States.RunAway:
                PatrolState();
                break;

            case States.Pause:
                PauseState();
                break;
        }       
    }

    void PlaySound()
    {
        if (!_audioSourceOn.IsPlaying && state != States.StandBy && state!=States.Pause)
        {
            _audioSourceOn.Play();
            _audioSourceOff.Stop(true);
        }
        else if (!_audioSourceOff.IsPlaying && state == States.Stun)
        {
            _audioSourceOff.Play();
            _audioSourceOn.Stop(true);
        }
    }


    private void PatrolState()
    {        
        PlaySound();        

        float dist = Vector3.Distance(_waypoints[_currentWaypoint].position, _robot.transform.position);

        _robot.transform.LookAt(_waypoints[_currentWaypoint].position);

        if (dist>  2)
        {
            _robot.transform.Translate(Vector3.forward*Time.deltaTime*speed);
        }
        else
        {
           _currentWaypoint = Random.Range(0, _waypoints.Count+1);
            if (_currentWaypoint >= _waypoints.Count)
                _currentWaypoint = 0;
        }

        if (state != States.RunAway)
        {
            RaycastHit hit;
            if (Physics.SphereCast(_robot.position, 3, _robot.transform.up, out hit))
            {
                if (hit.transform.name == "Frame" && hit.transform.parent.parent.GetComponent<ConnectorFunctions>().Connection == ConnectionEnum.ConnectionType.Temporary)
                {
                    _target = hit.transform;
                    state = States.Thief;
                }
                return;
            }
        } 

        lastKnownState = state;
    }

    void ThiefState()
    {
        if (_target == null)
        {
            state = States.Patrol;
            return;
        }

        PlaySound();

        float dist = Vector3.Distance(_target.position, _robot.transform.position);

        _robot.transform.LookAt(_target.position);

        if (dist > 0.1f)
        {
            _robot.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            _target.parent.parent.GetComponent<ConnectorFunctions>().BreakLine();
            state = States.RunAway;
            ResourcesManager.INSTANCE.RemoveTrack();
            _currentWaypoint = Random.Range(0, _waypoints.Count + 1);
            if (_currentWaypoint >= _waypoints.Count)
                _currentWaypoint = 0;      
        }
        lastKnownState = state;
    }

    void PauseState()
    {
        if(state != States.Pause)
            lastKnownState = state;
    }

    #region Stun
    public void StunRobot()
    {
        Debug.Log(state);

        if (state == States.RunAway)
        {            
            ResourcesManager.INSTANCE.AddTrack();
        }


        state = States.Stun;

        StartCoroutine(Stun());
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(60);
        state = States.Patrol;
    }

    void StunState()
    {
        PlaySound();
    }
    #endregion   
    
}
