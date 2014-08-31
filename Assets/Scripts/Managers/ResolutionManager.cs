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
        Screen.SetResolution(width, height, full);    
    }
}
