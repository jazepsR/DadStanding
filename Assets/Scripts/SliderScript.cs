using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum SliderState { green, yellow, orange, red};
public class SliderScript : MonoBehaviour
{
    private SliderState prevSliderState = SliderState.green;
    public Slider balanceSlider;
    public InputAction balanceInput;
    private float sliderSpeed;
    private float yVelocity = 0.0f;
    // Start is called before the first frame update
    [SerializeField] private DadController DadController;
    [SerializeField] private GameObject greenFeedback, yellowFeedback, orangeFeedback, redFeedback;
    [SerializeField] private Transform handleTransform;
    [SerializeField] private Image handleImage;
    [SerializeField] private Color dangerColor = Color.red, orangeColor = Color.red, yellowColor = Color.yellow, greenColor = Color.green;
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
            SliderState sliderState = GetSliderState(balanceSlider.value);
            if (sliderState != prevSliderState && sliderState != SliderState.green && sliderState != SliderState.yellow)
                ShowFeedback(sliderState);
            handleImage.color = GetStateColor(sliderState);
            prevSliderState = sliderState;
        }
    }
    public int GetSliderMultiplier()
    {
        return GetSliderMultiplier(GetSliderState(balanceSlider.value));
    }
    public Color GetStateColor(SliderState sliderState)
    {
        switch (sliderState)
        {
            case SliderState.green:
                return greenColor;
            case SliderState.yellow:
                return yellowColor;
            case SliderState.orange:
                return orangeColor;
            case SliderState.red:
                return dangerColor;
        }
        return Color.white;
    }

    public int GetSliderMultiplier(SliderState sliderState)
    {
        switch (sliderState)
        {
            case SliderState.green:
                return 5;
            case SliderState.yellow:
                return 3;
            case SliderState.orange:
                return 2;
            case SliderState.red:
                return 1;
        }
        return 0;
    }

    public void ShowFeedback(SliderState sliderState)
    {
        GameObject feedbackObj = null;
        switch(sliderState)
        {
            case SliderState.green:
                feedbackObj = greenFeedback;
                break;
            case SliderState.yellow:
                feedbackObj = yellowFeedback;
                break;
            case SliderState.orange:
                feedbackObj = orangeFeedback;
                break;
            case SliderState.red:
                feedbackObj = redFeedback;
                break;
        }
        GameObject instantiatedObj = Instantiate(feedbackObj, handleTransform);
        Destroy(instantiatedObj, instantiatedObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    public SliderState GetSliderState(float sliderValue)
    {
        sliderValue = Mathf.Abs(sliderValue);
        if (sliderValue > 0.8f)
        {
            return SliderState.red;
        }
        else if (sliderValue > 0.5f)
        {
            return SliderState.orange;
        }
        else if (sliderValue > 0.3f)
        {
            return SliderState.yellow;
        }
        else
        {
            return SliderState.green;
        }
    }

    public void Reset()
    {
        DadController.ResetDad();
        balanceSlider.value = 0;
        handleImage.color = GetStateColor(SliderState.green);
        prevSliderState = SliderState.green;
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
