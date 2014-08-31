using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {


    #region Singleton
    private static GUIManager _INSTANCE;
    public static GUIManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<GUIManager>();
                //DontDestroyOnLoad(_INSTANCE.gameObject);
            }
            return _INSTANCE;
        }
    }

    #endregion

    private GUIText _distributionText;
    private string _displayText;

	// Use this for initialization
	void Start () 
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _INSTANCE)
                Destroy(this.gameObject);
        }

        GameObject go = new GameObject("GUIText");
        go.transform.parent = transform;
        _distributionText = (GUIText)go.AddComponent(typeof(GUIText));
        _distributionText.text = "Hello";
        _distributionText.pixelOffset = new Vector2(20, 20);
	}
	
	// Update is called once per frame
	void OnGUI ()
    {
        _distributionText.text = _displayText;
	}

    public void ChangeDisplayText(string str)
    {
        _displayText = str;
    }
}
