using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalsControl : MonoBehaviour 
{
    #region Singleton

    private static CrystalsControl _INSTANCE;

    public static CrystalsControl INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<CrystalsControl>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    public Material _structureOn;
    public Material _structureOff;

    void Start()
    {
        _structureOn = Resources.Load("Materials/ReplicatorOn") as Material;

        _structureOff = Resources.Load("Materials/ReplicatorOff") as Material;
    }

    public void TurnThisSystemOff(Transform t)
    {
        switch (t.GetComponent<CrystalsUnit>().systemType)
        { 
            case CrystalsUnit.SystemType.Replicator:
                TurnReplicatorOff(t);
                break;

            case CrystalsUnit.SystemType.Bridge:
                TurnReplicatorOff(t);
                break;

            case  CrystalsUnit.SystemType.Generator:
                TurnGeneratorOff(t);
                break;

            case CrystalsUnit.SystemType.Unstable:
                TurnUnstableCrystalOff(t);
                break;

            case CrystalsUnit.SystemType.Pin:
                TurnPinOff(t);
                break;
        }
    }

    public void TurnThisSystemOn(Transform t)
    {        
        switch (t.GetComponent<CrystalsUnit>().systemType)
        {
            case CrystalsUnit.SystemType.Replicator:
                TurnReplicatorOn(t);
                break;

            case CrystalsUnit.SystemType.Bridge:
                TurnReplicatorOn(t);
                break;

            case CrystalsUnit.SystemType.Generator:
                TurnGeneratorOn(t);
                break;

            case CrystalsUnit.SystemType.Unstable:
                TurnUnstableCrystalOn(t);
                break;

            case CrystalsUnit.SystemType.Pin:
                TurnPinOn(t);
                break;
        }
    }

    #region TurnOn

    void TurnReplicatorOn(Transform t)
    {
        t.tag = "On";
        t.FindChild("CoreOff").gameObject.active = false;
        //t.FindChild("Structure").gameObject.renderer.material = _structureOn
        t.FindChild("Structure").transform.renderer.material = _structureOff;

        TurnShadowOff(t);
    }
    
    void TurnUnstableCrystalOn(Transform t)
    {
        t.tag = "On";
        t.FindChild("CoreOff").gameObject.active = false;
        t.FindChild("Structure").gameObject.renderer.material = _structureOn;

        TurnShadowOff(t);
    }
    
    void TurnGeneratorOn(Transform t)
    {
        t.tag = "On";
        TurnShadowOff(t);
    }

    void TurnPinOn(Transform t)
    {
        t.tag = "On";

        t.FindChild("Structure").gameObject.GetComponent<MeshRenderer>().material = Resources.Load("Materials/PinOn") as Material;

        if (t.FindChild("AudioSource_on"))
            t.FindChild("AudioSource_on").GetComponent<SECTR_PointSource>().Play();      
            //t.FindChild("AudioSource_on").active = true;        

        if (t.FindChild("AudioSource_off"))
            t.FindChild("AudioSource_off").GetComponent<SECTR_PointSource>().Stop(true);
            //t.FindChild("AudioSource_off").active = false;
        
    }

    void TurnShadowOn(Transform t)
    {
        if (Application.loadedLevelName != "Stage_Null")
            t.gameObject.SetActiveRecursively(false);
        else
            t.gameObject.SetActiveRecursively(true);

        t.gameObject.active = true;

        if (t.FindChild("AudioSource_on"))
            t.FindChild("AudioSource_on").active = false;

        if (t.FindChild("AudioSource_off"))
        {
            t.FindChild("AudioSource_off").active = true;
            t.FindChild("AudioSource_off").GetComponent<SECTR_PointSource>().Play();
        }

        foreach (Transform trans in t)
        {
            if (trans.name.Contains("Track:"))
            {
                trans.gameObject.SetActiveRecursively(true);
                trans.GetComponent<ConnectorFunctions>().TurnTrackOff();
            }
        }
    }  
    #endregion

    #region TurnOff
    void TurnReplicatorOff(Transform t)
    {
        t.tag = "Off";
        t.FindChild("CoreOff").gameObject.active = true;
        t.FindChild("Structure").transform.renderer.material = _structureOff;

        TurnShadowOn(t);
    }

    void TurnUnstableCrystalOff(Transform t)
    {
        t.tag = "Off";
        t.FindChild("CoreOff").gameObject.active = true;
        t.FindChild("Structure").transform.renderer.material = _structureOff;        

        TurnShadowOn(t);
    }

    void TurnGeneratorOff(Transform t)
    {
        t.tag = "Off";
        TurnShadowOn(t);
    }

    void TurnPinOff(Transform t)
    {

        t.FindChild("Structure").gameObject.GetComponent<MeshRenderer>().material = Resources.Load("Materials/PinOff") as Material;

        t.tag = "Off";

        if (t.FindChild("AudioSource_on"))
            t.FindChild("AudioSource_on").active = false;

        if (t.FindChild("AudioSource_off"))
            t.FindChild("AudioSource_off").active = true;
    }

    void TurnShadowOff(Transform t)
    {
        if (Application.loadedLevelName != "Stage_Null")
            t.gameObject.SetActiveRecursively(true);
        else
            t.gameObject.SetActiveRecursively(true);


        if (t.FindChild("AudioSource_off"))
            t.FindChild("AudioSource_off").GetComponent<SECTR_PointSource>().Play();

        if (t.FindChild("AudioSource_on"))
            t.FindChild("AudioSource_on").GetComponent<SECTR_PointSource>().Stop(true);

        foreach (Transform trans in t)
        {
            if (trans.name.Contains("Track:"))
            {
                trans.gameObject.SetActiveRecursively(true);
                trans.GetComponent<ConnectorFunctions>().TurnTrackOn();
            }
        }

    }
    #endregion
}
