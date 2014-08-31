using UnityEngine;
using System.Collections;

public class CrystalsUnit_pinOR : CrystalUnitFunctions {
    
    public override bool CheckEnergy()
    {
        if (energyInsideMe >= 1 && ConnectedToMe.Count >1)
            return true;
        else
            return false;
    }

    public override void TurnMeOn()
    {
        if(!isThisSystemOn)
            EventManager.INSTANCE.CallPinTurnOn_Or();

        CrystalsControl.INSTANCE.TurnThisSystemOn(transform);
        isThisSystemOn = true;

    }

    public override void TurnMeOff()
    {
        if(isThisSystemOn)
            EventManager.INSTANCE.CallPinTurnOff_Or();

        CrystalsControl.INSTANCE.TurnThisSystemOff(transform);
        isThisSystemOn = false;

        SystemsThisReceivedEnergyFrom.Clear();        

    }
}
