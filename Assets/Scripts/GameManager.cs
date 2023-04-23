using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum GameState { Starting, Playing, Fail, Win, punchLine, windGust}
public class GameManager : MonoBehaviour
{
    public static GameState gameState = GameState.Starting;
    public static GameManager instance;
    public Transform projectilePointR, projectilePointL;
    [SerializeField] private LevelData debugLevel;
    [SerializeField] public LevelData[] levels;
    public static int levelIndex= 0;
    public TMP_Text timer;
    private float levelTime;
    [HideInInspector] public LevelData activeLevel;
    [SerializeField] private JokeScriptable[] generalJokes;
    [HideInInspector] public JokeScriptable activeJoke;
    [HideInInspector] public int score = 0;
    [HideInInspector] public static int totalScore = 0;
    private float scoringIncrement = 0.2f;
    private float scoringTimeStamp = 0;
    public SliderScript[] sliders;
    private int winMultiplier = 10;
    public static string saveKey = "levelSave";
    private bool resetThisFrame = false;
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
            return activeLevel.GetSpeedAdjustment(levelTime / activeLevel.levelLength);
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
        SoundController.instance.FadeInMusic();
        foreach (SliderScript slider in sliders)
        {
            slider.Reset();
        }
        resetThisFrame = true;
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
        StartCoroutine(StartLevelCoroutine());
    }

    private IEnumerator StartLevelCoroutine()
    {

        gameState = GameState.windGust;
        UiManager.instance.SetupUI();
        UiManager.instance.wind.SetActive(true);
        SoundController.instance.PlayWoosh();
        yield return new WaitForSecondsRealtime(0.35f);
        SoundController.instance.PlayNoBalance();
        yield return new WaitForSecondsRealtime(0.35f);
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
        foreach(ProjectileData data in activeLevel.projectileData)
        {
            data.launched = false;
        }
    }
    public void SetLoseState()
    {
        gameState = GameState.Fail;
        activeLevel.SaveFallTimes();
        UiManager.instance.SetupUI();
        SoundController.instance.PlayLose();
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
        totalScore += score;
        activeJoke.timesTold++;
        UiManager.instance.SetupUI();
        foreach (SliderScript slider in sliders)
        {
            slider.Reset();
            slider.SetWinState();
        }
        SoundController.instance.PlayJoke(activeJoke);
        activeJoke.SaveTimesTold();
        PlayerPrefs.SetInt(saveKey, levelIndex + 1);
        PlayerPrefs.Save();
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
                if(Keyboard.current.anyKey.wasPressedThisFrame && !resetThisFrame)
                {
                   StartCoroutine(StartLevelCoroutine());
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
                float time =Mathf.Abs(levelTime- activeLevel.levelLength);
                foreach (ProjectileData data in activeLevel.projectileData)
                { 
                    if(!data.launched && time > data.launchTime && activeLevel.projectile != null)
                    {
                        data.launched = true;
                        Transform launchPoint = data.isLeft ? projectilePointL : projectilePointR;
                        projectile projectile = Instantiate(activeLevel.projectile, launchPoint);
                        if (!data.isLeft)
                            projectile.moveSpeed= projectile.moveSpeed*-1f;
                        projectile.transform.localPosition = Vector3.zero;
                    }
                }


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
        resetThisFrame = false;
    }
}
