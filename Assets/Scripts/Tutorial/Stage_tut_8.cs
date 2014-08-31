﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage_tut_8 : TutorialManager {

	void Start () 
    {
        Steps.Add("ShowTutorialPanel"); //Mostrar a primeira imagem: Desfazer ligação;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/8-1") as Material);

        Steps.Add("WaitForMouseInput"); //Esperar comando;        
        ClickTarget.Add(GameObject.Find("RhythmButton_1"));

        Steps.Add("ShowTutorialPanel"); //Mostrar a segunda imagem: Clicar no gerador depois em uma peça;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/8-2") as Material);

        Steps.Add("WaitForMouseInput"); //Esperar comando;        
        ClickTarget.Add(GameObject.Find("ForceField_1"));

        Steps.Add("ShowTutorialPanel"); //Mostrar a segunda imagem: Clicar no gerador depois em uma peça;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/8-3") as Material);
        
        Steps.Add("WaitForKeyboardInput"); //Esperar comando: Espaço;        
        Inputs.Add(KeyCode.A);
        
        if(Steps.Count > 0)
            ReceiveSteps(Steps, Inputs, Mats, ClickTarget);
	}

    public override void CheckMouseInput()
    {
        Ray ray = ActualCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log(_clickTargetList[_currentTarget]);
                if (hit.transform.gameObject == _clickTargetList[_currentTarget] || hit.transform.parent.gameObject == _clickTargetList[_currentTarget])
                {
                    Debug.Log(hit.transform.name);    

                    _currentTarget++;
                    _isWaitingInput = false;
                }
            }
        }
    }

    public override void CheckKeyboardInput()
    {
        if (Input.GetKey(_inputKeyList[_currentInput]))
        {
            _isWaitingInput = false;
        }

    }
}
