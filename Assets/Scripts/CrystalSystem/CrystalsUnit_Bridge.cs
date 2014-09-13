using UnityEngine;
using System.Collections;

public class CrystalsUnit_Bridge: CrystalUnitFunctions
{
    public GameObject Twin;
    private AudioSource _audioSource;
    private AudioClip _audioClip;

    public override void Start()
    {
        base.Start();

        _audioSource = transform.GetComponent<AudioSource>();

        _audioClip = Resources.Load("Sounds/BridgeFeedback") as AudioClip;

        if (Twin != null)            
            Twin.GetComponent<CrystalsUnit_Bridge>().Twin = gameObject;
    }

    public override void TurnMeOn()
    {
        CrystalsControl.INSTANCE.TurnThisSystemOn(transform);
        isThisSystemOn = true;

        if (Twin != null)
            ConnectedToMe.Add(Twin);

        TransferEnergyToConnections();
        Twin.GetComponent<CrystalsUnit_Bridge>().PlayMySound();
    }

    public void PlayMySound()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
