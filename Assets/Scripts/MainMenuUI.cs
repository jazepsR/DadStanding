using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
public class MainMenuUI : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject fade;
    public GameObject clickToStart;
    public GameObject mainMenu;
    public GameObject highScore;
    public GameObject levelMenu;
    public TMP_Text levelSelectTitle;
    public TMP_Text levelSelectDescription;
    public static MainMenuUI instance;
    public static bool openLevel = false;
    public static bool openHighScore = false;
    public List<HighScoreData> highScores;
    public HighScoreElement highScoreElement;
    public Transform highScoreParent;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        highScore.SetActive(false);
        mainMenu.SetActive(true);
        fade.SetActive(false);
        levelMenu.SetActive(false);
        tutorial.SetActive(false);
        clickToStart.SetActive(true);
        if(openLevel)
        {
            mainMenu.SetActive(false);
            highScore.SetActive(false);
            levelMenu.SetActive(true);
            openLevel = false;
        }
        if (openHighScore)
        {
            SetupHighScore();
            mainMenu.SetActive(false);
            highScore.SetActive(true);
            levelMenu.SetActive(false);
            openHighScore = false;
        }
    }

    public void OpenHighScore()
    {
        SetupHighScore();
        mainMenu.SetActive(false);
        highScore.SetActive(true);
        levelMenu.SetActive(false);
    }

    public void SetupHighScore()
    {
        foreach(Transform child in highScoreParent)
        {
            Destroy(child.gameObject);
        }
        if (openHighScore)
        {
            highScores.Add(new HighScoreData("NEW DAD", GameManager.totalScore));
            GameManager.totalScore = 0;
        }
        highScores = highScores.OrderBy(p => p.score).ToList();
        highScores.Reverse();
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

    public HighScoreData(string nameString, int score)
    {
        this.nameString = nameString;
        this.score = score;
    }
}
