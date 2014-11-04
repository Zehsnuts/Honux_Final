using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResolutionManager : MonoBehaviour 
{
    public static List<Resolution> GetCompatibleResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<Resolution> reso = new List<Resolution>();

        foreach (Resolution res in resolutions)
        {
            reso.Add(res);
        }

        return reso;
    }   

    public static void  ChangeResolution(int width, int height, bool full)
    {
        Debug.Log("Trying to change resolution. From:" + Screen.currentResolution.width + "x" + Screen.currentResolution.height);

        if (Screen.currentResolution.width == width && Screen.currentResolution.height == height && full == Screen.fullScreen)
            return;

        Debug.Log("Changing resolution to:" + width +"x"+ height);
        Screen.SetResolution(width, height, full);    
    }
}
