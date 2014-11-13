using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalsUnit : MonoBehaviour 
{

    public enum SystemType
    { 
        Generator, Replicator, Bridge, Pin, Unstable
    }

    public SystemType systemType;

    public bool isThisSystemOn;
    public bool isSystemSuposedToTurnOn = true;

    public GameObject PrimarySourceOfEnergy;    
    [HideInInspector]
    public bool hasThisSystemTurnedOnAtLastFrame = false;
    [HideInInspector]
    public bool isAffectedByForceField = false;
    [HideInInspector]
    public GameObject lastPrimarySourceOfEnergy;


    public List<GameObject> ConnectedToMe = new List<GameObject>();//Lista todos os elementos que tem uma trilha com este. Mesmo que não forneça ou receba energia deste

    public List<GameObject> SystemsThisReceivedEnergyFrom = new List<GameObject>(); //Lista de elementos que doaram/repassaram energia para este. Serve para não retornar energia na mesma trilha.
    public List<GameObject> SystemsThisDonatedEnergyTo = new List<GameObject>(); //Lista de elementos que receberam energia deste.

    public List<GameObject> TracksOfDonatedEnergy = new List<GameObject>();//Lista de trilhas de energia.
    public bool hasLaidTracks = false;

    public int energyNeededToWork = 1;
    public int energyInsideMe;

    public Transform _unitAudioObjectOn;
    public Transform _unitAudioObjectOff;

    public SECTR_AudioSource _unitAudioSourceOn;
    public SECTR_AudioSource _unitAudioSourceOff;

    public Animator unitAnimator;
}
