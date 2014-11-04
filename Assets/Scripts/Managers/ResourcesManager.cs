using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour
{

    #region Singleton
    private static ResourcesManager _INSTANCE;
    public static ResourcesManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<ResourcesManager>();

            }
            return _INSTANCE;
        }
    }

    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _INSTANCE)
                Destroy(this.gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }
    #endregion
      
    public int _tracks = 0;

    public void SetNumberOfInitialTrack(int i)
    {
        _tracks = i;
    }

    public void AddTrack()
    {
        _tracks++;
    }
    public void RemoveTrack()
    {
        _tracks--;
    }
    public int GetTracksNumber()
    {
        return _tracks;
    }
}