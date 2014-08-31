using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage_tut_1 : TutorialManager {


    void Start()
    {
        Steps.Add("ShowTutorialPanel"); //Mostrar a primeira imagem: Aperte espaço;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/1-1") as Material);


        Steps.Add("WaitForKeyboardInput"); //Esperar comando: Espaço;        
        Inputs.Add(KeyCode.Space);

        Steps.Add("ShowTutorialPanel"); //Mostrar a primeira imagem: Parabéns;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/1-2") as Material);

        Steps.Add("WaitForKeyboardInput"); //Esperar comando: Espaço;        
        Inputs.Add(KeyCode.Space);

        if (Steps.Count > 0)
            ReceiveSteps(Steps, Inputs, Mats, ClickTarget);
    }

     public override void CheckKeyboardInput()
       {
        if (Input.GetKey(_inputKeyList[_currentInput]))
        {
            _isWaitingInput = false;
            EventManager.INSTANCE.CallChangeCamera();
        }
    
	}
	

}
