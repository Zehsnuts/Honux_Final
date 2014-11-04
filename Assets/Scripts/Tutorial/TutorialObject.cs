using UnityEngine;
using System.Collections;

[System.Serializable]
public class TutorialObject : MonoBehaviour 
{
    public delegate void OnTutorialStepCompleted();
    public static event OnTutorialStepCompleted TUTORIALSTEPCOMPLETED;

    public void CallTutorialStepComplete()
    { 
        
    }
}
