using UnityEngine;
using System.Collections;

public class MainMenuBackgroundExpansion : MonoBehaviour {

	
	void Start ()
    {
        transform.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(gameObject, iTween.Hash("x", 2, "y", 2, "z", 2, "time", 20, "delay", 2, "onComplete", "ScaleAndGrow", "onCompleteTarget", gameObject));
	}

    void ScaleAndGrow()
    {
        transform.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(gameObject, iTween.Hash("x", 2, "y", 2, "z", 2, "time", 20, "delay", 2, "onComplete", "ScaleAndGrow", "onCompleteTarget", gameObject));
    }

	
}
