using UnityEngine;
using System.Collections;

public class ForceField_Button : MonoBehaviour {

    public GameObject ForceField;
    private ForceField _myForceFieldScript;

    private SECTR_AudioSource _audioSourceOff;

    private GameObject _buttonCore;
    private GameObject _structure;

    private Material _buttonMaterialActive;
    private Material _buttonMaterialSolved;


    public ForceField.ForceFieldStatus ButtonStatus;

    public void InitializeButton(GameObject go)
    {
        ForceField = go;
        _myForceFieldScript = ForceField.GetComponent<ForceField>();
        ButtonStatus = _myForceFieldScript.FieldStatus;

        _buttonCore = transform.FindChild("ButtonCore").gameObject;
        _structure = transform.FindChild("Structure").gameObject;

        _buttonMaterialActive = Resources.Load("Material/Firula") as Material;
        _buttonMaterialSolved = Resources.Load("Material/Generator 1") as Material;


        _audioSourceOff = transform.FindChild("AudioSource_off").GetComponent<SECTR_AudioSource>();

        ButtonInactive();
    }

    public void ClickForceFieldButton()
    {
        switch(ButtonStatus)
        {
            case global::ForceField.ForceFieldStatus.Inactive:
                ButtonActive();
                break;
            case global::ForceField.ForceFieldStatus.Active:
                ButtonInactive();
                break;
            case global::ForceField.ForceFieldStatus.Solved:
                break;        
        }
    }

    void ButtonInactive()
    {
        _structure.active = false;
        _buttonCore.transform.renderer.enabled = false;

        ButtonStatus = global::ForceField.ForceFieldStatus.Inactive;

        _myForceFieldScript.StopRhythmSequence();

    }

    void ButtonActive()
    {
        ButtonStatus = global::ForceField.ForceFieldStatus.Active;

        _buttonCore.transform.renderer.enabled = true;

        _audioSourceOff.Stop(true);

        _structure.active = true;
        _buttonCore.renderer.active = true;

        _myForceFieldScript.BeginRhythmSequence();
    }

    public void ButtonSolved()
    {
        _audioSourceOff.Stop(true);
        _buttonCore.renderer.material = _buttonMaterialSolved;
        _structure.active = true;
        _buttonCore.renderer.active = true;

        Destroy(this);
    }
}
