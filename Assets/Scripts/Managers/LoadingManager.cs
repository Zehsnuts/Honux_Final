using UnityEngine;
using System.Collections;

public class LoadingManager: MonoBehaviour
{
    #region Singleton

    private static LoadingManager _INSTANCE;
    public static LoadingManager INSTANCE
    {
        get 
        {
            if (_INSTANCE == null)
                _INSTANCE = GameObject.FindObjectOfType<LoadingManager>();

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
    }
    #endregion

    public string levelToLoad;

    private GameObject _loadBackground;
    private GameObject _loadText;
    private GameObject _loadProgressBar;
    private bool _shoulShowLoadingScreen;

    private bool _isCurrentlyLoading = false;

    private int _loadingPercentageProgress = 0;
    private bool _isLoading = false;
    

    void Start()
    {
        _isCurrentlyLoading = false;

        CameraFade.StartAlphaFade(Color.black, true, 2);
        ShowOrHideLoadingScreenStatus(false);        
    }

    IEnumerator DisplayLoadingScreen(string level)
    {
        Vector3 probar = _loadProgressBar.transform.localScale;
        _isLoading = true;
        ShowOrHideLoadingScreenStatus(true);

        _loadProgressBar.transform.localScale = new Vector3(_loadingPercentageProgress, probar.y, probar.z);
        _loadText.guiText.text = "Loading Progress\n " + _loadingPercentageProgress+"%";

        AsyncOperation async = Application.LoadLevelAsync(level);
        async.allowSceneActivation = false;

        while (_isLoading)
        {   
            _loadingPercentageProgress = (int)(async.progress * 100);
            _loadText.guiText.text = "Loading Progress\n " + _loadingPercentageProgress + "%";
            _loadProgressBar.transform.localScale = new Vector3(async.progress, probar.y, probar.z);

            //Se o progresso for mais que 90%. Tem que ser assim pq o async.progress para em 0.9
            if (async.progress>=0.9f && _isLoading)
            {
                _isLoading = false; 
                CameraFade.StartAlphaFade(Color.black, false, 3, 0, ()=>FinishLoadingScreen(async));
                //StartCoroutine(FinishLoadingScreen(async));
                _loadText.guiText.text = "Loading Progress\n 100%";
                _loadProgressBar.transform.localScale = new Vector3(1, probar.y, probar.z);                
            }
            yield return null;
        }        
    }

    void ShowOrHideLoadingScreenStatus(bool shouldShow)
    {
        if (_loadBackground == null || _loadText==null || _loadProgressBar == null)
            GrabScreenElements();

        _shoulShowLoadingScreen = shouldShow;

        _loadBackground.SetActive(_shoulShowLoadingScreen);
        _loadText.SetActive(_shoulShowLoadingScreen);
        _loadProgressBar.SetActive(_shoulShowLoadingScreen);
    }

    void GrabScreenElements()
    {
        _loadBackground = new GameObject();
        _loadBackground.transform.parent = transform;
        _loadBackground.name = "Background";
        _loadBackground.AddComponent<GUITexture>();
        _loadBackground.GetComponent<GUITexture>().texture = Resources.Load("Textures/Black") as Texture;
        _loadBackground.transform.position = new Vector3(0.5f, 0.5f, -1f);

        _loadProgressBar = new GameObject();
        _loadProgressBar.transform.parent = transform;
        _loadProgressBar.name = "ProgressBar";
        _loadProgressBar.AddComponent<GUITexture>();
        _loadProgressBar.GetComponent<GUITexture>().texture = Resources.Load("Textures/White") as Texture;
        _loadProgressBar.transform.position = new Vector3(0.5f, 0.2f, 0);
        _loadProgressBar.transform.localScale = new Vector3(1, -0.03f, 1);

        _loadText = new GameObject();
        _loadText.transform.parent = transform;
        _loadText.name = "Text";
        _loadText.AddComponent<GUIText>();
        _loadText.GetComponent<GUIText>().text = "Loading Progress";
        _loadText.transform.position = new Vector3(0.5f, 0.5f, 0);

    }

    public void LoadLevelByName(string level)
    {
        if (_isCurrentlyLoading)
            return;

        _isCurrentlyLoading = true;

        EventManager.INSTANCE.CallLoadingStart();

        levelToLoad = level;

        StartCoroutine(DisplayLoadingScreen(levelToLoad));
    }

    void FinishLoadingScreen(AsyncOperation async)
    {
        async.allowSceneActivation = true;
        _isCurrentlyLoading = false;
        EventManager.INSTANCE.CallLoadingEnd();
    }    
}
