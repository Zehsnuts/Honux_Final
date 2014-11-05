using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TutorialObject : MonoBehaviour 
{
    public enum TutorialSteps 
    {
        KeyPress, TargetClick, WaitForTrackCount, WaitForRobotSteal
    }

    public TutorialSteps tutorialSteps;

    public string clickTargetName;

    private GameObject _clickTarget;
    public GameObject clickTarget;
    public KeyCode pressKeys;

    public delegate void OnTutorialStepCompleted();
    public event OnTutorialStepCompleted TUTORIALSTEPCOMPLETED;

    public void CallTutorialStepComplete()
    {
        if (TUTORIALSTEPCOMPLETED != null)
            TUTORIALSTEPCOMPLETED();
        GameStateManager.INSTANCE.SpecialGameState = GameStateManager.SpecialGameStates.FreeInput;
        Destroy(gameObject);
    }

    public void BeginStep()
    {
        GameStateManager.INSTANCE.SpecialGameState = GameStateManager.SpecialGameStates.OnTutorial;
        if (clickTarget == null)
            clickTarget = GameObject.Find(clickTargetName);

        switch (tutorialSteps)
        {
            case TutorialSteps.KeyPress:
                StartCoroutine(WaitForKeyPress());
                break;
        }
    }

    IEnumerator WaitForKeyPress()
    {
        bool isWaitingForKeyPress = true;
        while (isWaitingForKeyPress)
        {
            yield return 0;
            if (Input.GetKeyDown(pressKeys))            
                isWaitingForKeyPress = false;            
        }

        CallTutorialStepComplete();
    }

    public GameObject TryClickTarget(RaycastHit hit)
    {
        if (hit.transform.parent.parent.gameObject == clickTarget ||
            hit.transform.parent.gameObject == clickTarget ||
            hit.transform.gameObject == clickTarget)
        {
            CallTutorialStepComplete();
            return hit.transform.gameObject;
        }
        else
            return null;
    }

    
}
