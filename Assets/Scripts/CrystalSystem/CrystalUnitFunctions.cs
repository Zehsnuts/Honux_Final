using UnityEngine;
using System.Collections;

public class CrystalUnitFunctions : CrystalsUnit
{

    #region Events
    void Events()
    {
        EventManager.GAMEPAUSESTART += PauseUnit;
        EventManager.GAMEPAUSEEXIT += UnpauseUnit;

        EventManager.BLUEPRINTSTART += PauseUnit;
        EventManager.BLUEPRINTEXIT += UnpauseUnit;

        EventManager.AUDIOSCREENSTART += PauseUnit;
        EventManager.AUDIOSCREENEXIT += UnpauseUnit;

        EventManager.STAGEFAIL += PauseUnit;
        EventManager.STAGESUCESS += PauseUnit;
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
    }
    #endregion

    void PauseUnit()
    {
        if(_unitAudioSourceOn!=null)
            _unitAudioSourceOn.Stop(true);

        if (_unitAudioSourceOff != null)
            _unitAudioSourceOff.Stop(true);        
    }

    void UnpauseUnit()
    {
        ChangeSystemStatus();
        if (_unitAudioSourceOn != null)
            _unitAudioSourceOn.Play();

        if (_unitAudioSourceOff != null)
            _unitAudioSourceOff.Play(); 
    }

    public virtual void Start()//Sequencia de inicialização do sistema inteiro. 
    {
        GrabAudioSource();
        this.UnitCheckSystemType(); //Qual tipo de unidade esse GameObject é;
        this.UnitGrabAnimator();
        ChangeSystemStatus(); //Usa o resultado de CheckEnergy para mudar o status do systema para on ou off;
        Events();
    }

    public void GrabAudioSource()
    {
        _unitAudioObjectOn = transform.FindChild("AudioSource_on");
        _unitAudioObjectOff = transform.FindChild("AudioSource_off");

        while(_unitAudioSourceOn==null)
            _unitAudioSourceOn = _unitAudioObjectOn.GetComponent<SECTR_AudioSource>();

        while (_unitAudioSourceOff == null)
            _unitAudioSourceOff = _unitAudioObjectOff.GetComponent<SECTR_AudioSource>();    
    }
            

    //Função que verifica quais unidades estão conectadas neste e faz uma lista (ConnectedToMe)
    public void CheckConnections()
    {        
        //Ele não pode se auto-conter na lista de conexões; Evita erro humano;
        if (ConnectedToMe.Contains(gameObject))
            ConnectedToMe.Remove(gameObject);

        //Para cada elemento que está conectado neste, verifica se este está conectado no elemento. Apenas para preencher a lista de conexões.
        for (int i = 0; i < ConnectedToMe.Count; i++)
        {
            var connectedList = ConnectedToMe[i].GetComponent<CrystalsUnit>().ConnectedToMe;
            if (!connectedList.Contains(gameObject))
                connectedList.Add(gameObject);
        }
    }

    public virtual bool CheckEnergy()
    {
        if (energyInsideMe >= energyNeededToWork && !isAffectedByForceField)
            return true;
        else
            return false;
    }

    public void ChangeSystemStatus()
    {
        if (CheckEnergy())
            TurnMeOn();
        else
            TurnMeOff();
    }
    
    public virtual void TurnMeOn()
    {
        if (isThisSystemOn)        
            return;

        CheckIfMyConnectionsHaveMeOnTracks();

        isSystemSuposedToTurnOn = true;

        isThisSystemOn = true;

        CrystalsControl.INSTANCE.TurnThisSystemOn(transform);
    }

    public void TurnOnAfterAnimation()
    {
        if (!isSystemSuposedToTurnOn)
            return;

        Debug.Log("Turning on after animation.");

        TransferEnergyToConnections();

        if (_unitAudioSourceOn == null || _unitAudioSourceOff == null)
            GrabAudioSource();

        _unitAudioSourceOn.Play();
        _unitAudioSourceOff.Stop(true);
    }

    void CheckIfMyConnectionsHaveMeOnTracks()
    {
        for (int i = 0; i < ConnectedToMe.Count; i++)
        {
            var itemCrystalUnit = ConnectedToMe[i].GetComponent<CrystalsUnit>();

            for (int k = 0; i <itemCrystalUnit.TracksOfDonatedEnergy.Count; i++)
            {
                var trackConnector = itemCrystalUnit.TracksOfDonatedEnergy[k].GetComponent<ConnectorFunctions>();
                if (trackConnector.Destination == transform)
                    this.Connection_CheckIfConnectionsNeedToChangeParent(trackConnector);
            }
        }
    }

    IEnumerator WaitAnimationToTurnOn()
    {
        isThisSystemOn = true;        
        yield return 0;        

        if (_unitAudioSourceOn == null || _unitAudioSourceOff == null)
            GrabAudioSource();

        _unitAudioSourceOn.Play();
        _unitAudioSourceOff.Stop(true);   
    }

    public virtual void TurnMeOff()
    {
        isThisSystemOn = false;

        isSystemSuposedToTurnOn = false;

        RemoveEnergyFromConnections();

        if (AskForEnergyFromConnections())
        {
            ChangeSystemStatus();
            return;
        }

        CrystalsControl.INSTANCE.TurnThisSystemOff(transform);        

        if (_unitAudioSourceOn == null || _unitAudioSourceOff== null)
            GrabAudioSource();

        _unitAudioSourceOff.Play();
        _unitAudioSourceOn.Stop(true);        
    }

    bool AskForEnergyFromConnections()
    {
        if (isAffectedByForceField)
            return false;   

        foreach (var item in ConnectedToMe)
        {
            if (item == lastPrimarySourceOfEnergy)
            {
                lastPrimarySourceOfEnergy = null;
                Debug.Log(gameObject.name + " has last source: " + item.name);
                continue;
            }

            var cu = item.GetComponent<CrystalUnitFunctions>();
            if (cu.SystemsThisReceivedEnergyFrom.Count > 0 && cu.PrimarySourceOfEnergy != gameObject)
            {
                foreach (var system in cu.SystemsThisReceivedEnergyFrom)
                {
                    if (cu.SystemsThisReceivedEnergyFrom.Count == 1 &&
                        cu.SystemsThisReceivedEnergyFrom.Contains(gameObject) ||
                        cu.systemType == SystemType.Pin)
                    {
                        Debug.Log(gameObject.name +" can't receive energy from"+ cu.gameObject);
                        continue;
                    }
                    else if (system == gameObject)
                    continue;
                    else
                    {
                        AddEnergy(cu.gameObject);

                        if (cu.SystemsThisReceivedEnergyFrom.Contains(gameObject))
                            cu.RemoveEnergy(gameObject);
                        if (SystemsThisDonatedEnergyTo.Contains(cu.gameObject))
                            SystemsThisDonatedEnergyTo.Remove(cu.gameObject);

                        return true;
                    }                    
                }
            }
        }

        return false;
    }

    public void AddEnergy(GameObject donor)
    {
        if (donor.GetComponent<CrystalsUnit>().systemType == SystemType.Pin)
            return;

        //Adiciona donor a lista de doadores;
        if (donor != null && !SystemsThisReceivedEnergyFrom.Contains(donor))
        {
            if (PrimarySourceOfEnergy == null)
                PrimarySourceOfEnergy = donor;

            energyInsideMe++;
            SystemsThisReceivedEnergyFrom.Add(donor);
            donor.GetComponent<CrystalsUnit>().SystemsThisDonatedEnergyTo.Add(gameObject);
        }

        ChangeSystemStatus();       
    }

    public void RemoveEnergy(GameObject donor)
    {      
        if (SystemsThisReceivedEnergyFrom.Contains(donor)) 
        {
            energyInsideMe--;
            SystemsThisReceivedEnergyFrom.Remove(donor);

            if (PrimarySourceOfEnergy == donor)
            {
                lastPrimarySourceOfEnergy = donor;
                if (SystemsThisReceivedEnergyFrom.Count > 1)
                    PrimarySourceOfEnergy = SystemsThisReceivedEnergyFrom[0];
                else
                    PrimarySourceOfEnergy = null;
            }
        }        

        if (energyInsideMe < 0)
            energyInsideMe = 0;

        ChangeSystemStatus();
    }

    public void TransferEnergyToConnections()
    {        
        if (!isThisSystemOn || energyInsideMe==0 || systemType == SystemType.Pin)
            return;

        Debug.Log(transform.name + " is transfering energy");

        for (int i = 0; i < TracksOfDonatedEnergy.Count; i++)
        {
            Debug.Log("Turning on: " + TracksOfDonatedEnergy[i].name);

            TracksOfDonatedEnergy[i].GetComponent<ConnectorFunctions>().TurnTrackOn();
        }
    }

    IEnumerator WaitBeforeTransferingEnergy()
    {
        yield return new WaitForSeconds(0.1f);        

        foreach (GameObject go in ConnectedToMe)
        {
            //ConnectedToMe pode conter alguem que está doando energia para este; Exclui essa possibilidade da lista que receberá energia deste.
            if (SystemsThisReceivedEnergyFrom.Contains(go))
                continue;

            if (!SystemsThisDonatedEnergyTo.Contains(go))
            {
                SystemsThisDonatedEnergyTo.Add(go);
                go.GetComponent<CrystalUnitFunctions>().AddEnergy(gameObject);
            }

            if (!go.GetComponent<CrystalsUnit>().hasLaidTracks)
                go.GetComponent<CrystalUnitFunctions>().LineParentAssign();
        }

        if (!hasLaidTracks)
            LineParentAssign();
    }

    public void LineParentAssign()
    {        
        foreach (Transform t in transform)
            {
                if (t.GetComponent<LineRenderer>())
                    TracksOfDonatedEnergy.Add(t.gameObject);
            }
        hasLaidTracks = true;

        //ChangeSystemStatus();
    }

    public void RemoveEnergyFromConnections()
    {
        if(SystemsThisDonatedEnergyTo.Count<1)
            return;

        foreach (GameObject go in SystemsThisDonatedEnergyTo)
        {
            go.GetComponent<CrystalUnitFunctions>().RemoveEnergy(gameObject);
        }

        foreach (var item in ConnectedToMe)
        {
            var cu = item.GetComponent<CrystalUnitFunctions>();
            if (cu.SystemsThisReceivedEnergyFrom.Contains(gameObject))
                cu.RemoveEnergy(gameObject);
        }
        
        SystemsThisDonatedEnergyTo.Clear();
    }

    public void ConnectSingleUnit(GameObject unit)
    {
        if (ConnectedToMe.Contains(unit))
            return;

       ConnectedToMe.Add(unit);
    }

    void CheckIfCanReceiveEnergyFromUnit(GameObject unit)
    {
        if (unit.GetComponent<CrystalsUnit>().energyInsideMe > 0 && unit.GetComponent<CrystalsUnit>().SystemsThisReceivedEnergyFrom.Count > 0 && !unit.GetComponent<CrystalsUnit>().SystemsThisReceivedEnergyFrom.Contains(gameObject))
            AddEnergy(unit);
    }    
}
