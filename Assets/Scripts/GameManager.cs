using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public enum GameState { Starting, Playing, Fail, Win}
public class GameManager : MonoBehaviour
{
    public static GameState gameState = GameState.Starting;
    public static GameManager instance;
    [SerializeField] private LevelData debugLevel;
    [SerializeField] private float randomnessFactor = 4f;
    [SerializeField] private float randomnessSpeed = 0.1f;
    public TMP_Text timer;
    private float levelTime;
    [HideInInspector] public LevelData activeLevel;
    [SerializeField] private JokeScriptable[] jokes;
    [HideInInspector] public JokeScriptable activeJoke;

    public SliderScript[] sliders;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ResetLevel();
    }
    public float GetRandomnessSpeed()
    {
        if(activeLevel)
        {
            return Mathf.Clamp( activeLevel.GetSpeedAdjustment(levelTime / activeLevel.levelLength)* randomnessFactor, -randomnessSpeed, randomnessSpeed);
        }
        return 0;
    }
    public void ResetLevel()
    {
        gameState = GameState.Starting;
        activeJoke = jokes[Random.Range(0, jokes.Length)];
        UiManager.instance.SetupUI();
        foreach (SliderScript slider in sliders)
        {
            slider.Reset();
        }
    }
    public void StartLevel()
    {
        StartLevel(debugLevel);
    }
    void StartLevel(LevelData level)
    {
        activeJoke = jokes[Random.Range(0, jokes.Length)];
        levelTime = level.levelLength;
        gameState = GameState.Playing;
        activeLevel = level;
        UiManager.instance.SetupUI();
        foreach(SliderScript slider in sliders)
        {
            slider.Reset();
        }

    }
    public void SetLoseState()
    {
        gameState = GameState.Fail;
        UiManager.instance.SetupUI();
    }

    public string GetLevelTimeString()
    {
        return $"{(int)levelTime / 60}:{levelTime % 60:00.00}";
    }
    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.Starting:
                if(  Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    StartLevel();
                }
                break;
            case GameState.Playing:
                levelTime = Mathf.Max(0, levelTime - Time.deltaTime);
                timer.text = GetLevelTimeString();
                if (levelTime == 0)
                {
                    gameState = GameState.Win;
                    UiManager.instance.SetupUI();
                    foreach (SliderScript slider in sliders)
                    {
                        slider.Reset();
                        slider.SetWinState();
                    }
                }
                break;


            case GameState.Fail:


                break;
            case GameState.Win:


                break;


        }
    }
}
