using UnityEngine;
using System.Collections;

public class Skill_StunRay : MonoBehaviour {
    
    #region Singleton

    private static Skill_StunRay _INSTANCE;

    public static Skill_StunRay INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = GameObject.FindObjectOfType<Skill_StunRay>();
            }
            return _INSTANCE;
        }
    }
    #endregion

    private Transform _sphere;
    private Vector3 _initialScale;
    private Vector3 _initialPosition;

    private bool _canCast= true;

	void Start () 
    {

        _sphere = transform.FindChild("Sphere");
        _initialPosition = _sphere.position;
        _initialScale = _sphere.localScale;
	}

    public void CastStunRay(Vector3 pos)
    {
        if (!_canCast)
            return;

        _canCast = false;
        _sphere.localScale = _initialScale;
        _sphere.position = pos;

        SoundManager.INSTANCE.PlaysSingleFileByName("Stun", pos);

        StartCoroutine(GrowingBall());            
    }

    IEnumerator GrowingBall()
    {
        for (float timer = 0; timer < 1; timer += Time.deltaTime)
        {            
            _sphere.localScale += new Vector3(0.05F, 0.05f, 0.05f);
            yield return 0;
        }
        ReturnSphereToOriginal();    
    }

    void ReturnSphereToOriginal()
    {
        _canCast = true;
        _sphere.position = _initialPosition;
        _sphere.localScale = _initialScale;
    }
     
}
