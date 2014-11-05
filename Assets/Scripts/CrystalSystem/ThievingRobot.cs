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
        Thief,
        None
    }
    public States state = States.Patrol;
    public States lastKnownState = States.Patrol;
    #endregion

    public SECTR_PointSource audioSourceOn;
    public SECTR_PointSource audioSourceOff;

    private Transform _robot;
    private Transform _destination;
    

    private AudioClip _pickupSound;

    #region Movement Variables
    public float speed = 0.5f;

    private bool _isRobotInTransition = false;

    private Transform _currentMovementTarget;
    private Transform _waypointHolder;
    private Transform _transitionPoint;
    public List<Transform> _waypoints;
    private List<Transform> _waypointsLeft = new List<Transform>();
    private   List<Transform> _waypointsRight = new List<Transform>();

    private Transform _target;

    private int _currentWaypoint = 0;
    #endregion

    public void PauseUnit()
    {
        state = States.Pause;

        audioSourceOn.Stop(true);
        audioSourceOff.Stop(true);        
    }

    public void UnpauseUnit()
    {
        audioSourceOn.Play();

        state = lastKnownState;
    }


    public void StartRobot()
    {
        state = States.Patrol;
    }

    void Start()
    {
        _pickupSound = Resources.Load("Sounds/PickingUpTrackBot") as AudioClip;
        _robot = transform.FindChild("Robot");
        lastKnownState = States.None;

        _waypointHolder = transform.FindChild("Waypoints_Left").transform;

        if (transform.FindChild("Transition Point"))
        _transitionPoint = transform.FindChild("Transition Point").transform;

        foreach (Transform t in _waypointHolder)
        {
            if (t != null)
                _waypointsLeft.Add(t);
        }

        if(_waypointsLeft.Count >0)
            _waypoints = _waypointsLeft;

        _waypointHolder = transform.FindChild("Waypoints_Right").transform;

        foreach (Transform t in _waypointHolder)
        {
            if (t != null)
                _waypointsRight.Add(t);
        }

        if (_waypointsLeft.Count < 1)
            _waypoints = _waypointsRight;

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
        if (!audioSourceOn.IsPlaying && state != States.StandBy && state!=States.Pause)
        {
            audioSourceOn.Play();
            audioSourceOff.Stop(true);
        }
        else if (!audioSourceOff.IsPlaying && state == States.Stun)
        {
            audioSourceOff.Play();
            audioSourceOn.Stop(true);
        }
    }


    private void PatrolState()
    {        
        PlaySound();

        if (_isRobotInTransition && _transitionPoint!=null)
            _currentMovementTarget = _transitionPoint;
        else
            _currentMovementTarget = _waypoints[_currentWaypoint];

        float dist = Vector3.Distance(_currentMovementTarget.position, _robot.transform.position);

        _robot.transform.LookAt(_currentMovementTarget.position);

        if (dist>  2)
        {
            _robot.transform.Translate(Vector3.forward*Time.deltaTime*speed);
        }
        else if (_isRobotInTransition)
        {
            _isRobotInTransition = false;

            if (_waypoints == _waypointsLeft)
                _waypoints = _waypointsRight;
            else
                _waypoints = _waypointsLeft;

            _currentWaypoint = Random.Range(0, _waypoints.Count);

            if (_currentWaypoint >= _waypoints.Count)
                _currentWaypoint = 0; 
        }
        else
        {
            int chosenStageSide = Random.RandomRange(1, 100);

            if (chosenStageSide < 10 && _waypoints == _waypointsRight)
                _isRobotInTransition = true;
            else if (chosenStageSide > 90 && _waypoints == _waypointsLeft)
                _isRobotInTransition = true;

            _currentWaypoint = Random.Range(0, _waypoints.Count + 1);

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
            if (Application.loadedLevelName == "Stage_0_0")
                FindObjectOfType<TutorialObject>().CallTutorialStepComplete();
            state = States.RunAway;
            //ResourcesManager.INSTANCE.RemoveTrack();
            SoundManager.INSTANCE.PlaySingleFile(_pickupSound);
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
        if (Application.loadedLevelName == "Stage_0_0")
            FindObjectOfType<TutorialObject>().CallTutorialStepComplete();

        Debug.Log(state);

        if (state == States.Pause)
            return;

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
