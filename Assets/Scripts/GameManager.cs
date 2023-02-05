using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum GameState { Starting, Playing, Fail, Win}
public class GameManager : MonoBehaviour
{
    public static GameState gameState = GameState.Starting;
    public static GameManager instance;
    [SerializeField] private LevelData debugLevel;
    [SerializeField] private float randomnessFactor = 4f;
    public TMP_Text timer;
    private float levelTime;
    [HideInInspector] public LevelData activeLevel;
    [SerializeField] private JokeScriptable[] generalJokes;
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
            return Mathf.Clamp( activeLevel.GetSpeedAdjustment(levelTime / activeLevel.levelLength)* randomnessFactor, -activeLevel.randomnessMaxSpeed, activeLevel.randomnessMaxSpeed);
        }
        return 0;
    }
    public void ResetLevel()
    {
        activeLevel = debugLevel;
        gameState = GameState.Starting;
        activeJoke = GetNextJoke();
        SoundController.instance.PlaySetup(activeJoke);
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
        activeLevel = level;
        if (activeJoke== null)
            activeJoke = GetNextJoke();
        // SoundController.instance.PlaySetup(activeJoke);
        levelTime = level.levelLength;
        gameState = GameState.Playing;
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

    private JokeScriptable GetNextJoke()
    {
        List<JokeScriptable> availableJokes = new List<JokeScriptable>(generalJokes);
        if (activeLevel)
            availableJokes.AddRange(activeLevel.jokes);
        int leastTold = int.MaxValue;
        foreach(JokeScriptable joke in availableJokes)
        {
            if (joke.timesTold < leastTold)
            {
                leastTold = joke.timesTold;
            }
        }
        List<JokeScriptable> leastToldJokes = new List<JokeScriptable>();
        foreach(JokeScriptable joke in availableJokes)
        {
            if(joke.timesTold == leastTold)
            {
                leastToldJokes.Add(joke);
            }
        }
        return leastToldJokes[Random.Range(0, leastToldJokes.Count)];

    }


    void OnWin()
    {
        gameState = GameState.Win;
        UiManager.instance.SetupUI();
        foreach (SliderScript slider in sliders)
        {
            slider.Reset();
            slider.SetWinState();
        }
        SoundController.instance.PlayJoke(activeJoke);
        activeJoke.timesTold++;
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
                    OnWin();
                }
                break;


            case GameState.Fail:


                break;
            case GameState.Win:


                break;


        }
    }
}
