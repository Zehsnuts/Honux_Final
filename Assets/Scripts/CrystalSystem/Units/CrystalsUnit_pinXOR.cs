using UnityEngine;
using System.Collections;

public class CrystalsUnit_pinXOR : CrystalUnitFunctions {

    public override bool CheckEnergy()
    {
        if (energyInsideMe == 1 &&  ConnectedToMe.Count > 1)
            return true;
        else
            return false;
    }

    public override void TurnMeOn()
    {
        if (!isThisSystemOn)
        {
            EventManager.INSTANCE.CallPinTurnOn_Xor();

            CrystalsControl.INSTANCE.TurnThisSystemOn(transform);

            isThisSystemOn = true;
        }
    }

    public override void TurnMeOff()
    {
        if (isThisSystemOn )
            EventManager.INSTANCE.CallPinTurnOff_Xor();

        CrystalsControl.INSTANCE.TurnThisSystemOff(transform);

        isThisSystemOn = false;

        if (energyInsideMe < 1)
            SystemsThisReceivedEnergyFrom.Clear();

    }
}
