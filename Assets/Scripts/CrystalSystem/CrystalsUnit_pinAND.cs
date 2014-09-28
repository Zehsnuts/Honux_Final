using UnityEngine;
using System.Collections;

public class CrystalsUnit_pinAND : CrystalUnitFunctions {


    public override bool CheckEnergy()
    {
        if (energyInsideMe == ConnectedToMe.Count && ConnectedToMe.Count >1)
            return true;
        else
            return false;
    }

    void Start()
    {
        GrabAudioSource();
        CheckSystemType(); //Qual tipo de unidade esse GameObject é;    
        energyNeededToWork = 2;
        ChangeSystemStatus(); //Usa o resultado de CheckEnergy para mudar o status do systema para on ou off;
        
    }

    public override void TurnMeOn()
    {
        if(!isThisSystemOn)
            EventManager.INSTANCE.CallPinTurnOn_And();

        CrystalsControl.INSTANCE.TurnThisSystemOn(transform);

        isThisSystemOn = true;
    }

    public override void TurnMeOff()
    {
        if (isThisSystemOn)
        {
            EventManager.INSTANCE.CallPinTurnOff_And();            
        }

        if(energyInsideMe<1)
            SystemsThisReceivedEnergyFrom.Clear();

        CrystalsControl.INSTANCE.TurnThisSystemOff(transform);
        isThisSystemOn = false;

    }
}
