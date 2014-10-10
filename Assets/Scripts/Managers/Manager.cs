using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
    void OnGUI()
    {
        GUILayout.Label("Version 0.2.0");
    }
    void Awake()
    {
        if (!GameObject.FindObjectOfType<EventManager>())
        { 
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "EventManager";
            go.AddComponent<EventManager>();
        }

        if (!GameObject.FindObjectOfType<SoundManager>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "SoundManager";
            go.AddComponent<SoundManager>();
        }

        if (!GameObject.FindObjectOfType<GUIManager>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "GUIManager";
            go.AddComponent<GUIManager>();
        }

        if (!GameObject.FindObjectOfType<LoadingManager>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "LoadingManager";
            go.AddComponent<LoadingManager>();
        }

        if (!GameObject.FindObjectOfType<StageManager>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "StageManager";
            go.AddComponent<StageManager>();
        }

        if (!GameObject.FindObjectOfType<CameraControl>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "CameraControl";
            go.AddComponent<CameraControl>();
        }

        if (!GameObject.FindObjectOfType<ResourcesManager>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "ResourcesManager";
            go.AddComponent<ResourcesManager>();
        }
        if (!GameObject.FindObjectOfType<ConnectionsManager>())
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = "ConnectionsManager";
            go.AddComponent<ConnectionsManager>();

            if (!GameObject.FindObjectOfType<ConnectionFunctions>())
            {
                var gmo = new GameObject();
                gmo.transform.parent = go.transform;
                gmo.name = "ConnectionFunctions";
                gmo.AddComponent<ConnectionFunctions>();
            }
        }
    }

 
}
