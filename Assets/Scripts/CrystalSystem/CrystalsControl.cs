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
        t.tag = "Off";

        if (Application.loadedLevelName != "Stage_Null" && t.GetComponent<CrystalsUnit>().systemType != CrystalsUnit.SystemType.Pin)
            foreach (Transform item in t)
            {
                if (item.GetComponent<Animator>())                
                    item.gameObject.SetActive(false);
                
                if (item.GetComponent<MeshRenderer>())
                    item.GetComponent<MeshRenderer>().enabled = false;

                foreach (Transform i in item)
                {
                    if (i.GetComponent<MeshRenderer>())
                        i.GetComponent<MeshRenderer>().enabled = false;
                }
            }

        TurnShadowOn(t);

        switch (t.GetComponent<CrystalsUnit>().systemType)
        { 
            case CrystalsUnit.SystemType.Pin:
                TurnPinOff(t);
                break;
        }
    }

    public void TurnThisSystemOn(Transform t)
    {
        t.tag = "Off";
        if (Application.loadedLevelName != "Stage_Null")
            foreach (Transform item in t)
            {
                if (item.GetComponent<Animator>())
                {
                    item.gameObject.SetActive(true);

                    StartCoroutine(WaitAnimationBeforeTurningOn(t));
                }

                if (item.GetComponent<MeshRenderer>())
                    item.GetComponent<MeshRenderer>().enabled = true;
                foreach (Transform i in item)
                {
                    if (i.GetComponent<MeshRenderer>())
                        i.GetComponent<MeshRenderer>().enabled = true;
                }
            }



        switch (t.GetComponent<CrystalsUnit>().systemType)
        {
            case CrystalsUnit.SystemType.Pin:
                TurnPinOn(t);
                break;
        }
    }

    IEnumerator WaitAnimationBeforeTurningOn(Transform t)
    {
        if (t.gameObject.GetComponent<CrystalUnitAnimator>())
        yield return new WaitForSeconds(t.gameObject.GetComponent<CrystalUnitAnimator>().TurnOnAnimation());
        else
            yield return new WaitForSeconds(1);

        if (t.GetComponent<CrystalsUnit>().isSystemSuposedToTurnOn)
        {
            t.GetComponent<CrystalUnitFunctions>().TurnOnAfterAnimation();

            TurnShadowOff(t);
        }
    }

    #region TurnOn    

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
        t.gameObject.active = true;

        if (t.FindChild("AudioSource_on"))
            t.FindChild("AudioSource_on").GetComponent<SECTR_PointSource>().Stop(true);

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
