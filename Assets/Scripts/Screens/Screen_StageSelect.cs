using UnityEngine;
using System.Collections;

public class Screen_StageSelect : MonoBehaviour {

	void Start ()
    {
        EventManager.INSTANCE.CallStageSelectStart();
	}
}
