using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    public SliderScript[] sliders;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartLevel(debugLevel);
    }
    public float GetRandomnessSpeed()
    {
        if(activeLevel)
        {
            return activeLevel.GetSpeedAdjustment(levelTime / activeLevel.levelLength)* randomnessFactor;
        }
        return 0;
    }
    public void ResetLevel()
    {
        StartLevel(activeLevel);
    }
    void StartLevel(LevelData level)
    {
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

                break;
            case GameState.Playing:
                levelTime = Mathf.Max(0, levelTime - Time.deltaTime);
                timer.text = GetLevelTimeString();
                if (levelTime == 0)
                {
                    gameState = GameState.Win;
                    UiManager.instance.SetupUI();
                }
                break;


            case GameState.Fail:


                break;
            case GameState.Win:


                break;


        }
    }
}
