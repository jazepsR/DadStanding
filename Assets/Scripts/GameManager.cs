using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum GameState { Starting, Playing, Fail, Win, punchLine}
public class GameManager : MonoBehaviour
{
    public static GameState gameState = GameState.Starting;
    public static GameManager instance;
    [SerializeField] private LevelData debugLevel;
    [SerializeField] private LevelData[] levels;
    private int levelIndex= 0;
    [SerializeField] private float randomnessFactor = 4f;
    public TMP_Text timer;
    private float levelTime;
    [HideInInspector] public LevelData activeLevel;
    [SerializeField] private JokeScriptable[] generalJokes;
    [HideInInspector] public JokeScriptable activeJoke;
    [HideInInspector] public int score = 0;
    private float scoringIncrement = 0.2f;
    private float scoringTimeStamp = 0;
    public SliderScript[] sliders;
    private int winMultiplier = 10;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Application.targetFrameRate = 60;
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
        score = 0;
        activeLevel = levels[levelIndex];
        gameState = GameState.Starting;
        activeJoke = GetNextJoke();
        SoundController.instance.PlaySetup(activeJoke);
        UiManager.instance.SetupUI();
        foreach (SliderScript slider in sliders)
        {
            slider.Reset();
        }
    }

    public void NextLevel()
    {
        levelIndex++;
        if(levelIndex >= levels.Length)
            levelIndex = 0;
        ResetLevel();
    }
    public void StartLevel()
    {
        StartLevel(activeLevel);
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
        scoringTimeStamp = Time.time;
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
        score += winMultiplier * GetScoreIncreaseIncrement();
        UiManager.instance.SetupUI();
        foreach (SliderScript slider in sliders)
        {
            slider.Reset();
            slider.SetWinState();
        }
        SoundController.instance.PlayJoke(activeJoke);
        activeJoke.timesTold++;
    }

    public void OnPunchline()
    {
        gameState = GameState.punchLine;
        UiManager.instance.SetupUI();
        foreach (SliderScript slider in sliders)
        {
            slider.SetPunchlineState();
        }

    }

    public int GetScoreIncreaseIncrement()
    {
         return sliders[0].GetSliderMultiplier();
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
        switch (gameState)
        {
            case GameState.Starting:
                if(  Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    StartLevel(activeLevel);
                }
                break;
            case GameState.Playing:
                if(Time.time - scoringTimeStamp >scoringIncrement)
                {
                    scoringTimeStamp = Time.time;
                    score += GetScoreIncreaseIncrement();
                }
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
