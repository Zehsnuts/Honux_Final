using UnityEngine;
using System.Collections;

public static class CrystalUnitAnimator
{
    public static float AnimationGetTime(this Animator unitAnimator, string animationHashString)
    {        
        var hash = Animator.StringToHash(animationHashString);

        unitAnimator.SetTrigger(hash);

        var animationState = unitAnimator.GetCurrentAnimatorStateInfo(0);

        var animationClips = unitAnimator.GetCurrentAnimationClipState(0);
        Debug.Log("animationState for:" + animationState);

        if (animationClips.Length == 0)
            return 2;

        Debug.Log("Normalized: "+animationState.normalizedTime);

        var animationClip = animationClips[0].clip;
        var animationTime = animationClip.length * animationState.normalizedTime;

        Debug.Log("Waiting for:" + animationClip);

        Debug.Log("Waiting for:" + animationTime/2);

        return 1.7f;        
    }
}

