using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TutorialUtils
{
    public static void CreateTutorialObjects(this List<Transform> toList)
    {
        Object[] objList = Resources.LoadAll("Tutorials");

        foreach (var item in objList)
        {
            GameObject go = new GameObject();
            go.AddClassesAccordingToName(item);
        }
    }

    public static void AddClassesAccordingToName(this GameObject go, Object item)
    {
        go.name = item.name;

        switch (go.name)
        { 
            case "TO_1_Stage_0_0":
                go.AddComponent<TO_1_Stage_0_0>();
                break;
        }
    }
}
