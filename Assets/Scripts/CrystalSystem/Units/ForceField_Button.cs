using UnityEngine;
using System.Collections;

public class ForceField_Button : MonoBehaviour {

    public GameObject ForceField;
    private ForceField _myForceFieldScript;

    public SECTR_AudioSource _audioSourceOff;

    public MeshRenderer buttonCore;
    public GameObject buttonRenderer;

    private Material _buttonMaterialActive;
    private Material _buttonMaterialSolved;


    public ForceField.ForceFieldStatus ButtonStatus;

    void Awake()
    {
        buttonRenderer.active = false;
        buttonCore.enabled = false;
    }

    public void InitializeButton(GameObject go)
    {
        ForceField = go;
        _myForceFieldScript = ForceField.GetComponent<ForceField>();
        ButtonStatus = _myForceFieldScript.fieldStatus;

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
        buttonRenderer.active = false;

        buttonCore.enabled = false;

        ButtonStatus = global::ForceField.ForceFieldStatus.Inactive;

        _myForceFieldScript.StopRhythmSequence();

    }

    void ButtonActive()
    {
        buttonRenderer.active = true;

        buttonCore.enabled = true;

        ButtonStatus = global::ForceField.ForceFieldStatus.Active;

        buttonCore.transform.renderer.enabled = true;

        _audioSourceOff.Stop(true);

        buttonCore.renderer.active = true;

        _myForceFieldScript.BeginRhythmSequence();
    }

    public void ButtonSolved()
    {
        _audioSourceOff.Stop(true);
        buttonCore.renderer.material = _buttonMaterialSolved;
        buttonCore.renderer.active = true;

        Destroy(this);
    }
}
