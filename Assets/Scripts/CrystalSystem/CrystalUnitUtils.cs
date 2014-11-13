using UnityEngine;
using System.Collections;

public static class  CrystalUnitUtils 
{
    public static void UnitCheckSystemType(this CrystalsUnit crystalUnit)
    {
        switch (crystalUnit.transform.tag)
        {  
            case "Generator":
                crystalUnit.systemType = CrystalsUnit.SystemType.Generator;
                break;
            case "Replicator":
                crystalUnit.systemType = CrystalsUnit.SystemType.Replicator;
                break;
            case "Bridge":
                crystalUnit.systemType = CrystalsUnit.SystemType.Bridge;
                break;
            case "Unstable":
                crystalUnit.systemType = CrystalsUnit.SystemType.Unstable;
                break;
            case "Pin":
                crystalUnit.systemType = CrystalsUnit.SystemType.Pin;
                break;
        }
    }

    public static void UnitGrabAnimator(this CrystalsUnit t)
    {
        foreach (Transform item in t.transform)
        {
            if (item.GetComponent<Animator>())
                t.unitAnimator = item.GetComponent<Animator>();
        }

        if (t.unitAnimator == null)
            t.unitAnimator = new Animator();
    }


    public static void UnitCheckSystemConnections(this CrystalsUnit crystalUnit)
    {
        for (int i = 0; i < crystalUnit.ConnectedToMe.Count; i++)
        {
            
        }
    }

    public static void UnitCheckSystemStatus()
    { 
        
    }
}
