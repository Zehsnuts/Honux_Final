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
        CheckSystemType(); //Qual tipo de unidade esse GameObject é;
        //CheckConnections(); //Quais conexões ele tem com os outros elementos da fase;
        //CheckEnergy(); //Verifica se ele tem energia para estar ligado ou não; Retorna bool; Não precisa estar aqui pois ChangeSystemStatus usa ele; Coloquei para saber ordem inicial.
        //TurnMeOff();
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
            
    public void CheckSystemType()
    {
        switch (transform.name)
        { 
            case "Generator":
                gameObject.AddComponent<CrystalUnitAnimator>();
                systemType = SystemType.Generator;
                TurnMeOff();
                break;
            case "ReplicatorNormal":
                gameObject.AddComponent<CrystalUnitAnimator>();
                systemType = SystemType.Replicator;
                break;
            case "Bridge":
                gameObject.AddComponent<CrystalUnitAnimator>();
                systemType = SystemType.Bridge;
                break;
            case "UnstableCrystal":
                gameObject.AddComponent<CrystalUnitAnimator>();
                systemType = SystemType.Unstable;
                break;
        }
    }

    //Função que verifica quais unidades estão conectadas neste e faz uma lista (ConnectedToMe)
    public void CheckConnections()
    {
        //GlobalFunctions.CheckIfMyConnectionsHaveMe(gameObject);

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

    public void CheckIfConnected(GameObject go) //Recebe elementou que chamou a função neste.
    {
        //Se minha lista não contem go(elemento que está ligado em mim), adiciona go na minha lista;
        if (!ConnectedToMe.Contains(go))
        {
            ConnectedToMe.Add(go);               
        }
        //Caso contenha, veja se os outros elementos conectados em mim tambem estão conecatos em go; Ativa a mesma função em todos os elementos;
        else
        {           
            for (int i = 0; i < ConnectedToMe.Count; i++)
            {
                ConnectedToMe[i].GetComponent<CrystalUnitFunctions>().CheckIfConnected(go);
            }
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
        

        if (PrimarySourceOfEnergy == null && systemType == SystemType.Generator)
        {
            isThisSystemOn = false;
            isSystemSuposedToTurnOn = true;

            isThisSystemOn = true;

            CrystalsControl.INSTANCE.TurnThisSystemOn(transform);

            CheckIfMyConnectionsHaveMeOnTracks();
            return;
        }

        isSystemSuposedToTurnOn = true;

        isThisSystemOn = true;

        CrystalsControl.INSTANCE.TurnThisSystemOn(transform);

        CheckIfMyConnectionsHaveMeOnTracks();

        //StartCoroutine(WaitAnimationToTurnOn());  
    }

    public void TurnOnAfterAnimation()
    {
        if (!isSystemSuposedToTurnOn)
            return;

        TransferEnergyToConnections();

        if (_unitAudioSourceOn == null || _unitAudioSourceOff == null)
            GrabAudioSource();

        _unitAudioSourceOn.Play();
        _unitAudioSourceOff.Stop(true);
    }

    void CheckIfMyConnectionsHaveMeOnTracks()
    {
        foreach (GameObject item in ConnectedToMe)
        {
            foreach (var track in item.GetComponent<CrystalsUnit>().TracksOfDonatedEnergy)
            {
                if (track.GetComponent<Connection_Fixed>())
                    if (track.GetComponent<Connection_Fixed>().Destination == transform)
                        track.GetComponent<Connection_Fixed>().TurnTrackOn();

                else if (track.GetComponent<Connection_Temporary>())
                        if (track.GetComponent<Connection_Temporary>().Destination == transform)
                            track.GetComponent<Connection_Temporary>().TurnTrackOn();
            }
        }
    }

    IEnumerator WaitAnimationToTurnOn()
    {
        isThisSystemOn = true;

        if (gameObject.GetComponent<CrystalUnitAnimator>())
            yield return new WaitForSeconds(gameObject.GetComponent<CrystalUnitAnimator>().TurnOnAnimation());
        else
            yield return new WaitForSeconds(1);      

        TransferEnergyToConnections();

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

       //SystemsThisReceivedEnergyFrom.Clear();

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
            //Debug.Log(gameObject.name + " trying to get energy from: " + item.name);

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

                        cu.SystemsThisDonatedEnergyTo.Add(gameObject);

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
            TransferEnergyToConnections();
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

        StartCoroutine(WaitBeforeTransferingEnergy());
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
            else
                go.GetComponent<CrystalUnitFunctions>().TurnLinesOn();
        }

        if (!hasLaidTracks)
            LineParentAssign();
        else
            TurnLinesOn();
    }

    public void LineParentAssign()
    {        
        foreach (Transform t in transform)
            {
                if (t.GetComponent<LineRenderer>())
                    TracksOfDonatedEnergy.Add(t.gameObject);
            }
        hasLaidTracks = true;

        ChangeSystemStatus();
    }

    public void TurnLinesOn()
    {
        foreach (GameObject go in TracksOfDonatedEnergy)
        {
            //go.GetComponent<LineDrawer>().TurnTrackOn();
        }
    }

    public void RemoveEnergyFromConnections()
    {
        //Existe a possibilidade que este apenas receba energia e não passe pra frente; 
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

       if (energyInsideMe > 0)
       {
           unit.GetComponent<CrystalUnitFunctions>().AddEnergy(gameObject);
           SystemsThisDonatedEnergyTo.Add(unit);
       }
       else CheckIfCanReceiveEnergyFromUnit(unit);

       CheckConnections();
       
       ChangeSystemStatus();

       unit.GetComponent<CrystalUnitFunctions>().ChangeSystemStatus();
    }

    void CheckIfCanReceiveEnergyFromUnit(GameObject unit)
    {
        if (unit.GetComponent<CrystalsUnit>().energyInsideMe > 0 && unit.GetComponent<CrystalsUnit>().SystemsThisReceivedEnergyFrom.Count > 0 && !unit.GetComponent<CrystalsUnit>().SystemsThisReceivedEnergyFrom.Contains(gameObject))
            AddEnergy(unit);
    }    
}
