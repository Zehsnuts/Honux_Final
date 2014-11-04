using UnityEngine;
using System.Collections;

public class CrystalUnitAnimator : MonoBehaviour
{
    private Animator _anim;

    int turnOnHash = Animator.StringToHash("turnUnitOn");

    void Awake()
    {
        _anim = gameObject.GetComponent<CrystalsUnit>().animator;
    }

    public float TurnOnAnimation()
    {
        _anim.SetTrigger(turnOnHash);        

        return _anim.gameObject.animation.clip.length;
    }
}
