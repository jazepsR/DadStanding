using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadController : MonoBehaviour
{
    public Animator dadAnimator;
    // Start is called before the first frame update
    public void SetDadAnimation(float value)
    {
        dadAnimator.SetFloat("balance", value);
    }
    public void SetWinState()
    {
        dadAnimator.SetTrigger("win");
    }
    public void ResetDad()
    {
        dadAnimator.runtimeAnimatorController = GameManager.instance.activeLevel.dadAnimationController;
        dadAnimator.Rebind();
        dadAnimator.Update(0f);
    }
}
