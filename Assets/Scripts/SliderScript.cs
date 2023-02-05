using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class SliderScript : MonoBehaviour
{
    public Slider balanceSlider;
    public InputAction balanceInput;
    private float sliderSpeed;
    private float yVelocity = 0.0f;
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
                sliderSpeed -= GameManager.instance.activeLevel.sliderMoveSpeed * balanceInput.ReadValue<float>();
            sliderSpeed += GameManager.instance.GetRandomnessSpeed();
            balanceSlider.value = balanceSlider.value + sliderSpeed* Time.deltaTime;
            sliderSpeed = Mathf.SmoothDamp(sliderSpeed, 0, ref yVelocity, GameManager.instance.activeLevel.dampingTime);
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

    public void SetWinState()
    {
      //  DadController.SetWinState();
    }

    public void SetPunchlineState()
    {
        DadController.SetWinState();
    }
    private void OnInput(float value)
    {
        if(GameManager.instance.activeLevel.controlType == ControlType.Tap)
            sliderSpeed -= value * GameManager.instance.activeLevel.sliderMoveSpeed;
    }
}
