using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
    public Button continueButton;
    public GameObject tutorial;
    public GameObject fade;
    public GameObject clickToStart;
    public GameObject mainMenu;
    public GameObject highScore;
    public GameObject levelMenu;
    public TMP_Text levelSelectTitle;
    public TMP_Text levelSelectDescription;
    public GameObject highScoreNameInput;
    public TMP_Text highScoreNameText;
    public TMP_Text highScoreStats;
    public GameObject newGamePopup;
    public static MainMenuUI instance;
    public static bool openLevel = false;
    public static bool openHighScore = false;
    public List<HighScoreData> highScores;
    public List<LevelData> levelData;
    public HighScoreElement highScoreElement;
    public Transform highScoreParent;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SoundController.instance.FadeInMusic();
        highScore.SetActive(false);
        mainMenu.SetActive(true);
        fade.SetActive(false);
        levelMenu.SetActive(false);
        tutorial.SetActive(false);
        highScoreNameInput.gameObject.SetActive(false);
        clickToStart.SetActive(true);
        newGamePopup.SetActive(false);
        continueButton.interactable = PlayerPrefs.GetInt(GameManager.saveKey) > 0;
        if (openLevel)
        {
            mainMenu.SetActive(false);
            highScore.SetActive(false);
            levelMenu.SetActive(true);
            newGamePopup.SetActive(false);
            openLevel = false;
        }
        if (openHighScore)
        {
            SetupHighScore();
            mainMenu.SetActive(false);
            highScore.SetActive(true);
            levelMenu.SetActive(false);
            newGamePopup.SetActive(false);
            openHighScore = false;
            highScoreNameInput.gameObject.SetActive(true);
        }
    }

    public void OpenHighScore()
    {
        SetupHighScore();
        mainMenu.SetActive(false);
        highScore.SetActive(true);
        levelMenu.SetActive(false);
        newGamePopup.SetActive(false);
    }


    public string GetStatsText()
    {
        string toReturn = "";
        int jokesTold = 0;
        int totalJokes = 0;
        int fallTimes = 0;
        int mostFalls = 0;
        string mostFallLevel = "";
        foreach(LevelData level in levelData)
        {
            jokesTold += level.GetJokesTold();
            totalJokes += level.jokes.Length;
            fallTimes += level.GetFallTimes();
            if(level.GetFallTimes()>mostFalls)
            {
                mostFalls = level.GetFallTimes();
                mostFallLevel = level.levelName;
            }
        }
        toReturn += "Jokes told\t\t" + jokesTold + "/" + totalJokes + "\n";
        toReturn += "Times fallen\t" + fallTimes + "\n";
        toReturn += "Times hit\t\t" + PlayerPrefs.GetInt("timesHit") + "\n";
        if (mostFallLevel != "")
            toReturn += "Toughest level\t" + mostFallLevel + "\n";

        return toReturn;
    }
    public void AddHighScore()
    {
        highScores.Add(new HighScoreData(highScoreNameText.text, GameManager.totalScore));
        GameManager.totalScore = 0;
        highScoreNameInput.gameObject.SetActive(false);
        SetupHighScore();
    }

    public void ShowNewGamePopup()
    {
        if (PlayerPrefs.GetInt(GameManager.saveKey) > 0)
        {
            newGamePopup.gameObject.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
            highScore.SetActive(false);
            levelMenu.SetActive(true);
            newGamePopup.SetActive(false);
            openLevel = false;
        }
    }

    public void StartNewGame()
    {
        
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
    public void SetupHighScore()
    {
        foreach(Transform child in highScoreParent)
        {
            Destroy(child.gameObject);
        }
        highScores = highScores.OrderBy(p => p.score).ToList();
        highScores.Reverse();
        highScoreStats.text = GetStatsText();
        for(int i =0; i< 8; i++)
        {
            HighScoreElement highScoreObj = Instantiate(highScoreElement, highScoreParent);
            highScoreObj.Setup(i+1, highScores[i].score, highScores[i].nameString);
        }
    }
    private void Update()
    {     
        if(Keyboard.current.escapeKey.wasPressedThisFrame )
        {
            Application.Quit();
        }else if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            ShowTutorial();
    }

    public void SetLevelSelect(string title, string description)
    {
        levelSelectTitle.text = title;
        levelSelectDescription.text = description;
    }

    public void ClearLevelSelect()
    {
        levelSelectDescription.text = "";
        levelSelectTitle.text = "";
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        clickToStart.SetActive(false);
    }

    public void OpenMainMenu()
    {
        Start();
    }
}
[System.Serializable]
public class HighScoreData
{
    public string nameString;
    public int score;
    [HideInInspector] public bool playerScore;

    public HighScoreData(string nameString, int score)
    {
        this.nameString = nameString;
        this.score = score;
    }
}
