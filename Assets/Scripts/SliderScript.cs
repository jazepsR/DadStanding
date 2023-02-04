using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class SliderScript : MonoBehaviour
{
    public Slider balanceSlider;
    public InputAction balanceInput;
    public float sliderMoveMultiplier;
    public float dampingTime = 0.5f;
    private float sliderSpeed;
    private float yVelocity = 0.0f;
    [SerializeField] private float maxSpeed = 0.1f;
    // Start is called before the first frame update
    [SerializeField] private DadController DadController;
    void Start()
    {
        balanceInput.performed += ctx => OnInput(ctx.ReadValue<float>());
        balanceInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.Playing)
        {
            if(GameManager.instance.activeLevel.controlType == ControlType.Hold)
                sliderSpeed -= sliderMoveMultiplier * balanceInput.ReadValue<float>();
            sliderSpeed += GameManager.instance.GetRandomnessSpeed();
            //Debug.LogError("randomness speed: " + GameManager.instance.GetRandomnessSpeed());
            sliderSpeed = Mathf.Clamp(sliderSpeed, -maxSpeed, maxSpeed);
            balanceSlider.value = balanceSlider.value + sliderSpeed* Time.deltaTime;
            sliderSpeed = Mathf.SmoothDamp(sliderSpeed, 0, ref yVelocity, dampingTime);
            DadController.SetDadAnimation(balanceSlider.value);
            if (Mathf.Abs(balanceSlider.value) > 0.99f)
            {
                GameManager.instance.SetLoseState();
                DadController.SetDadAnimation(balanceSlider.value >0? 2 :-2);
            }
        }
    }
    public void Reset()
    {
        DadController.ResetDad();
        balanceSlider.value = 0;
        sliderSpeed = 0;
    }
    private void OnInput(float value)
    {
        if(GameManager.instance.activeLevel.controlType == ControlType.Tap)
            sliderSpeed -= value * sliderMoveMultiplier;
    }
}
