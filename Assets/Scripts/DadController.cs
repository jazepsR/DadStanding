using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadController : MonoBehaviour
{
    public Animator dadAnimator;
    float sliderValue = 0;
    Collider2D dadBody;
    [SerializeField] private SliderScript slider;
    Material dadMaterial;
    float flashTime = 0.2f;

    private void Awake()
    {
        dadBody = GetComponent<Collider2D>();
        dadMaterial = GetComponent<SpriteRenderer>().material;
    }


    // Start is called before the first frame update
    public void SetDadAnimation(float value)
    {
        sliderValue = value;
        dadAnimator.SetFloat("balance", value);
    }

    private void Update()
    {
        if(dadAnimator.GetCurrentAnimatorStateInfo(0).IsName("DodgeR") || dadAnimator.GetCurrentAnimatorStateInfo(0).IsName("DodgeL"))
        {
            dadBody.enabled = false;
        }
        else
        {
            dadBody.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "projectile")
        {
            Debug.LogError("got hit by projectile trigger");
            Destroy(collision.gameObject);
            projectile projectile = collision.gameObject.GetComponent<projectile>();
            if (projectile)
            {
                slider.GetHit(projectile.moveSpeed > 0? GameManager.instance.activeLevel.projectileStrenght : -GameManager.instance.activeLevel.projectileStrenght);
            }
            dadMaterial.SetFloat("_FlashAmount", 1);
            StartCoroutine(DisableFlash());
            GameManager.instance.score -= 20;
        }
    }

    IEnumerator DisableFlash()
    {
        yield return new WaitForSeconds(flashTime);
        dadMaterial.SetFloat("_FlashAmount", 0);

    }
    public void SetWinState()
    {
        dadAnimator.SetTrigger("win");
    }

    public void TriggerDodge()
    {
        dadAnimator.SetTrigger(sliderValue>-0.3f ? "dodgeL": "dodgeR");
    }
    public void ResetDad()
    {
        dadAnimator.runtimeAnimatorController = GameManager.instance.activeLevel.dadAnimationController;
        dadAnimator.Rebind();
        dadAnimator.Update(0f);
    }
}
