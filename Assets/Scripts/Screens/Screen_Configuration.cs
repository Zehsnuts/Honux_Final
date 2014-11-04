using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Screen_Configuration : MonoBehaviour {

    public UISlider slider;
    public UIPopupList popUpList;
    public UIToggle fullScreenToggle;

    private bool _isFullScreen;
     

    List<Resolution> resolutions = new List<Resolution>();

    void OnEnable()
    {
        fullScreenToggle.value = _isFullScreen = Screen.fullScreen;

        if (resolutions.Count < 1)
        {
            resolutions = ResolutionManager.GetCompatibleResolutions();

            for (int i = 0; i < resolutions.Count; i++)
            {
                string res = resolutions[i].width + "x" + resolutions[i].height;
                string cRes = Screen.width+"x"+Screen.height;

                Debug.Log("Resolution:" + res + "\n Current Resolution:" +cRes);
                if (res == cRes)
                    popUpList.value = res;

                popUpList.AddItem(resolutions[i].width + "x" + resolutions[i].height);
            }
        }

       

        if (FindObjectOfType<SoundManager>() && slider !=null)
            slider.value = SoundManager.INSTANCE.masterVolume;
    }

    void Awake()
    {
        if (FindObjectOfType<SoundManager>() && slider != null)
            slider.value = SoundManager.INSTANCE.masterVolume;
    }

    public void ChangeVolume(float volumeValue)
    {
        SoundManager.INSTANCE.ChangeVolume(volumeValue);    
    }

    public void ChangeResolution(string newResolution)
    {
        string[] reso = newResolution.Split("x"[0]);

        ResolutionManager.ChangeResolution(int.Parse(reso[0]), int.Parse(reso[1]),_isFullScreen);
    }

    public void ChangeToFullScreen(bool checkBox)
    {
        _isFullScreen = checkBox;
        ResolutionManager.ChangeResolution(Screen.currentResolution.width, Screen.currentResolution.height, checkBox);
    }
}
