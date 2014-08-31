using UnityEngine;
using System.Collections;

public class CrystalsUnit_Unstable : CrystalUnitFunctions
{
    public override void TurnMeOn()
    {
        if (!isThisSystemOn)
            EventManager.INSTANCE.CallUnstableTurnOn();

        base.TurnMeOn();

    }

    public override void TurnMeOff()
    {
        if (isThisSystemOn)
            EventManager.INSTANCE.CallUnstableTurnOff();  
        

        base.TurnMeOff();

        
    }
}
