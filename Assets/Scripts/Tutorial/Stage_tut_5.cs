using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage_tut_5 : TutorialManager {

	void Start () 
    {
        Steps.Add("ShowTutorialPanel"); //Mostrar a primeira imagem: Desfazer ligação;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/5-1") as Material);

        Steps.Add("WaitForKeyboardInput"); //Esperar comando: Espaço;        
        Inputs.Add(KeyCode.A);
        Steps.Add("ShowTutorialPanel"); //Mostrar a primeira imagem: Desfazer ligação;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/5-2") as Material);

        Steps.Add("WaitForKeyboardInput"); //Esperar comando: Espaço;        
        Inputs.Add(KeyCode.A);
        Steps.Add("ShowTutorialPanel"); //Mostrar a primeira imagem: Desfazer ligação;
        Mats.Add(Resources.Load("PNG/Tutorial/Materials/5-3") as Material);

        Steps.Add("WaitForKeyboardInput"); //Esperar comando: Espaço;        
        Inputs.Add(KeyCode.P);

        
        
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
}
